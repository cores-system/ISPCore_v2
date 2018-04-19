using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ISPCore.Models.Databases;
using ISPCore.Engine.Common.Views;
using ISPCore.Models.SyncBackup.Operation;

namespace ISPCore.Controllers
{
    public class SyncBackupFilesToOperation : ControllerToDB
    {
        [HttpGet]
        public IActionResult Index(bool ajax, int page = 1, int TaskId = -1)
        {
            // Список операций
            var db = coreDB.SyncBackup_Notations.AsNoTracking().Include(n => n.More).AsEnumerable().Where(i => TaskId == -1 || i.TaskId == TaskId);

            // Список активных операций
            if (page == 1) {
                var workNotes = CoreDB.SyncBackupWorkNote.Where(i => TaskId == -1 || i.TaskId == TaskId).ToList();
                ViewBag.WorkNote = workNotes.Count == 0 ? null : workNotes;
            }

            // Выводим контент
            var navPage = new NavPage<Notation>(db, HttpContext, 15, page);
            return View("~/Views/SyncBackup/Files/Operation.cshtml", navPage, ajax);
        }
    }
}
