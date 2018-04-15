using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases.json;
using ISPCore.Engine;
using ISPCore.Engine.Databases;
using ISPCore.Models.Databases.Enums;

namespace ISPCore.Controllers
{
    public class ApiEditSettingsToService : Controller
    {
        JsonDB jsonDB;
        public ApiEditSettingsToService()
        {
            jsonDB = Service.Get<JsonDB>();
        }

        public JsonResult Telegram(Telega tlg)
        {
            CommonModels.Update(tlg, jsonDB.TelegramBot, HttpContext, updateType: UpdateType.skip);
            return new SettingsToServiceController().Save(tlg, IsAPI: true);
        }
    }
}
