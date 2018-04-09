using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Security;
using ISPCore.Engine;
using ISPCore.Models.Databases.json;
using ISPCore.Engine.Databases;
using ISPCore.Models.Databases.Enums;

namespace ISPCore.Controllers
{
    public class ApiCommonAV : Controller
    {
        JsonDB jsonDB;
        public ApiCommonAV()
        {
            jsonDB = Service.Get<JsonDB>();
        }

        public JsonResult Start(AntiVirus av)
        {
            CommonModels.Update(av, jsonDB.AntiVirus, HttpContext, updateType: UpdateType.skip);
            return new SecurityToAntiVirusController().Start(av);
        }


        public JsonResult Stop() => new SecurityToAntiVirusController().Stop();
    }
}
