using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases;
using ISPCore.Engine.core.Cache.CheckLink;
using ISPCore.Models.Response;
using Trigger = ISPCore.Models.Triggers.Events.RequestsFilter.Domain;

namespace ISPCore.Controllers
{
    public class ApiRemoveDomain : ControllerToDB
    {
        public JsonResult Base(int Id) => new RequestsFilterToDomainBaseController().Remove(Id);
        public JsonResult Alias(int DomainId, int Id) => new RequestsFilterToCommonController().RemoveToAlias(DomainId, Id);

        #region Template
        public JsonResult Template(int DomainId, int Id)
        {
            // Удаляем данные
            if (coreDB.RequestsFilter_Domain_TemplatesId.RemoveAttach(coreDB, Id))
            {
                // Удаляем кеш
                ISPCache.RemoveDomain(DomainId);

                // 
                Trigger.OnChange((DomainId, "Base"));

                // Отдаем результат
                return Json(new TrueOrFalse(true));
            }

            return Json(new TrueOrFalse(false));
        }
        #endregion

        #region Ignore
        public JsonResult Ignore(int DomainId, int Id)
        {
            // Удаляем данные
            if (coreDB.RequestsFilter_Domain_IgnoreToLogs.RemoveAttach(coreDB, Id))
            {
                // Удаляем кеш
                ISPCache.RemoveDomain(DomainId);

                // 
                Trigger.OnChange((DomainId, "LogSettings"));

                // Отдаем результат
                return Json(new TrueOrFalse(true));
            }

            return Json(new TrueOrFalse(false));
        }
        #endregion
    }
}
