using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases;
using ISPCore.Models.RequestsFilter.Domains;
using ISPCore.Engine.core.Cache.CheckLink;
using ISPCore.Engine.Databases;
using System.Collections.Generic;
using ISPCore.Models.RequestsFilter.Domains.Log;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ISPCore.Engine.Base;
using ISPCore.Models.Response;
using Trigger = ISPCore.Models.Triggers.Events.RequestsFilter.Domain;

namespace ISPCore.Controllers
{
    public class RequestsFilterToDomainLogSettingsController : ControllerToDB
    {
        [HttpGet]
        public IActionResult Index(int Id, bool ajax)
        {
            if (Id != 0 && FindAndInclude(coreDB.RequestsFilter_Domains.AsNoTracking(), Id) is Domain model)
            {
                ViewData["Id"] = Id;
                ViewData["ajax"] = ajax;
                return View("~/Views/RequestsFilter/Domain/LogSettings.cshtml", model);
            }
            
            return Redirect($"/requests-filter/domain/base{(ajax ? "?ajax=true" : "")}");
        }


        [HttpPost]
        public JsonResult Save(Domain domain, IDictionary<string, IgnoreToLog> IgnoreToLogs = null)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            // Поиск шаблона
            var FindDomain = coreDB.RequestsFilter_Domains.Where(i => i.Id == domain.Id).Include(c => c.confToLog).FirstOrDefault();
            if (FindDomain == null)
                return Json(new Text("Домен не найден"));

            // Обновляем настройки журнала
            CommonModels.Update(FindDomain.confToLog, domain.confToLog);

            // Удаляем запись с игнорированием логов
            coreDB.RequestsFilter_Domain_IgnoreToLogs.RemoveAll(i => i.DomainId == domain.Id);

            // Создаем данные для игнорирования логов
            coreDB.RequestsFilter_Domain_IgnoreToLogs.AddRange(domain.Id, IgnoreToLogs, out _);

            // Сохраняем базу
            coreDB.SaveChanges();

            // Удаляем кеш для домена
            ISPCache.RemoveDomain(domain.Id);

            // 
            Trigger.OnChange((domain.Id, "LogSettings"));

            // Отдаем сообщение
            return Json(new Text("Настройки домена сохранены"));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="Id"></param>
        private Domain FindAndInclude(IQueryable<Domain> db, int Id)
        {
            return db.Where(i => i.Id == Id).Include(i => i.IgnoreToLogs).Include(c => c.confToLog).FirstOrDefault();
        }
    }
}
