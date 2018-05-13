using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Engine.Auth;
using ISPCore.Engine.Base;
using ISPCore.Models.Response;
using ISPCore.Models.Databases;
using ISPCore.Models.Databases.json;
using Trigger = ISPCore.Models.Triggers.Events.Settings.Service;

namespace ISPCore.Controllers
{
    public class SettingsToServiceController : ControllerToDB
    {
        [HttpGet]
        public IActionResult Index(bool ajax)
        {
            ViewData["salt"] = PasswdTo.salt;
            ViewData["ajax"] = ajax;
            return View("/Views/Settings/Service.cshtml", jsonDB.ServiceBot);
        }


        [HttpPost]
        public JsonResult Save(TelegramBot tlg, EmailBot email, SmsBot sms, bool IsAPI = false)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            // Проверка Email
            if (!string.IsNullOrWhiteSpace(email.ConnectUrl) && 0 >= email.ConnectPort)
                return Json(new Text("Укажите порт почтового сервера"));

            // Меняем настройки
            jsonDB.ServiceBot.Telegram = tlg;
            jsonDB.ServiceBot.Email = email;
            jsonDB.ServiceBot.SMS = sms;
            jsonDB.Save();

            // 
            Trigger.OnChange((0, 0));

            // Ответ
            if (IsAPI)
                return Json(new TrueOrFalse(true));

            // Успех
            return Json(new Text("Настройки успешно сохранены"));
        }
    }
}
