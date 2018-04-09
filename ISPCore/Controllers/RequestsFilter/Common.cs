using ISPCore.Engine;
using ISPCore.Engine.Base;
using ISPCore.Engine.core.Cache.CheckLink;
using ISPCore.Engine.Databases;
using ISPCore.Models.Databases;
using ISPCore.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ISPCore.Controllers
{
    public class RequestsFilterToCommonController : ControllerToDB
    {
        #region RemoveToRule
        [HttpPost]
        public JsonResult RemoveToRule(int DomainId, int TemplateId, int Id)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            // Удаляем кеш для домена
            ISPCache.RemoveDomain(DomainId);
            ISPCache.RemoveTemplate(TemplateId);

            // Удаляем правило из шаблона
            if (TemplateId != 0 && coreDB.RequestsFilter_Template_Rules.RemoveAttach(coreDB, Id))
                return Json(new TrueOrFalse(true));

            // Удаляем правило из домена
            if (DomainId != 0 && coreDB.RequestsFilter_Domain_Rules.RemoveAttach(coreDB, Id))
                return Json(new TrueOrFalse(true));

            // Отдаем результат
            return Json(new TrueOrFalse(false));
        }
        #endregion

        #region RemoveToRuleReplace
        [HttpPost]
        public JsonResult RemoveToRuleReplace(int DomainId, int TemplateId, int Id)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            // Удаляем кеш для домена
            ISPCache.RemoveDomain(DomainId);
            ISPCache.RemoveTemplate(TemplateId);

            // Удаляем правило из шаблона
            if (TemplateId != 0 && coreDB.RequestsFilter_Template_RuleReplaces.RemoveAttach(coreDB, Id))
                return Json(new TrueOrFalse(true));

            // Удаляем правило из домена
            if (DomainId != 0 && coreDB.RequestsFilter_Domain_RuleReplaces.RemoveAttach(coreDB, Id))
                return Json(new TrueOrFalse(true));

            // Отдаем результат
            return Json(new TrueOrFalse(false));
        }
        #endregion

        #region RemoveToRuleOverride
        [HttpPost]
        public JsonResult RemoveToRuleOverride(int DomainId, int TemplateId, int Id)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            // Удаляем кеш для домена
            ISPCache.RemoveDomain(DomainId);
            ISPCache.RemoveTemplate(TemplateId);

            // Удаляем правило из шаблона
            if (TemplateId != 0 && coreDB.RequestsFilter_Template_RuleOverrides.RemoveAttach(coreDB, Id))
                return Json(new TrueOrFalse(true));

            // Удаляем правило из домена
            if (DomainId != 0 && coreDB.RequestsFilter_Domain_RuleOverrides.RemoveAttach(coreDB, Id))
                return Json(new TrueOrFalse(true));

            // Отдаем результат
            return Json(new TrueOrFalse(false));
        }
        #endregion

        #region RemoveToRuleArg
        [HttpPost]
        public JsonResult RemoveToRuleArg(int DomainId, int TemplateId, int Id)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            // Удаляем кеш для домена
            ISPCache.RemoveDomain(DomainId);
            ISPCache.RemoveTemplate(TemplateId);

            // Удаляем правило из шаблона
            if (TemplateId != 0 && coreDB.RequestsFilter_Template_RuleArgs.RemoveAttach(coreDB, Id))
                return Json(new TrueOrFalse(true));

            // Удаляем правило из домена
            if (DomainId != 0 && coreDB.RequestsFilter_Domain_RuleArgs.RemoveAttach(coreDB, Id))
                return Json(new TrueOrFalse(true));

            // Отдаем результат
            return Json(new TrueOrFalse(false));
        }
        #endregion

        #region RemoveToAlias
        [HttpPost]
        public JsonResult RemoveToAlias(int DomainId, int Id)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            // Удаляем правило из шаблона
            if (coreDB.RequestsFilter_Domain_Aliases.RemoveAttach(coreDB, Id))
            {
                // Удаляем кеш для домена
                ISPCache.RemoveDomain(DomainId);
                return Json(new TrueOrFalse(true));
            }

            // Отдаем результат
            return Json(new TrueOrFalse(false));
        }
        #endregion
    }
}
