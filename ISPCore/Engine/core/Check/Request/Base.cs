using ISPCore.Engine.Base.SqlAndCache;
using ISPCore.Models.Databases.json;
using ISPCore.Models.RequestsFilter.Monitoring;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

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
    }
}
