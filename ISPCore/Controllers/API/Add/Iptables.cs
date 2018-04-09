using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.RequestsFilter.Domains;

namespace ISPCore.Controllers
{
    public class ApiAddIptables : Controller
    {
        public JsonResult Base(string value, string Description, int BlockingTimeDay, TypeBlockIP typeBlockIP)
        {
            return new SecurityToIPtablesController().Add(value, Description, BlockingTimeDay, IsAPI: true, typeBlockIP: typeBlockIP);
        }
    }
}
