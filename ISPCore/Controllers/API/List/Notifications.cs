using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases;
using Microsoft.EntityFrameworkCore;

namespace ISPCore.Controllers
{
    public class ApiListNotifications : ControllerToDB
    {
        public JsonResult Jurnal(int page = 1, int pageSize = 20) => Json(coreDB.Notations.AsNoTracking().Include(n => n.More).AsEnumerable().Reverse().Skip((page * pageSize) - pageSize).Take(pageSize));
    }
}
