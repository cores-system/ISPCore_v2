using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases;

namespace ISPCore.Controllers
{
    public class ApiRemoveBackupDatabase : ControllerToDB
    {
        public JsonResult Task(int Id)
        {
            return new SyncBackupDatabaseToTask().Remove(Id);
        }
    }
}
