using System;
using System.IO;
using System.Linq;
using System.Text;
using ISPCore.Engine.Base;
using ISPCore.Engine.Common.Views;
using ISPCore.Engine.Triggers;
using ISPCore.Models.Databases;
using ISPCore.Models.RequestsFilter.Templates;
using ISPCore.Models.Response;
using ISPCore.Models.Triggers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ISPCore.Controllers
{
    public class ToolsToTriggerNode : ControllerToDB
    {
        public IActionResult Index(bool ajax, int Id = 0)
        {
            ViewData["Id"] = Id;
            ViewData["ajax"] = ajax;
            return View("~/Views/Tools/Trigger/Node.cshtml", RegisteredTriggers.List().Where(i => i.Id == Id).FirstOrDefault());
        }
    }
}
