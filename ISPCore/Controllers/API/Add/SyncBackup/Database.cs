using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases;
using ISPCore.Models.SyncBackup.Database;

namespace ISPCore.Controllers
{
    public class ApiAddBackupDatabase : ControllerToDB
    {
        public JsonResult Task(Task tk, DumpConf dumpConf, ConnectionConf connectionConf)
        {
            return new SyncBackupDatabaseToTask().Save(tk, dumpConf, connectionConf);
        }
    }
}
