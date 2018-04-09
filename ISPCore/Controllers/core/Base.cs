using System;
using Microsoft.AspNetCore.Mvc;

namespace ISPCore.Controllers.core
{
    public class CoreBaseController : Controller
    {
        public string MyIP()
        {
            return HttpContext.Connection.RemoteIpAddress.ToString();
        }
    }
}
