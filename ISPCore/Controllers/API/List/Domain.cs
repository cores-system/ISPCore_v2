using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ISPCore.Controllers 
{
    public class ApiListDomain : ControllerToDB
    {
        public JsonResult Domains(int page = 1, int pageSize = 20)
        {
            return Json(coreDB.RequestsFilter_Domains.AsNoTracking().AsEnumerable().Skip((page * pageSize) - pageSize).Take(pageSize).Select(i => new { @id = i.Id, @host = i.host, Protect = i.Protect, typeBlockIP = i.typeBlockIP }));
        }

        public JsonResult Domain(int Id) => Json(coreDB.RequestsFilter_Domains.FindAndInclude(Id, AsNoTracking: true));
    }
}
