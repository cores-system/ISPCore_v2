using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases;
using ISPCore.Models.RequestsFilter.Domains;
using ISPCore.Engine.core.Cache.CheckLink;
using ISPCore.Engine.Databases;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ISPCore.Engine.Base;
using ISPCore.Models.Response;

namespace ISPCore.Controllers
{
    public class RequestsFilterToDomainAntiBotController : ControllerToDB
    {
        [HttpGet]
        public IActionResult Index(int Id, bool ajax)
        {
            if (Id != 0 && FindAndInclude(coreDB.RequestsFilter_Domains.AsNoTracking(), Id) is Domain model)
            {
                ViewData["Id"] = Id;
                ViewData["ajax"] = ajax;
                return View("~/Views/RequestsFilter/Domain/AntiBot.cshtml", model);
            }
            
            return Redirect($"/requests-filter/domain/base{(ajax ? "?ajax=true" : "")}");
        }


        [HttpPost]
        public JsonResult Save(Domain domain)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            // Поиск домена
            var FindDomain = FindAndInclude(coreDB.RequestsFilter_Domains, domain.Id);
            if (FindDomain == null)
                return Json(new Text("Домен не найден"));

            // Обновляем настройки журнала
            CommonModels.Update(FindDomain.AntiBot, domain.AntiBot);

            // Сохраняем базу
            coreDB.SaveChanges();

            // Удаляем кеш для домена
            ISPCache.RemoveDomain(domain.Id);

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
            return db.Where(i => i.Id == Id).Include(a => a.AntiBot).FirstOrDefault();
        }
    }
}
