using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ISPCore.Engine.Base;
using ISPCore.Models.Response;
using ISPCore.Models.Databases;
using Microsoft.EntityFrameworkCore;
using ISPCore.Engine.Base.SqlAndCache;
using ISPCore.Models.Base;

namespace ISPCore.Controllers
{
    public class SettingsToSystemWhiteList : ControllerToDB
    {
        [HttpGet]
        public IActionResult Index(bool ajax)
        {
            ViewData["ajax"] = ajax;
            return View("/Views/Settings/WhiteList/Systems.cshtml", coreDB.WhitePtrIPs.AsNoTracking().AsEnumerable().Reverse());
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
