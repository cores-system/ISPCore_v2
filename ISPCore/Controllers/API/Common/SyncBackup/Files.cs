using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.SyncBackup.Tasks;
using ISPCore.Models.SyncBackup.ToolsEngine;
using System.Collections.Generic;

namespace ISPCore.Controllers
{
    public class ApiCommonBackupFiles : Controller
    {
        public JsonResult ClearingCache(int TaskId) => new SyncBackupFilesToTask().ClearingCache(new Task() { Id = TaskId }, IsAPI: true);
        public JsonResult Recovery(Task tk, TypeRecovery type, IDictionary<string, string> value) => new SyncBackupFilesToTools().Recovery(tk, type, value);
    }
}
