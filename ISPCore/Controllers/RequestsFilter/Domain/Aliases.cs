using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases;
using System.Collections.Generic;
using ISPCore.Models.RequestsFilter.Domains;
using System.Text.RegularExpressions;
using ISPCore.Engine.core.Cache.CheckLink;
using ISPCore.Engine.Databases;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ISPCore.Models.Response;
using ISPCore.Engine.Base;

namespace ISPCore.Controllers
{
    public class RequestsFilterToDomainAliasesController : ControllerToDB
    {
        [HttpGet]
        public IActionResult Index(int Id, bool ajax)
        {
            ViewData["Id"] = Id;
            ViewData["ajax"] = ajax;
            return View("~/Views/RequestsFilter/Domain/Aliases.cshtml", FindAndInclude(coreDB.RequestsFilter_Domains.AsNoTracking(), Id));
        }


        [HttpPost]
        public JsonResult Save(Domain domain, IDictionary<string, Alias> aliases = null)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            # region Проверяем нету ли в именах алиасов лишних символов
            if (aliases != null)
            {
                foreach (var alias in aliases)
                {
                    if (string.IsNullOrWhiteSpace(alias.Value?.host))
                        continue;

                    if (!Regex.IsMatch(alias.Value.host, "^[a-z0-9-\\.]+$", RegexOptions.IgnoreCase))
                        return Json(new Text($"Алиас {alias.Value.host} не должен содержать тип протокола или url"));
                }
            }
            #endregion

            // Поиск домена
            var FindDomain = coreDB.RequestsFilter_Domains.Where(i => i.Id == domain.Id).Include(i => i.Aliases).FirstOrDefault();
            if (FindDomain == null)
                return Json(new Text("Домен не найден"));

            // Записываем новые алиасы и перезаписываем старые
            FindDomain.Aliases.UpdateOrAddRange(aliases, out var NewAliases);

            // Сохраняем базу
            coreDB.SaveChanges();

            // Удаляем кеш для домена
            ISPCache.RemoveDomain(domain.Id);

            // Отдаем сообщение и Id новых алиасов
            return Json(new UpdateToIds("Настройки домена сохранены", 0, NewAliases));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="Id"></param>
        private Domain FindAndInclude(IQueryable<Domain> db, int Id)
        {
            return db.Where(i => i.Id == Id).Include(i => i.Aliases).FirstOrDefault();
        }
    }
}
