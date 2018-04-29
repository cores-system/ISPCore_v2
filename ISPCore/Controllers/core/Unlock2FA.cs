using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ISPCore.Engine.Auth;
using ISPCore.Engine.Hash;
using System.Text.RegularExpressions;
using System.Net;
using ISPCore.Engine.core.Cache.CheckLink;
using ISPCore.Engine.RequestsFilter.Access;
using ISPCore.Engine.Base;
using ISPCore.Models.RequestsFilter.Domains;
using ModelCache = ISPCore.Models.core.Cache.CheckLink;
using ISPCore.Engine.Middleware;
using ISPCore.Models.RequestsFilter.Monitoring;
using ISPCore.Engine;
using ISPCore.Models.RequestsFilter.Domains.Log;
using ISPCore.Models.Security;
using CheckRequest = ISPCore.Engine.core.Check.Request;
using ISPCore.Models.RequestsFilter.Base.Enums;
using ISPCore.Models.RequestsFilter.Access;
using ISPCore.Engine.Base.SqlAndCache;

namespace ISPCore.Controllers.core
{
    public class CoreUnlock2FAController : Controller
    {
        [HttpPost]
        public JsonResult Index(string password, string host, string method, string uri, string referer, string hash)
        {
            // Декодируем uri, referer и FormData
            uri = WebUtility.UrlDecode(uri);
            referer = WebUtility.UrlDecode(referer);

            #region Проверка переданых параметров
            if (string.IsNullOrWhiteSpace(password))
                return Json(new Models.Response.Text("Введите пароль"));

            if (SHA256.Text($"{host}:{method}:{uri}:{referer}:{PasswdTo.salt}") != hash)
                return Json(new Models.Response.Text("Хеш сумма не совпадает"));
            #endregion

            // User-Agent
            string userAgent = string.Empty;
            if (HttpContext.Request.Headers.TryGetValue("User-Agent", out var tmp_userAgent))
                userAgent = tmp_userAgent.ToString();

            // Переменные
            string IP = HttpContext.Connection.RemoteIpAddress.ToString();               // IP адрес пользователя
            string HostConvert = Regex.Replace(host.ToLower().Trim(), "^www\\.", "");    // Спецальный host
            TypeBlockIP typeBlockIP = TypeBlockIP.global;                                // Блокировка IP в 'Брандмауэр' глобально или только для домена
            var memoryCache = Service.Get<IMemoryCache>();                               // Кеш IMemoryCache

            #region ModelCache.Domain
            ModelCache.Domain domain = new ModelCache.Domain();
            int DomainID = ISPCache.DomainToID(HostConvert);
            if (DomainID != 0)
            {
                // Достаем данные для домена из кеша
                domain = ISPCache.GetDomain(DomainID);
                typeBlockIP = domain.typeBlockIP;
            }
            #endregion

            #region Проверяем IP в блокировке IPtables по домену
            if (IPtablesMiddleware.CheckIP(IP, memoryCache, out IPtables BlockedData, HostConvert))
            {
                // Логируем пользователя
                AddJurnalToIPtables(domain, IP, host, method, userAgent, referer, uri);
                CheckRequest.SetCountRequestToHour(TypeRequest._200, host, domain.confToLog.EnableCountRequest);

                // Отдаем ответ
                return Json(new Models.Response.Text(BlockedData.Description));
            }
            #endregion

            // Проверяем пароль
            if (SHA256.Text(password) == PasswdTo.FA || (!string.IsNullOrWhiteSpace(domain.Auth2faToPasswd) && SHA256.Text(password) == domain.Auth2faToPasswd))
            {
                // Добавляем информацию о разрешеном доступе, для вывода информации и отмены доступа
                AccessIP.Add(IP, host, DateTime.Now.AddHours(12), domain.Auth2faToAccess == Auth2faToAccess.FullAccess ? AccessType.all : AccessType.Is2FA);

                // Добовляем IP в белый список на 12 часа
                string keyToAccess = domain.Auth2faToAccess == Auth2faToAccess.FullAccess ? KeyToMemoryCache.CheckLinkWhitelistToAll(host, IP) : KeyToMemoryCache.CheckLinkWhitelistTo2FA(host, IP);
                memoryCache.Set(keyToAccess, (byte)1, TimeSpan.FromHours(12));

                // Записываем данные авторизации в журнал
                AddToJurnal2FA(domain, IP, host, method, uri, referer, "Успешная авторизация");

                // Считаем статистику запросов
                CheckRequest.SetCountRequestToHour(TypeRequest._2fa, host, domain.confToLog.EnableCountRequest);

                // Удаляем список неудачных попыток
                LimitLogin.SuccessAuthorization(IP);

                // Отдаем результат
                return Json(new Models.Response.TrueOrFalse(true));
            }

            // Записываем данные авторизации в журнал
            AddToJurnal2FA(domain, IP, host, method, uri, referer, "Неудачная попытка авторизации");

            // Считаем статистику запросов
            CheckRequest.SetCountRequestToHour(TypeRequest._2fa, host, domain.confToLog.EnableCountRequest);

            // Записываем в базу IP адрес пользователя, который ввел неправильно пароль
            LimitLogin.FailAuthorization(IP, typeBlockIP, HostConvert);

            // Отдаем результат
            return Json(new Models.Response.Text("Неверный пароль"));
        }

