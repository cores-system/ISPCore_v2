using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases;
using System.Collections.Generic;
using ISPCore.Models.RequestsFilter.Domains;
using ISPCore.Engine.core.Cache.CheckLink;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ISPCore.Models.Response;
using ISPCore.Engine.Base;
using ISPCore.Models.RequestsFilter.Domains.Rules;

namespace ISPCore.Controllers
{
    public class RequestsFilterToDomainRulesController : ControllerToDB
    {
        [HttpGet]
        public IActionResult Index(int Id, bool ajax)
        {
            if (Id != 0 && FindAndInclude(coreDB.RequestsFilter_Domains.AsNoTracking(), Id) is Domain model)
            {
                ViewData["Id"] = Id;
                ViewData["ajax"] = ajax;
                return View("~/Views/RequestsFilter/Domain/Rules.cshtml", model);
            }

            return Redirect($"/requests-filter/domain/base{(ajax ? "?ajax=true" : "")}");
        }


        [HttpPost]
        public JsonResult Save(Domain domain, IDictionary<string, Rule> rules = null, IDictionary<string, RuleReplace> RuleReplaces = null, IDictionary<string, RuleOverride> RuleOverrides = null, IDictionary<string, RuleArg> RuleArgs = null, bool IsAPI = false)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            // Поиск домена
            var FindDomain = FindAndInclude(coreDB.RequestsFilter_Domains, domain.Id);
            if (FindDomain == null)
                return Json(new Text("Домен не найден"));
            
            // Записываем новые правила и перезаписываем старые
            FindDomain.Rules.UpdateOrAddRange(rules, out var NewRules);
            FindDomain.RuleReplaces.UpdateOrAddRange(RuleReplaces, out var NewRuleReplace);
            FindDomain.RuleOverrides.UpdateOrAddRange(RuleOverrides, out var NewRuleOverrides);
            FindDomain.RuleArgs.UpdateOrAddRange(RuleArgs, out var NewRuleArgs);

            // Сохраняем базу
            coreDB.SaveChanges();

            // Удаляем кеш для домена
            ISPCache.RemoveDomain(domain.Id);

            // API
            if (IsAPI)
                return Json(new UpdateToIds("accepted", domain.Id, NewRules, NewRuleOverrides, NewRuleArgs, NewRuleReplace));

            // Отдаем сообщение и Id новых правил
            return Json(new UpdateToIds("Настройки домена сохранены", 0, NewRules, NewRuleOverrides, NewRuleArgs, NewRuleReplace));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="Id"></param>
        private Domain FindAndInclude(IQueryable<Domain> db, int Id)
        {
            return db.Where(i => i.Id == Id).Include(i => i.Rules).Include(i => i.RuleReplaces).Include(i => i.RuleOverrides).Include(c => c.RuleArgs).FirstOrDefault();
        }
    }
}
