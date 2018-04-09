using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases;
using ISPCore.Engine.Common.Views;
using ISPCore.Models.RequestsFilter.Domains;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ISPCore.Controllers
{
    public class RequestsFilterToDomainsController : ControllerToDB
    {
        [HttpGet]
        public IActionResult Index(bool ajax, int page = 1, string search = null)
        {
            var domains = coreDB.RequestsFilter_Domains.AsNoTracking().Include(t => t.Templates).Where(i => search == null || i.host.Contains(search));
            var navPage = new NavPage<Domain>(domains, HttpContext, 12, page);
            return View("~/Views/RequestsFilter/Domains.cshtml", navPage, ajax);
        }
    }
}
