 using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using ISPCore.Models.RequestsFilter.Access;
using ISPCore.Engine.RequestsFilter.Access;
using ISPCore.Engine.Base;
using ISPCore.Engine;
using System.Text.RegularExpressions;
using ISPCore.Models.Response;
using ISPCore.Engine.Base.SqlAndCache;
using ISPCore.Engine.Common.Views;
using ISPCore.Models.Databases;

namespace ISPCore.Controllers
{
    public class RequestsFilterToAccessController : ControllerToDB
    {
        #region Index 
        [HttpGet]
        public IActionResult Index(bool ajax, int page = 1, string search = null)
        {
            var db = AccessIP.List().Where(i => search == null || i.IP.Contains(search) || i.host.Contains(search));
            var navPage = new NavPage<AccessIPModel>(db, HttpContext, 20, page);
            return View("~/Views/RequestsFilter/Access.cshtml", navPage, ajax);
        }
        #endregion

        #region Open
        [HttpPost]
        public JsonResult Open(string host, string IP, int AccessTimeToMinutes, AccessType accessType)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Modal("Операция недоступна в демо-режиме"));
            #endregion

            // Проверка IP
            if (string.IsNullOrWhiteSpace(IP) || (!IP.Contains(".") && !IP.Contains(":")) || (IP.Contains(".") && !Regex.IsMatch(IP, @"^[0-9]+\.[0-9]+\.[0-9]+\.([0-9]+|\*)$")))
                return Json(new Modal("Поле 'IP-адрес' имеет недопустимое значение"));

            // Проверка домена
            if (accessType != AccessType.allDomain && string.IsNullOrWhiteSpace(host))
                return Json(new Modal("Поле 'Домен' не может быть пустым"));

            // Коректор времени
            if (AccessTimeToMinutes <= 0)
                AccessTimeToMinutes = 2160;

            // Достаем IMemoryCache
            var memoryCache = Service.Get<IMemoryCache>();

            // Добавляем данные в список разрешенных IP, для вывода информации и отмены доступа
            AccessIP.Add(IP, accessType == AccessType.allDomain ? "все домены" : host, DateTime.Now.AddMinutes(AccessTimeToMinutes), accessType);

            // IP для кеша
            string ipCache = IP.Replace(".*", "").Replace(":*", "");

            // Добовляем IP в белый список
            switch (accessType)
            {
                case AccessType.all:
                    memoryCache.Set(KeyToMemoryCache.CheckLinkWhitelistToAll(host, ipCache), (byte)1, TimeSpan.FromMinutes(AccessTimeToMinutes));
                    break;
                case AccessType.Is2FA:
                    memoryCache.Set(KeyToMemoryCache.CheckLinkWhitelistTo2FA(host, ipCache), (byte)1, TimeSpan.FromMinutes(AccessTimeToMinutes));
                    break;
                case AccessType.allDomain:
                    memoryCache.Set(KeyToMemoryCache.CheckLinkWhitelistToAllDomain(ipCache), (byte)1, TimeSpan.FromMinutes(AccessTimeToMinutes));
                    break;
            }

            // Отдаем ответ
            return Json(new Modal($"Разрешен доступ для '{IP}' на {AccessTimeToMinutes} {EndOfText.get("минуту", "минуты", "минут", AccessTimeToMinutes)}", true));
        }
        #endregion

        #region Remove
        [HttpPost]
        public JsonResult Remove(string IP, string host, AccessType accessType)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            // Удаляем IP из списка
            AccessIP.Remove(IP, host, accessType);

            // Достаем IMemoryCache
            var memoryCache = Service.Get<IMemoryCache>();

            // IP для кеша
            string ipCache = IP.Replace(".*", "").Replace(":*", "");

            // Удаляем запись с кеша
            switch (accessType)
            {
                case AccessType.all:
                    memoryCache.Remove(KeyToMemoryCache.CheckLinkWhitelistToAll(host, ipCache));
                    break;
                case AccessType.Is2FA:
                    memoryCache.Remove(KeyToMemoryCache.CheckLinkWhitelistTo2FA(host, ipCache));
                    break;
                case AccessType.allDomain:
                    memoryCache.Remove(KeyToMemoryCache.CheckLinkWhitelistToAllDomain(ipCache));
                    break;
            }

            // Отдаем результат
            return Json(new TrueOrFalse(true));
        }
        #endregion
    }
}
