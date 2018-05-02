using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases;
using Microsoft.Extensions.Caching.Memory;
using ISPCore.Engine.Middleware;
using ISPCore.Engine.Base;
using ISPCore.Models.RequestsFilter.Domains;
using ISPCore.Models.Security;
using ISPCore.Models.Response;
using ISPCore.Engine.Base.SqlAndCache;
using ISPCore.Engine.Common.Views;

namespace ISPCore.Controllers
{
    public class SecurityToIPtablesController : ControllerToDB
    {
        [HttpGet]
        public IActionResult Index(bool ajax, int page = 1, string ShowIP = null)
        {
            var navPage = new NavPage<BlockedIP>(coreDB.BlockedsIP.FindAll(i => i.BlockingTime > DateTime.Now && (ShowIP == null || i.IP.Contains(ShowIP))), HttpContext, 20, page);
            return View("~/Views/Security/IPtables.cshtml", navPage, ajax);
        }

        #region Remove
        [HttpPost]
        public JsonResult Remove(int Id)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            // Поиск ip
            var item = coreDB.BlockedsIP.Find(Id);
            if (item == null)
                return Json(new Text("Запись не найдена"));

            string IP = item.IP.Replace(".*", "").Replace(":*", "");

            // Удаляем IP с кеша
            memoryCache.Remove(KeyToMemoryCache.IPtables(IP));
            memoryCache.Remove(KeyToMemoryCache.IPtables(IP, item.BlockedHost));

            // Удаляем IP
            coreDB.BlockedsIP.Remove(item);

            // Сохраняем базу
            coreDB.SaveChanges();

            // Очистка кеша IPtables
            if (item.typeBlockIP == TypeBlockIP.UserAgent)
                IPtablesMiddleware.ClearCache();

            // Отдаем результат
            return Json(new TrueOrFalse(true));
        }
        #endregion

        #region Add
        [HttpPost]
        public JsonResult Add(string value, string Description, int BlockingTimeDay, TypeBlockIP typeBlockIP = TypeBlockIP.global, bool IsAPI = false)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            // Коректировка
            if (typeBlockIP == TypeBlockIP.domain)
                typeBlockIP = TypeBlockIP.global;

            // Коректор времени
            if (BlockingTimeDay <= 0)
                BlockingTimeDay = 1;

            #region Проверка IP/UserAgent
            if (typeBlockIP == TypeBlockIP.global)
            {
                // IP-адрес
                string IP = value;

                // Проверка IP
                if (string.IsNullOrWhiteSpace(IP) || (!IP.Contains(".") && !IP.Contains(":")))
                    return Json(new Models.Response.Text("Поле 'Значение' имеет недопустимое значение"));

                // IP для кеша и проверки
                string IPshort = IP.Replace(".*", "").Replace(":*", "");

                // Проверка IP на дубликат
                if (IPtablesMiddleware.CheckIP(IPshort, memoryCache, out _))
                    return Json(new Models.Response.Text("Данный IP-адрес уже заблокирован"));

                // Записываем IP в кеш IPtables
                memoryCache.Set(KeyToMemoryCache.IPtables(IPshort), new IPtables(Description, DateTime.Now.AddDays(BlockingTimeDay)), TimeSpan.FromDays(BlockingTimeDay));
            }
            else
            {
                // Проверка UserAgent
                if (string.IsNullOrWhiteSpace(value))
                    return Json(new Models.Response.Text("Поле 'Значение' имеет недопустимое значение"));
            }
            #endregion

            // Дублируем информацию в SQL
            var blockedIP = new BlockedIP()
            {
                IP = value,
                Description = Description,
                BlockingTime = DateTime.Now.AddDays(BlockingTimeDay),
                typeBlockIP = typeBlockIP
            };
            coreDB.BlockedsIP.Add(blockedIP);

            // Сохраняем SQL
            coreDB.SaveChanges();

            // Очистка кеша IPtables
            if (typeBlockIP == TypeBlockIP.UserAgent)
                IPtablesMiddleware.ClearCache();

            if (IsAPI)
                return Json(new TrueOrFalse(true));

            // Отдаем результат
            return Json(new Html($@"<tr class='elemDelete'>
                                        <td class='text-left table-products'>
                                            <strong>{value}</strong>
                                        </td>

                                        <td>{(typeBlockIP == TypeBlockIP.global ? "IP-адрес" : "User Agent")}</td>
                                        <td>{Description}</td>
                                        <td>{DateTime.Now.AddDays(BlockingTimeDay).ToString("dd.MM.yyyy H:mm")}</td>

                                        <td style='text-align: right;' class='table-products btn-icons'>" +
                                        "<a onclick=\"return deleteElement(this,'/security/iptables/remove',{Id:'" + blockedIP.Id + "'});\" class=\"btn nopadding-nomargin\"><i class=\"fa fa-trash-o\"></i></a>" + 
                                        @"</td>
                                    </tr>"));
        }
        #endregion
    }
}
