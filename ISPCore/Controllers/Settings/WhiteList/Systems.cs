using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Engine.Base;
using ISPCore.Models.Response;
using ISPCore.Models.Databases;
using Microsoft.EntityFrameworkCore;
using ISPCore.Engine.Base.SqlAndCache;
using ISPCore.Models.Base;
using ISPCore.Engine.Common.Views;

namespace ISPCore.Controllers
{
    public class SettingsToSystemWhiteList : ControllerToDB
    {
        [HttpGet]
        public IActionResult Index(bool ajax, int page = 1)
        {
            var navPage = new NavPage<WhitePtrIP>(coreDB.WhitePtrIPs.AsNoTracking(), HttpContext, 20, page);
            return View("~/Views/Settings/WhiteList/Systems.cshtml", navPage, ajax);
        }

        #region Remove
        [HttpPost]
        public JsonResult Remove(int Id)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion
            
            // Удаляем домен
            if (coreDB.WhitePtrIPs.FindItem(i => i.Id == Id) is WhitePtrIP item)
            {
                // Сохраняем IP
                string IPv4Or6 = item.IPv4Or6;

                // Удаляем запись в SQL
                coreDB.WhitePtrIPs.Remove(item);
                coreDB.SaveChanges();

                // Удаляем IP в кеше
                memoryCache.Remove(KeyToMemoryCache.WhitePtrIP(IPv4Or6));

                // Отдаем результат
                return Json(new TrueOrFalse(true));
            }

            // Ошибка
            return Json(new Text("Ошибка ;("));
        }
        #endregion
    }
}
