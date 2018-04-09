using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases;
using Microsoft.EntityFrameworkCore;

namespace ISPCore.Controllers
{
    public class ApiListHome : ControllerToDB
    {
        public JsonResult Jurnal(int page = 1, int pageSize = 20) => Json(coreDB.Home_Jurnals.AsNoTracking().AsEnumerable().Reverse().Skip((page * pageSize) - pageSize).Take(pageSize));
    }
}
