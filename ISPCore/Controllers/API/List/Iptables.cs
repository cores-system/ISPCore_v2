using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases;
using Microsoft.EntityFrameworkCore;

namespace ISPCore.Controllers
{
    public class ApiListIptables : ControllerToDB
    {
        public JsonResult BlockedsIP(int page = 1, int pageSize = 20, string search = null)
        {
            return Json(coreDB.BlockedsIP.AsNoTracking().AsEnumerable().Where(i => i.BlockingTime > DateTime.Now && (search == null || i.IP.Contains(search))).Reverse().Skip((page * pageSize) - pageSize).Take(pageSize));
        }
    }
}
