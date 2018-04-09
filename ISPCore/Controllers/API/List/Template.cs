using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases;
using Microsoft.EntityFrameworkCore;

namespace ISPCore.Controllers
{
    public class ApiListTemplate : ControllerToDB
    {
        public JsonResult Template(int Id) => Json(coreDB.RequestsFilter_Templates.FindAndInclude(Id, AsNoTracking: true));
        public JsonResult Templates(int page = 1, int pageSize = 20) => Json(coreDB.RequestsFilter_Templates.AsNoTracking().AsEnumerable().Skip((page * pageSize) - pageSize).Take(pageSize));
    }
}
