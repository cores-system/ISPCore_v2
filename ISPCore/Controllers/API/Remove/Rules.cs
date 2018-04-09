using System;
using Microsoft.AspNetCore.Mvc;

namespace ISPCore.Controllers
{
    public class ApiRemoveRules : Controller
    {
        public JsonResult Rule(int DomainId, int TemplateId, int Id) => new RequestsFilterToCommonController().RemoveToRule(DomainId, TemplateId, Id);
        public JsonResult RuleReplace(int DomainId, int TemplateId, int Id) => new RequestsFilterToCommonController().RemoveToRuleReplace(DomainId, TemplateId, Id);
        public JsonResult RuleOverride(int DomainId, int TemplateId, int Id) => new RequestsFilterToCommonController().RemoveToRuleOverride(DomainId, TemplateId, Id);
        public JsonResult RuleArg(int DomainId, int TemplateId, int Id) => new RequestsFilterToCommonController().RemoveToRuleArg(DomainId, TemplateId, Id);
    }
}
