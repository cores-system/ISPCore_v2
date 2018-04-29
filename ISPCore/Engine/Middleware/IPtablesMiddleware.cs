using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using ISPCore.Models.Security;
using System.Text.RegularExpressions;
using ISPCore.Models.Databases;
using ISPCore.Models.RequestsFilter.Domains;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using ISPCore.Models.Base;
using ISPCore.Engine.Base.SqlAndCache;

namespace ISPCore.Engine.Middleware
{
    public class IPtablesMiddleware
    {
        #region CheckIP
        public static bool CheckIP(string RemoteIpAddress, IMemoryCache memoryCache, out IPtables data, string BlockedHost = null)
        {
            // Результат кеша
            string memKey = $"IPtablesMiddleware.CheckIP:local-{RemoteIpAddress}:{BlockedHost}";
            if (memoryCache.TryGetValue(memKey, out bool cacheResult)) {
                data = new IPtables();
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
                if (BlockedHost == null)
                {
                    if (memoryCache.TryGetValue(KeyToMemoryCache.IPtables(tmp.Remove(0, 1)), out data))
                    {
                        memoryCache.Set(memKey, true, TimeSpan.FromMilliseconds(300));
                        return true;
                    }
                }
                else
                {
                    if (memoryCache.TryGetValue(KeyToMemoryCache.IPtables(tmp.Remove(0, 1), BlockedHost), out data))
                    {
                        memoryCache.Set(memKey, true, TimeSpan.FromMilliseconds(300));
                        return true;
                    }
                }
            }

            data = new IPtables();
            memoryCache.Set(memKey, false, TimeSpan.FromMilliseconds(300));
            return false;
        }
        #endregion

        #region ClearCache
        public static void ClearCache()
        {
            userAgentsRegex = null;
        }
        #endregion

        #region CheckUserAgent
        private static string userAgentsRegex = null;

        public static bool CheckUserAgent(string userAgent)
        {
            if (userAgentsRegex != null)
            {
                if (userAgentsRegex == "^$")
                    return false;

                return Regex.IsMatch(userAgent, userAgentsRegex, RegexOptions.IgnoreCase);
            }

            // Меняем режим доступа к SQL
            SqlToMode.SetMode(SqlMode.Read);

            // Подключаемся к базе
            using (CoreDB coreDB = Service.Get<CoreDB>())
            {
                List<string> mass = new List<string>();
                foreach (var blockedIP in coreDB.BlockedsIP.AsNoTracking())
                {
                    if (blockedIP.BlockingTime > DateTime.Now && blockedIP.typeBlockIP == TypeBlockIP.UserAgent) {
                        mass.Add(blockedIP.IP);
                    }
                }

                // Клеим данные в строчку
                userAgentsRegex = mass.Count == 0 ? "^$" : $"({string.Join("|", mass)})";
            }

            // Меняем режим доступа к SQL
            SqlToMode.SetMode(SqlMode.ReadOrWrite);

            // Результат
            return Regex.IsMatch(userAgent, userAgentsRegex, RegexOptions.IgnoreCase);
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


        private readonly RequestDelegate next;
        private readonly IMemoryCache memoryCache;

        public IPtablesMiddleware(RequestDelegate _next, IMemoryCache memCache)
        {
            memoryCache = memCache;
            next = _next;
        }


        public Task Invoke(HttpContext httpContext)
        {
            // Поиск IP в кеше для блокировки пользователя
            if (CheckIP(httpContext.Connection.RemoteIpAddress.ToString(), memoryCache, out IPtables data))
            {
                httpContext.Response.StatusCode = 401;
                if (Startup.cmd.StatusCode.IPtables)
                    return Task.FromResult(true);

                httpContext.Response.ContentType = "text/html";
                return httpContext.Response.WriteAsync(BlockedToHtml(httpContext.Connection.RemoteIpAddress.ToString(), data.Description, data.TimeExpires));
            }

            // Идем дальше
            return next(httpContext);
        }
    }
    

    public static class IPtablesMiddlewareExtensions
    {
        public static IApplicationBuilder UseIPtablesMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<IPtablesMiddleware>();
        }
    }
}
