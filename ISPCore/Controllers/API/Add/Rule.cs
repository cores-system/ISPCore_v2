using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ISPCore.Models.Databases;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ISPCore.Engine.core.Cache.CheckLink;
using ISPCore.Models.Response;

namespace ISPCore.Controllers
{
    public class ApiAddRule : ControllerToDB
    {
        public JsonResult RuleDomain(int Id, IDictionary<string, Models.RequestsFilter.Domains.Rules.Rule> rules, 
                                             IDictionary<string, Models.RequestsFilter.Domains.Rules.RuleOverride> RuleOverrides, 
                                             IDictionary<string, Models.RequestsFilter.Domains.Rules.RuleReplace> RuleReplaces,
                                             IDictionary<string, Models.RequestsFilter.Domains.Rules.RuleArg> RuleArgs)
        {
            if (0 >= Id)
                return Json(new Text("Укажите Id домена"));

            return new RequestsFilterToDomainRulesController().Save(new Models.RequestsFilter.Domains.Domain() { Id = Id }, rules, RuleReplaces, RuleOverrides, RuleArgs, IsAPI: true);
        }


        public JsonResult RuleTemplate(int Id, IDictionary<string, Models.RequestsFilter.Templates.Rules.Rule> rules,
                                               IDictionary<string, Models.RequestsFilter.Templates.Rules.RuleOverride> RuleOverrides,
                                               IDictionary<string, Models.RequestsFilter.Templates.Rules.RuleReplace> RuleReplaces,
                                               IDictionary<string, Models.RequestsFilter.Templates.Rules.RuleArg> RuleArgs)
        {
            if (0 >= Id)
                return Json(new Text("Укажите Id шаблона"));

            return new RequestsFilterToTemplateController().Save(new Models.RequestsFilter.Templates.Template() { Id = Id, Name = "API" }, rules, RuleReplaces, RuleOverrides, RuleArgs, IsAPI: true);
        }
    }
}
