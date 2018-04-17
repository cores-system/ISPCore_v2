using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ISPCore.Models.Databases;
using ISPCore.Engine.Common.Views;

namespace ISPCore.Controllers
{
    public class SyncBackupFilesToTasks : ControllerToDB
    {
        [HttpGet]
        public IActionResult Index(bool ajax, int page = 1)
        {
            var navPage = new NavPage<Models.SyncBackup.Tasks.Task>(coreDB.SyncBackup_Tasks.AsNoTracking(), HttpContext, 12, page);
            return View("~/Views/SyncBackup/Files/Tasks.cshtml", navPage, ajax);
        }
    }
}
