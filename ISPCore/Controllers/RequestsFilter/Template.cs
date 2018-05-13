using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases;
using System.Collections.Generic;
using ISPCore.Engine.Databases;
using ISPCore.Engine.core.Cache.CheckLink;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using ISPCore.Models.Response;
using ISPCore.Models.RequestsFilter.Templates;
using ISPCore.Engine.Base;
using ISPCore.Models.RequestsFilter.Templates.Rules;
using Trigger = ISPCore.Models.Triggers.Events.RequestsFilter.Template;

namespace ISPCore.Controllers
{
    public class RequestsFilterToTemplateController : ControllerToDB
    {
        [HttpGet]
        public IActionResult Index(int Id, bool ajax)
        {
            ViewData["Id"] = Id;
            ViewData["ajax"] = ajax;
            return View("~/Views/RequestsFilter/Template.cshtml", coreDB.RequestsFilter_Templates.FindAndInclude(Id, AsNoTracking: true));
        }

        #region Save
        [HttpPost]
        public JsonResult Save(Template tpl, IDictionary<string, Rule> rules = null, IDictionary<string, RuleReplace> RuleReplaces = null, IDictionary<string, RuleOverride> RuleOverrides = null, IDictionary<string, RuleArg> RuleArgs = null, bool IsAPI = false)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            if (string.IsNullOrWhiteSpace(tpl.Name))
                return Json(new Text("Имя шаблона не может быть пустым"));

            // Новый шаблон 
            if (tpl.Id == 0)
            {
                // Создаем правила
                tpl.Rules.UpdateOrAddRange(rules, out var NewRules);
                tpl.RuleReplaces.UpdateOrAddRange(RuleReplaces, out var NewRuleReplace);
                tpl.RuleOverrides.UpdateOrAddRange(RuleOverrides, out var NewRuleOverrides);
                tpl.RuleArgs.UpdateOrAddRange(RuleArgs, out var NewRuleArgs);

                // Добовляем в базу
                coreDB.RequestsFilter_Templates.Add(tpl);

                // Сохраняем базу
                coreDB.SaveChanges();

                // 
                Trigger.OnCreate((tpl.Id, 0));

                // Отдаем новый Id шаблона и Id новых правил
                return Json(new UpdateToIds(IsAPI ? "accepted" : null, tpl.Id, NewRules, NewRuleReplace, NewRuleOverrides, NewRuleArgs));
            }

            // Старый шаблон
            else
            {
                // Поиск шаблона
                if (coreDB.RequestsFilter_Templates.FindAndInclude(tpl.Id) is var FindTPL && FindTPL == null)
                    return Json(new Text("Шаблон не найден"));

                // Обновляем параметры шаблона
                if (!IsAPI)
                    CommonModels.Update(FindTPL, tpl);

                // Записываем новые правила и перезаписываем старые
                FindTPL.Rules.UpdateOrAddRange(rules, out var NewRules);
                FindTPL.RuleReplaces.UpdateOrAddRange(RuleReplaces, out var NewRuleReplace);
                FindTPL.RuleOverrides.UpdateOrAddRange(RuleOverrides, out var NewRuleOverrides);
                FindTPL.RuleArgs.UpdateOrAddRange(RuleArgs, out var NewRuleArgs);

                // Сохраняем базу
                coreDB.SaveChanges();

                // Удаляем кеш для шаблона
                ISPCache.RemoveTemplate(tpl.Id);

                // 
                Trigger.OnChange((tpl.Id, 0));

                // API
                if (IsAPI)
                    return Json(new UpdateToIds("accepted", tpl.Id, NewRules, NewRuleReplace, NewRuleOverrides, NewRuleArgs));

                // Отдаем сообщение и Id новых правил
                return Json(new UpdateToIds("Настройки шаблона сохранены", 0, NewRules, NewRuleReplace, NewRuleOverrides, NewRuleArgs));
            }
        }
        #endregion

        #region Remove
        [HttpPost]
        public JsonResult Remove(int Id)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            // Удаляем шаблон
            if (coreDB.RequestsFilter_Templates.RemoveAttach(coreDB, Id))
            {
                // Удаляем кеш для шаблона
                ISPCache.RemoveTemplate(Id);

                // 
                Trigger.OnRemove((Id, 0));

                // Отдаем результат
                return Json(new TrueOrFalse(true));
            }

            return Json(new Text("Ошибка ;("));
        }
        #endregion

        #region Export
        [HttpGet]
        public string Export(int Id)
        {
            // Поиск шаблона
            if (coreDB.RequestsFilter_Templates.FindAndInclude(Id) is var tpl && tpl == null)
                return JsonConvert.SerializeObject(new Text("Шаблон не найден"), Formatting.Indented);

            // Отдаем результат
            return Regex.Replace(JsonConvert.SerializeObject(tpl, Formatting.Indented), "\"Id\": +[0-9]+,", "\"Id\": 0,", RegexOptions.IgnoreCase);
        }
        #endregion

        #region Import
        [HttpPost]
        public JsonResult Import() => Json(Import(HttpContext));

        public TrueOrFalse Import(HttpContext context)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return new TrueOrFalse(false);
            #endregion

            bool res = false;
            if (context.Request.Form.Files.Count == 1)
            {
                using (MemoryStream mem = new MemoryStream())
                {
                    // Получаем файл
                    context.Request.Form.Files[0].CopyTo(mem);
                    var tpl = JsonConvert.DeserializeObject<Template>(Encoding.UTF8.GetString(mem.ToArray()));

                    // Добовляем в базу
                    coreDB.RequestsFilter_Templates.Add(tpl);

                    // Сохраняем базу
                    coreDB.SaveChanges();
                    res = true;

                    // 
                    Trigger.OnChange((tpl.Id, 0));
                }
            }

            // Отдаем результат
            return new TrueOrFalse(res);
        }
        #endregion
    }
}
