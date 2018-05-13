using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases.json;
using ISPCore.Engine;
using ISPCore.Engine.Databases;
using ISPCore.Models.Databases.Enums;
using ISPCore.Models.core.Cache.CheckLink;

namespace ISPCore.Controllers
{
    public class ApiEditAntiBot : Controller
    {
        JsonDB jsonDB;
        public ApiEditAntiBot()
        {
            jsonDB = Service.Get<JsonDB>();
        }

        public JsonResult Base(AntiBot antiBot)
        {
            CommonModels.Update(antiBot, jsonDB.AntiBot, HttpContext, updateType: UpdateType.skip);
            return new SecurityToAntiBotController().Save(antiBot, jsonDB.AntiBot.limitRequest, IsAPI: true);
        }

        public JsonResult Limit(LimitRequest limit)
        {
            CommonModels.Update(limit, jsonDB.AntiBot.limitRequest, HttpContext, updateType: UpdateType.skip);
            return new SecurityToAntiBotController().Save(jsonDB.AntiBot, limit, IsAPI: true);
        }
    }
}
