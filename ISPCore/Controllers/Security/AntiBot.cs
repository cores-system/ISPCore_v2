using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases.json;
using ISPCore.Models.Databases;
using ISPCore.Engine.Databases;
using ISPCore.Models.core.Cache.CheckLink;
using ISPCore.Models.Response;
using ISPCore.Engine.Base;

namespace ISPCore.Controllers
{
    public class SecurityToAntiBotController : ControllerToDB
    {
        [HttpGet]
        public IActionResult Index(bool ajax)
        {
            ViewData["ajax"] = ajax;
            return View("~/Views/Security/AntiBot/Index.cshtml", jsonDB.AntiBot);
        }
        

        [HttpPost]
        public JsonResult Save(AntiBot antiBot, LimitRequest limit, bool IsAPI = false)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            // Лимит запросов
            jsonDB.AntiBot.limitRequest = limit;

            // Обновляем параметры AntiBot
            CommonModels.Update(jsonDB.AntiBot, antiBot);

            // Сохраняем базу
            jsonDB.AntiBot.LastUpdateToConf = DateTime.Now;
            jsonDB.Save();

            // Ответ
            if (IsAPI)
                return Json(new TrueOrFalse(true));
            return Json(new Text("Настройки успешно сохранены"));
        }
    }
}
