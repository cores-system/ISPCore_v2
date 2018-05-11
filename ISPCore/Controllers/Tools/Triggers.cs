using System;
using ISPCore.Engine.Common.Views;
using ISPCore.Engine.Triggers;
using ISPCore.Models.Databases;
using ISPCore.Models.Triggers;
using Microsoft.AspNetCore.Mvc;

namespace ISPCore.Controllers
{
    public class ToolsToTriggers : ControllerToDB
    {
        public IActionResult Index(bool ajax, int page = 1)
        {
            var navPage = new NavPage<TriggerConf>(RegisteredTriggers.List(), HttpContext, 12, page);
            return View("~/Views/Tools/Triggers.cshtml", navPage, ajax);
        }
    }
}
