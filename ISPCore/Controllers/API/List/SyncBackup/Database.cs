using System;
using ISPCore.Models.Databases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using ISPCore.Models.SyncBackup.Operation;

namespace ISPCore.Controllers
{
    public class ApiListBackupDatabase : ControllerToDB
    {
        public JsonResult Task(int Id) => Json(coreDB.SyncBackup_db_Tasks.FindAndInclude(Id, AsNoTracking: true));
        public JsonResult Tasks(int page = 1, int pageSize = 20) => Json(coreDB.SyncBackup_db_Tasks.AsNoTracking().AsEnumerable().Skip((page * pageSize) - pageSize).Take(pageSize));


        public JsonResult Operation(int page = 1, int pageSize = 20, int TaskId = -1)
        {
            return Json(coreDB.SyncBackup_db_Reports.AsNoTracking().AsEnumerable().Where(i => TaskId == -1 || i.TaskId == TaskId).Reverse().Skip((page * pageSize) - pageSize).Take(pageSize));
        }
    }
}
