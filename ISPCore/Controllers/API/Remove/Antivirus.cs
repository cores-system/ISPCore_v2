using System;
using Microsoft.AspNetCore.Mvc;

namespace ISPCore.Controllers
{
    public class ApiRemoveAntivirus : Controller
    {
        public JsonResult Base(string FileName) => new SecurityToAntiVirusController().Remove(FileName);
    }
}
