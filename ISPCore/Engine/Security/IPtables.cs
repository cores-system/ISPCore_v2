using System;
using Microsoft.Extensions.Caching.Memory;
using System.Text.RegularExpressions;
using ISPCore.Models.Databases;
using ISPCore.Models.RequestsFilter.Domains;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using ISPCore.Models.Base;
using ISPCore.Engine.Base.SqlAndCache;
using ModelIPtables = ISPCore.Models.Security.IPtables;

namespace ISPCore.Engine.Security
{
    public static class IPtables
    {
        #region CheckIP
        public static bool CheckIP(string RemoteIpAddress, IMemoryCache memoryCache, out ModelIPtables data, string BlockedHost = null)
        {
            // Блокировка по домену
            if (BlockedHost != null)
                return memoryCache.TryGetValue(KeyToMemoryCache.IPtables(RemoteIpAddress, BlockedHost), out data);

            // Результат кеша
            string memKey = $"IPtablesMiddleware.CheckIP:local-{RemoteIpAddress}:{BlockedHost}";
            if (memoryCache.TryGetValue(memKey, out bool cacheResult))
            {
                data = new ModelIPtables();
                return cacheResult;
            }

            string[] mass;
            string patch, tmp = "";

            // IPv6
            if (RemoteIpAddress.Contains(":"))
            {
                mass = RemoteIpAddress.Split(':');
                patch = ":";
            }

            // IPv4
            else
            {
                mass = RemoteIpAddress.Split('.');
                patch = ".";
            }

            // Проверяем IP в кеше
            foreach (var ip in mass)
            {
                tmp += patch + ip;
                if (memoryCache.TryGetValue(KeyToMemoryCache.IPtables(tmp.Remove(0, 1)), out data))
                {
                    memoryCache.Set(memKey, true, TimeSpan.FromMilliseconds(300));
                    return true;
                }
            }

            data = new ModelIPtables();
            memoryCache.Set(memKey, false, TimeSpan.FromMilliseconds(300));
            return false;
        }
        #endregion

        #region CheckUserAgent
        private static string userAgentsRegex = "^$";

        public static bool CheckUserAgent(string userAgent)
        {
            if (userAgentsRegex == "^$")
                return false;

            return Regex.IsMatch(userAgent, userAgentsRegex, RegexOptions.IgnoreCase);
        }
        #endregion

        #region UpdateCacheToUserAgent
        public static void UpdateCacheToUserAgent()
        {
            // Меняем режим доступа к SQL
            SqlToMode.SetMode(SqlMode.Read);

            // Подключаемся к базе
            using (CoreDB coreDB = Service.Get<CoreDB>())
            {
                List<string> mass = new List<string>();
                foreach (var blockedIP in coreDB.BlockedsIP.AsNoTracking())
                {
                    if (blockedIP.BlockingTime > DateTime.Now && blockedIP.typeBlockIP == TypeBlockIP.UserAgent)
                    {
                        mass.Add(blockedIP.IP);
                    }
                }

                // Клеим данные в строчку
                userAgentsRegex = mass.Count == 0 ? "^$" : $"({string.Join("|", mass)})";
            }

            // Меняем режим доступа к SQL
            SqlToMode.SetMode(SqlMode.ReadOrWrite);
        }
        #endregion

        #region BlockedToHtml
        public static string BlockedToHtml(string RemoteIpAddress, string description, DateTime TimeExpires) => @"<!DOCTYPE html>
<html lang='ru-RU'>
<head>
    <title>Доступ запрещен</title>
    <meta http-equiv='Content-Type' content='text/html; charset=utf-8'>
    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
    <meta name='viewport' content='width=device-width, initial-scale=1, user-scalable=no'>
    <script type='text/javascript' src='/statics/jquery.min.js'></script>
    <link rel='stylesheet' href='/statics/style.css'>
</head>
<body>
    <div class='error'>
        <div class='error-block'>

            <div class='code'>401</div>
                <div class='title'>" + $"Доступ запрещен</div><pre>Ваш IP {RemoteIpAddress} заблокирован<br />{description}" +
                                       $"<br /><br />Доступ будет открыт {TimeExpires.ToString("dd.MM.yyyy")} в {TimeExpires.AddMinutes(1).ToString("H:mm")}</pre>" +

            @"
            <div class='copyright'>
                <div>
                    &copy; 2018 <strong>ISPCore</strong>. All rights reserved.
                </div>
                <div>
                    <a href='/'>Главная сайта</a> / <a href='http://core-system.org/' target='_blank'>Core System</a>
                </div>
            </div>

        </div>
    </div>
</body>
</html>
";

        public static string BlockedHtmlToUserAgent(string userAgent) => @"<!DOCTYPE html>
<html lang='ru-RU'>
<head>
    <title>Доступ запрещен</title>
    <meta http-equiv='Content-Type' content='text/html; charset=utf-8'>
    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
    <meta name='viewport' content='width=device-width, initial-scale=1, user-scalable=no'>
    <script type='text/javascript' src='/statics/jquery.min.js'></script>
    <link rel='stylesheet' href='/statics/style.css'>
</head>
<body>
    <div class='error'>
        <div class='error-block'>

            <div class='code'>401</div>
                <div class='title'>" + $"Доступ запрещен</div><pre>Ваш User-Agent в списке запрещенных</pre>" +
            @"
            <div class='copyright'>
                <div>
                    &copy; 2018 <strong>ISPCore</strong>. All rights reserved.
                </div>
                <div>
                    <a href='/'>Главная сайта</a> / <a href='http://core-system.org/' target='_blank'>Core System</a>
                </div>
            </div>

        </div>
    </div>
</body>
</html>
";
        #endregion
    }
}
