using System;
using Microsoft.AspNetCore.Mvc;

namespace ISPCore.Controllers
{
    public class ApiFaq : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View("~/Views/FAQ/API.cshtml");
        }
    }
}
