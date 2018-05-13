using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ISPCore.Models.Databases.json;
using Microsoft.EntityFrameworkCore;
using ISPCore.Models.Databases;
using ISPCore.Engine.Base;
using ISPCore.Models.Security.AntiDdos;
using ISPCore.Engine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ISPCore.Models;
using ISPCore.Models.Response;
using ISPCore.Engine.Base.SqlAndCache;
using Trigger = ISPCore.Models.Triggers.Events.Security.AntiDdos;

namespace ISPCore.Controllers
{
    public class SecurityToAntiDdosController : ControllerToDB
    {
        [HttpGet]
        public IActionResult Index(bool ajax, bool IsJurnal, int page = 1)
        {
            // Данные для вывода статистики
            var DtNumberOfRequestDay = coreDB.AntiDdos_NumberOfRequestDays.AsNoTracking().ToList();
            if (memoryCache.TryGetValue(KeyToMemoryCache.AntiDdosNumberOfRequestDay(DateTime.Now), out NumberOfRequestDay dataHour))
                DtNumberOfRequestDay.Add(dataHour);

            // База и остальные параметры
            ViewData["DtNumberOfRequestDay"] = DtNumberOfRequestDay;
            ViewData["jsonDB"] = jsonDB;
            ViewData["ajax"] = ajax;
            ViewData["page"] = page;
            ViewData["IsJurnal"] = IsJurnal;
            return View("~/Views/Security/AntiDdos.cshtml", coreDB);
        }
        

        [HttpPost]
        public JsonResult Save(AntiDdos antiDdos, IDictionary<string, NameAndValue> IgnoreToIP, bool UpdateIgnoreToIP = true, bool IsAPI = false)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            if (!Regex.IsMatch(antiDdos.CheckPorts, "^[0-9,]+$"))
                return Json(new Text("Список проверяемых портов имеет недопустимый формат"));

            // Зависимости
            if (antiDdos.IsActive)
            {
                // Проверяем iptables
                if (jsonDB.AntiDdos.BlockToIPtables && string.IsNullOrWhiteSpace(new Bash().Run("iptables -V 2>/dev/null")))
                    return Json(new Text("В системе отсутствует iptables"));

                // Проверяем tcpdump
                if (!(new Bash().Run("tcpdump --version 2>&1").Contains("OpenSSL")))
                    return Json(new Text("В системе отсутствует tcpdump"));

                // Проверяем ss
                if (string.IsNullOrWhiteSpace(new Bash().Run("ss -v 2>/dev/null")))
                    return Json(new Text("В системе отсутствует ss"));
            }

            // Убиваем tcpdump
            if (jsonDB.AntiDdos.IsActive && !antiDdos.IsActive)
                new Bash().Run("pkill tcpdump");

            //Обновляем базу
            jsonDB.AntiDdos = antiDdos;

            // Сохраняем базу
            jsonDB.Save();

            // 
            Trigger.OnChange((0, 0));

            // Ответ
            if (IsAPI)
                return Json(new TrueOrFalse(true));
            return Json(new Text("Настройки успешно сохранены"));
        }
    }
}
