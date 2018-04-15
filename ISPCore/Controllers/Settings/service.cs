using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases.json;
using ISPCore.Engine.Auth;
using ISPCore.Engine.Base;
using ISPCore.Models.Response;
using ISPCore.Models.Databases;

namespace ISPCore.Controllers
{
    public class SettingsToServiceController : ControllerToDB
    {
        [HttpGet]
        public IActionResult Index(bool ajax)
        {
            ViewData["salt"] = PasswdTo.salt;
            ViewData["ajax"] = ajax;
            return View("/Views/Settings/Service.cshtml", jsonDB);
        }


        [HttpPost]
        public JsonResult Save(Telega tlg, bool IsAPI = false)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            // Меняем настройки
            jsonDB.TelegramBot = tlg;
            jsonDB.Save();
            
            // Ответ
            if (IsAPI)
                return Json(new TrueOrFalse(true));

            // Успех
            return Json(new Text("Настройки успешно сохранены"));
        }
    }
}
