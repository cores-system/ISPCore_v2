using ISPCore.Engine.Base;
using ISPCore.Engine.Base.SqlAndCache;
using ISPCore.Engine.Security;
using ISPCore.Models.Databases;
using ISPCore.Models.Databases.json;
using ISPCore.Models.RequestsFilter.Domains;
using ISPCore.Models.RequestsFilter.Domains.Log;
using ISPCore.Models.RequestsFilter.Monitoring;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ModelCache = ISPCore.Models.core.Cache.CheckLink;
using ModelIPtables = ISPCore.Models.Security.IPtables;

namespace ISPCore.Engine.core.Check
{
    public partial class Request
    {
        static JsonDB jsonDB = Service.Get<JsonDB>();
        static IMemoryCache memoryCache = Service.Get<IMemoryCache>();

        #region SetCountRequestToHour
        public static void SetCountRequestToHour(TypeRequest type, string host, bool EnableCountRequest)
        {
            #region Локальный метод - "SetCount"
            void SetCount(NumberOfRequestHour dt)
            {
                switch (type)
                {
                    case TypeRequest._200:
                        dt.Count200++;
                        break;
                    case TypeRequest._303:
                        dt.Count303++;
                        break;
                    case TypeRequest._403:
                        dt.Count403++;
                        break;
                    case TypeRequest._401:
                        dt.Count401++;
                        break;
                    case TypeRequest._500:
                        dt.Count500++;
                        break;
                    case TypeRequest._2fa:
                        dt.Count2FA++;
                        break;
                    case TypeRequest.IPtables:
                        dt.CountIPtables++;
                        break;
                }
            }
            #endregion

            if (EnableCountRequest)
            {
                string keyNumberOfRequestToHour = KeyToMemoryCache.IspNumberOfRequestToHour(DateTime.Now);
                if (memoryCache.TryGetValue(keyNumberOfRequestToHour, out IDictionary<string, NumberOfRequestHour> DataNumberOfRequestDay))
                {
                    // Если хост есть в кеше
                    if (DataNumberOfRequestDay.TryGetValue(host, out NumberOfRequestHour dtValue))
                    {
                        SetCount(dtValue);
                    }

                    // Если хоста нету в кеше
                    else
                    {
                        var dt = new NumberOfRequestHour();
                        dt.Time = DateTime.Now;
                        SetCount(dt);
                        DataNumberOfRequestDay.Add(host, dt);
                    }
                }
                else
                {
                    // Считаем запрос
                    var dt = new NumberOfRequestHour();
                    dt.Time = DateTime.Now;
                    SetCount(dt);

                    // Создаем кеш
                    memoryCache.Set(keyNumberOfRequestToHour, new Dictionary<string, NumberOfRequestHour>() { [host] = dt }, TimeSpan.FromHours(2));
                }
            }
        }
        #endregion

        #region SetCountRequestToMinute
        public static void SetCountRequestToMinute(TypeRequest type, string host, int DomainID, bool EnableCountRequest)
        {
            if (type != TypeRequest.All && type != TypeRequest._303)
                return;

            #region Локальный метод - "SetCount"
            void SetCount(NumberOfRequestMinute dt)
            {
                switch (type)
                {
                    case TypeRequest._303:
                        dt.Count303++;
                        break;
                    case TypeRequest.All:
                        dt.NumberOfRequest++;
                        break;
                }
            }
            #endregion

            if (EnableCountRequest)
            {
                string keyNumberOfRequestToMinutes = KeyToMemoryCache.IspNumberOfRequestToMinutes(DateTime.Now);
                if (memoryCache.TryGetValue(keyNumberOfRequestToMinutes, out IDictionary<string, NumberOfRequestMinute> NumberOfRequestsPerMinute))
                {
                    // Если хост есть в кеше
                    if (NumberOfRequestsPerMinute.TryGetValue(host, out NumberOfRequestMinute dtValue))
                    {
                        SetCount(dtValue);
                    }

                    // Если хоста нету в кеше
                    else
                    {
                        var dt = new NumberOfRequestMinute();
                        dt.DomainID = DomainID;
                        SetCount(dt);
                        NumberOfRequestsPerMinute.Add(host, dt);
                    }
                }
                else
                {
                    // Считаем запрос
                    var dt = new NumberOfRequestMinute();
                    dt.DomainID = DomainID;
                    SetCount(dt);

                    // Создаем кеш
                    memoryCache.Set(keyNumberOfRequestToMinutes, new Dictionary<string, NumberOfRequestMinute>() { [host] = dt }, TimeSpan.FromMinutes(3));
                }
            }
        }
        #endregion

