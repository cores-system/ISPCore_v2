using System;
using Microsoft.AspNetCore.Mvc;

namespace ISPCore.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult _404(bool ajax)
        {
            ViewData["ajax"] = ajax;
            return View();
        }
    }
}
