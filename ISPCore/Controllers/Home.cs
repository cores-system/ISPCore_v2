using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases;
using Microsoft.EntityFrameworkCore;
using ISPCore.Engine.Common.Views;
using ISPCore.Models.Home;

namespace ISPCore.Controllers
{
    public class HomeController : ControllerToDB
    {
        public IActionResult Index(bool ajax, int page = 1)
        {
            var navPage = new NavPage<Jurnal>(coreDB.Home_Jurnals.AsNoTracking(), HttpContext, 10, page);
            return View(navPage, ajax);
        }
    }
}
