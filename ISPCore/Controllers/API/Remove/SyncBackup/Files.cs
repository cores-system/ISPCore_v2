using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases;
using ISPCore.Models.Response;
using Trigger = ISPCore.Models.Triggers.Events.SyncBackup.Files;

namespace ISPCore.Controllers
{
    public class ApiRemoveBackupFiles : ControllerToDB
    {
        public JsonResult Task(int Id)
        {
            return new SyncBackupFilesToTask().Remove(Id);
        }


        public JsonResult Ignore(int Id)
        {
            // Удаляем даннные
            var res = coreDB.SyncBackup_Task_IgnoreFileOrFolders.RemoveAttach(coreDB, Id);

            // 
            Trigger.OnChange((Id, 0));

            // Успех
            return Json(new TrueOrFalse(res));
        }
    }
}
