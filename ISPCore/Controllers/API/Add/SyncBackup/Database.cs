using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases;
using ISPCore.Models.SyncBackup.Database;

namespace ISPCore.Controllers
{
    public class ApiAddBackupDatabase : ControllerToDB
    {
        public JsonResult Task(Task tk, DumpConf conf, MySQL mysql)
        {
            return new SyncBackupDatabaseToTask().Save(tk, conf, mysql);
        }
    }
}
