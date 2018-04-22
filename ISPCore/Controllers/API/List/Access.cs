using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Engine.RequestsFilter.Access;

namespace ISPCore.Controllers
{
    public class ApiListAccess : Controller
    {
        public JsonResult Get(int page = 1, int pageSize = 20, string search = null)
        {
            return Json(AccessIP.List().Where(i => search == null || i.IP.Contains(search) || i.host.Contains(search)).Skip((page * pageSize) - pageSize).Take(pageSize));
        }
    }
}
