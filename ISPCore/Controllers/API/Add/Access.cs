using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.RequestsFilter.Access;

namespace ISPCore.Controllers
{
    public class ApiAddAccess : Controller
    {
        public JsonResult Base(string host, string IP, int AccessTimeToMinutes, AccessType accessType) {
            return new RequestsFilterToAccessController().Open(host, IP, AccessTimeToMinutes, accessType);
        }
    }
}
