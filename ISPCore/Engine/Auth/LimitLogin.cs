using System;
using System.Collections.Generic;
using System.Linq;
using ISPCore.Models.Databases;
using ISPCore.Models.Databases.json;
using Microsoft.Extensions.Caching.Memory;
using ISPCore.Models.RequestsFilter.Domains;
using ISPCore.Models.Security;
using ISPCore.Engine.Base.SqlAndCache;

namespace ISPCore.Engine.Auth
{
    public static class LimitLogin
    {
        /// <summary>
        /// Неудачная авторизация
        /// </summary>
        /// <param name="Settings"></param>
        /// <param name="memoryCache"></param>
        /// <param name="RemoteIpAddress">IP адрес пользователя</param>
        /// <param name="typeBlockIP">Блокировка IP в 'Брандмауэр' глобально или только для домена</param>
        /// <param name="host"></param>
        public static void FailAuthorization(string RemoteIpAddress, TypeBlockIP typeBlockIP, string host = null)
        {
            var memoryCache = Service.Get<IMemoryCache>();
            string key = KeyToMemoryCache.LimitLogin(RemoteIpAddress);
            if (memoryCache.TryGetValue(key, out List<DateTime> TimeArray))
            {
                // Актуальные записи
                var array = (from time in TimeArray where (DateTime.Now - time).TotalMinutes < 10 select time).ToList();

                // База JsonDB
                var jsonDB = Service.Get<JsonDB>();

                // Блокировка IP
                if ((array.Count + 1) >= jsonDB.Security.CountAccess)
                {
                    // Записываем IP в кеш IPtables
                    var data = new IPtables("Перебор паролей", DateTime.Now.AddMinutes(jsonDB.Security.BlockingTime));
                    Engine.Security.IPtables.AddIPv4Or6(RemoteIpAddress, data, typeBlockIP, host);

                    // Удаляем ключ LimitLogin
                    memoryCache.Remove(key);

                    // Дублируем информацию в SQL
                    WriteLogTo.SQL(new BlockedIP()
                    {
                        IP = RemoteIpAddress,
                        BlockingTime = DateTime.Now.AddMinutes(jsonDB.Security.BlockingTime),
                        Description = "Перебор паролей",
                        typeBlockIP = typeBlockIP,
                        BlockedHost = host
                    });
                    return;
                }

                // Обновление записей
                array.Add(DateTime.Now);
                memoryCache.Set(key, array, TimeSpan.FromMinutes(10));
            }
            else
            {
                memoryCache.Set(key, new List<DateTime>() { DateTime.Now }, TimeSpan.FromMinutes(10));
            }
        }


        /// <summary>
        /// Успешнная авторизация
        /// </summary>
        /// <param name="memoryCache"></param>
        /// <param name="RemoteIpAddress">IP адрес пользователя</param>
        public static void SuccessAuthorization(string RemoteIpAddress)
        {
            var memoryCache = Service.Get<IMemoryCache>();
            memoryCache.Remove(KeyToMemoryCache.LimitLogin(RemoteIpAddress));
        }


        /// <summary>
        /// Защита от перебора пароля по кукам
        /// </summary>
        /// <param name="act"></param>
        /// <param name="authCookie"></param>
        /// <param name="RemoteIpAddress">IP адрес пользователя</param>
        public static void FailCookieAuthorization(string act, string authCookie, string RemoteIpAddress)
        {
            var memoryCache = Service.Get<IMemoryCache>();
            string key = $"LimitLoginCookie:{act}-{RemoteIpAddress}";
            if (memoryCache.TryGetValue(key, out string cookie))
            {
                if (cookie == authCookie)
                    return;

                memoryCache.Set(key, authCookie, TimeSpan.FromMinutes(10));
                FailAuthorization(RemoteIpAddress, TypeBlockIP.global);
            }
            else
            {
                memoryCache.Set(key, authCookie, TimeSpan.FromMinutes(10));
            }
        }
    }
}