        #region SetBlockedToIPtables
        public static void SetBlockedToIPtables(ModelCache.Domain Domain, string IP, string host, string Msg, DateTime Expires, string uri, string userAgent, string PtrHostName)
        {
            if (Domain.typeBlockIP == TypeBlockIP.Triggers)
            {
                // Что-бы в статистике не считать лишний раз +1 к блокировке 
                string memKey = $"local-fb482608:SetBlockedToIPtables-{IP}";
                if (memoryCache.TryGetValue(memKey, out _))
                    return;

                // Данные для статистики
                SetCountRequestToHour(TypeRequest._401, host, Domain.confToLog.EnableCountRequest);
                memoryCache.Set(memKey, (byte)0, Expires);
            }
            else
            {
                // Если IP уже заблокирован
                if ((Domain.typeBlockIP == TypeBlockIP.domain && memoryCache.TryGetValue(KeyToMemoryCache.IPtables(IP, host), out _)) || 
                    (Domain.typeBlockIP == TypeBlockIP.global && Engine.Security.IPtables.CheckIP(IP, memoryCache, out _)))
                    return;

                // Данные для статистики
                SetCountRequestToHour(TypeRequest._401, host, Domain.confToLog.EnableCountRequest);

                #region Записываем IP в кеш IPtables
                switch (Domain.typeBlockIP)
                {
                    case TypeBlockIP.global:
                        IPtables.AddIPv4Or6(IP, new ModelIPtables(Msg, Expires));
                        break;
                    case TypeBlockIP.domain:
                        memoryCache.Set(KeyToMemoryCache.IPtables(IP, host), new ModelIPtables(Msg, Expires), Expires);
                        break;
                }
                #endregion

                // Дублируем информацию в SQL
                WriteLogTo.SQL(new BlockedIP()
                {
                    IP = IP,
                    BlockingTime = Expires,
                    Description = Msg,
                    typeBlockIP = Domain.typeBlockIP,
                    BlockedHost = host
                });
            }

            // Игнорирование логов
            if (Domain.confToLog.IsActive && !Regex.IsMatch(uri, Domain.IgnoreLogToRegex, RegexOptions.IgnoreCase))
            {
                var geoIP = (Country: "Disabled", City: "Disabled", Region: "Disabled");
                if (Domain.confToLog.EnableGeoIP)
                    geoIP = GeoIP2.City(IP);

                // Модель
                Jurnal401 model = new Jurnal401()
                {
                    Host = host,
                    IP = IP,
                    Msg = Msg,
                    Ptr = PtrHostName,
                    UserAgent = userAgent,
                    Country = geoIP.Country,
                    City = geoIP.City,
                    Region = geoIP.Region,
                    Time = DateTime.Now
                };

                // Записываем данные в журнал
                switch (Domain.confToLog.Jurn401)
                {
                    case WriteLogMode.File:
                        WriteLogTo.FileStream(model);
                        break;
                    case WriteLogMode.SQL:
                        WriteLogTo.SQL(model);
                        break;
                    case WriteLogMode.all:
                        WriteLogTo.SQL(model);
                        WriteLogTo.FileStream(model);
                        break;
                }
            }
        }
        #endregion
    }
}
