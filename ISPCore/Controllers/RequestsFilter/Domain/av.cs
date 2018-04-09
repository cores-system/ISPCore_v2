using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases;
using ISPCore.Models.RequestsFilter.Domains;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ISPCore.Engine.Databases;
using ISPCore.Engine.Base;
using ISPCore.Models.Response;

namespace ISPCore.Controllers
{
    public class RequestsFilterToDomainAvController : ControllerToDB
    {
        [HttpGet]
        public IActionResult Index(int Id, bool ajax = false)
        {
            if (Id != 0 && FindAndInclude(coreDB.RequestsFilter_Domains.AsNoTracking(), Id) is Domain model)
            {
                ViewData["Id"] = Id;
                ViewData["ajax"] = ajax;
                return View("~/Views/RequestsFilter/Domain/av.cshtml", model);
            }

            return Redirect($"/requests-filter/domain/base{(ajax ? "?ajax=true" : "")}");
        }


        [HttpPost]
        public JsonResult Save(Domain domain, AntiVirus av)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            if (string.IsNullOrWhiteSpace(av.path))
                return Json(new Text("Укажите каталог для сканирования"));

            // Поиск шаблона
            var FindDomain = FindAndInclude(coreDB.RequestsFilter_Domains, domain.Id);
            if (FindDomain == null)
                return Json(new Text("Домен не найден"));

            // Обновляем настройки антивируса
            CommonModels.Update(FindDomain.av, av);

            // Сохраняем базу
            coreDB.SaveChanges();

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
            return db.Where(i => i.Id == Id).Include(i => i.av).FirstOrDefault();
        }
    }
}
