using System;
using System.Linq;
using ISPCore.Models.Databases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ISPCore.Controllers
{
    public class ApiListWhitelistTo : ControllerToDB
    {
        public JsonResult Systems(int page = 1, int pageSize = 20)
        {
            return Json(coreDB.WhitePtrIPs.AsNoTracking().AsEnumerable().Skip((page * pageSize) - pageSize).Take(pageSize));
        }
    }
}
