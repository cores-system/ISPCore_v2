using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ISPCore.Models.Databases;
using ISPCore.Engine.Common.Views;
using ISPCore.Models.RequestsFilter.Templates;

namespace ISPCore.Controllers
{
    public class RequestsFilterToTemplatesController : ControllerToDB
    {
        [HttpGet]
        public IActionResult Index(bool ajax, int page = 1)
        {
            var navPage = new NavPage<Template>(coreDB.RequestsFilter_Templates.AsNoTracking(), HttpContext, 12, page);
            return View("~/Views/RequestsFilter/Templates.cshtml", navPage, ajax);
        }
    }
}
