using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.RequestsFilter.Access;

namespace ISPCore.Controllers
{
    public class ApiRemoveAccess : Controller
    {
        public JsonResult Base(string IP, string host, AccessType accessType) => new RequestsFilterToAccessController().Remove(IP, host, accessType);
    }
}
