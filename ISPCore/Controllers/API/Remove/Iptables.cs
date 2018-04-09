using System;
using Microsoft.AspNetCore.Mvc;

namespace ISPCore.Controllers
{
    public class ApiRemoveIptables : Controller
    {
        public JsonResult BlockedsIP(int Id) => new SecurityToIPtablesController().Remove(Id);
    }
}