        #region AddToJurnal2FA
        private void AddToJurnal2FA(ModelCache.Domain domain, string ip, string host, string method, string uri, string referer, string msg)
        {
            // Игнорирование логов
            if (domain.confToLog.Jurn2FA == WriteLogMode.off || Regex.IsMatch(uri, domain.IgnoreLogToRegex, RegexOptions.IgnoreCase))
                return;

            // GeoIP пользователя
            var geoIP = (Country: "Disabled", City: "Disabled", Region: "Disabled");
            if (domain.confToLog.EnableGeoIP)
                geoIP = GeoIP2.City(ip);

            // Данные для записи в журнал
            var model = new Jurnal2FA()
            {
                IP = ip,
                Host = host,
                Uri = uri,
                Method = method,
                Msg = msg,
                UserAgent = Request.Headers["User-Agent"],
                Referer = referer,
                Country = geoIP.Country,
                City = geoIP.City,
                Region = geoIP.Region,
                Time = DateTime.Now,
            };

            // Записываем данные в журнал
            switch (domain.confToLog.Jurn2FA)
            {
                case WriteLogMode.File:
                    WriteLogTo.FileStreamTo2faAuth(model);
                    break;
                case WriteLogMode.SQL:
                    WriteLogTo.SQL(model);
                    break;
                case WriteLogMode.all:
                    WriteLogTo.SQL(model);
                    WriteLogTo.FileStreamTo2faAuth(model);
                    break;
            }
        }
        #endregion

        #region AddJurnalToIPtables
        private void AddJurnalToIPtables(ModelCache.Domain domain, string IP, string host, string method, string userAgent, string Referer, string uri)
        {
            // Игнорирование логов
            if (domain.confToLog.Jurn200 == WriteLogMode.off || Regex.IsMatch(uri, domain.IgnoreLogToRegex, RegexOptions.IgnoreCase))
                return;

            var geoIP = (Country: "Disabled", City: "Disabled", Region: "Disabled");
            if (domain.confToLog.EnableGeoIP)
                geoIP = GeoIP2.City(IP);

            var model = new Jurnal200()
            {
                typeJurn = TypeJurn200.IPtables,
                IP = IP,
                Host = host,
                Method = method,
                Uri = uri,
                FormData = null,
                UserAgent = userAgent,
                Referer = Referer,
                Country = geoIP.Country,
                City = geoIP.City,
                Region = geoIP.Region,
                Time = DateTime.Now
            };

            // Записываем данные в журнал
            switch (domain.confToLog.Jurn200)
            {
                case WriteLogMode.File:
                    WriteLogTo.FileStream(model);
                    break;
                case WriteLogMode.SQL:
                    WriteLogTo.SQL(model);
                    break;
                case WriteLogMode.all:
                    WriteLogTo.FileStream(model);
                    break;
            }
        }
        #endregion
    }
}
