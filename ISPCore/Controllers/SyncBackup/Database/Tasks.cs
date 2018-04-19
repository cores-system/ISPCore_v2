using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ISPCore.Models.Databases;
using ISPCore.Engine.Common.Views;

namespace ISPCore.Controllers
{
    public class SyncBackupDatabaseToTasks : ControllerToDB
    {
        [HttpGet]
        public IActionResult Index(bool ajax, int page = 1)
        {
            var navPage = new NavPage<Models.SyncBackup.Database.Task>(coreDB.SyncBackup_db_Tasks.AsNoTracking(), HttpContext, 12, page);
            return View("~/Views/SyncBackup/Database/Tasks.cshtml", navPage, ajax);
        }
    }
}
