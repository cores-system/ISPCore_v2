using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Engine;
using ISPCore.Models.Databases.json;

namespace ISPCore.Controllers
{
    public class ApiListJsonDB : Controller
    {
        public JsonResult Get() => Json(Service.Get<JsonDB>());
    }
}
