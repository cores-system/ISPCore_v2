using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases;
using ISPCore.Engine.Common.Views;

namespace ISPCore.Controllers
{
    public class SyncBackupDatabaseToOperation : ControllerToDB
    {
        [HttpGet]
        public IActionResult Index(bool ajax, int page = 1, int TaskId = -1)
        {
            // Список операций
            var db = coreDB.SyncBackup_db_Reports.FindAll(i => TaskId == -1 || i.TaskId == TaskId);

            // Выводим контент
            var navPage = new NavPage<Models.SyncBackup.Database.Report>(db, HttpContext, 15, page);
            return View("~/Views/SyncBackup/Database/Operation.cshtml", navPage, ajax);
        }
    }
}
