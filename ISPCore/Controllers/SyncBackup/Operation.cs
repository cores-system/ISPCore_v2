using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ISPCore.Models.Databases;
using ISPCore.Engine.Common.Views;
using ISPCore.Models.SyncBackup.Operation;

namespace ISPCore.Controllers
{
    public class SyncBackupToOperationController : ControllerToDB
    {
        [HttpGet]
        public IActionResult Index(bool ajax, int page = 1, int TaskId = -1)
        {
            // Список операций
            var db = coreDB.SyncBackup_Notations.AsNoTracking().Include(n => n.More).AsEnumerable().Where(i => TaskId == -1 || i.TaskId == TaskId);

            // Список активных операций
            if (page == 1) {
                ViewBag.WorkNote = CoreDB.SyncBackupWorkNote.Where(i => TaskId == -1 || i.TaskId == TaskId);
            }

            // Выводим контент
            var navPage = new NavPage<Notation>(db, HttpContext, 15, page);
            return View("~/Views/SyncBackup/Operation.cshtml", navPage, ajax);
        }
    }
}
