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

        public JsonResult Telegram(TelegramBot tlg)
        {
            CommonModels.Update(tlg, jsonDB.ServiceBot.Telegram, HttpContext, updateType: UpdateType.skip);
            return new SettingsToServiceController().Save(tlg, jsonDB.ServiceBot.Email, jsonDB.ServiceBot.SMS, IsAPI: true);
        }


        public JsonResult Email(EmailBot email)
        {
            CommonModels.Update(email, jsonDB.ServiceBot.Email, HttpContext, updateType: UpdateType.skip);
            return new SettingsToServiceController().Save(jsonDB.ServiceBot.Telegram, email, jsonDB.ServiceBot.SMS, IsAPI: true);
        }


        public JsonResult SMS(SmsBot sms)
        {
            CommonModels.Update(sms, jsonDB.ServiceBot.SMS, HttpContext, updateType: UpdateType.skip);
            return new SettingsToServiceController().Save(jsonDB.ServiceBot.Telegram, jsonDB.ServiceBot.Email, sms, IsAPI: true);
        }
    }
}
