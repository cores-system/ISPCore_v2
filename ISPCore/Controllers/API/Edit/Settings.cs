using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases.json;
using ISPCore.Engine;
using ISPCore.Engine.Databases;
using ISPCore.Models.Security;
using ISPCore.Models.Databases.Enums;

namespace ISPCore.Controllers
{
    public class ApiEditSettings : Controller
    {
        JsonDB jsonDB;
        public ApiEditSettings()
        {
            jsonDB = Service.Get<JsonDB>();
        }


        public JsonResult API(API api)
        {
            CommonModels.Update(api, jsonDB.API, HttpContext, updateType: UpdateType.skip);
            return new SettingsController().Save(jsonDB.Base, api, jsonDB.Security, jsonDB.TelegramBot, jsonDB.BruteForceConf, null, IsAPI: true);
        }
        

        public JsonResult Base(Base bs)
        {
            CommonModels.Update(bs, jsonDB.Base, HttpContext, updateType: UpdateType.skip);
            return new SettingsController().Save(bs, jsonDB.API, jsonDB.Security, jsonDB.TelegramBot, jsonDB.BruteForceConf, null, IsAPI: true);
        }


        public JsonResult Security(Security sc)
        {
            CommonModels.Update(sc, jsonDB.Security, HttpContext, updateType: UpdateType.skip);
            return new SettingsController().Save(jsonDB.Base, jsonDB.API, sc, jsonDB.TelegramBot, jsonDB.BruteForceConf, null, IsAPI: true);
        }

        public JsonResult TelegramBot(Telega tlg)
        {
            CommonModels.Update(tlg, jsonDB.TelegramBot, HttpContext, updateType: UpdateType.skip);
            return new SettingsController().Save(jsonDB.Base, jsonDB.API, jsonDB.Security, tlg, jsonDB.BruteForceConf, null, IsAPI: true);
        }

        public JsonResult BruteForce(BruteForceConf conf)
        {
            CommonModels.Update(conf, jsonDB.BruteForceConf, HttpContext, updateType: UpdateType.skip);
            return new SettingsController().Save(jsonDB.Base, jsonDB.API, jsonDB.Security, jsonDB.TelegramBot, conf, null, IsAPI: true);
        }

        public JsonResult Passwd(string PasswdRoot, string Passwd2FA, string salt) => new SettingsController().Save(jsonDB.Base, jsonDB.API, jsonDB.Security, jsonDB.TelegramBot, jsonDB.BruteForceConf, null, PasswdRoot, Passwd2FA, salt, IsAPI: true);


        public JsonResult AntiDdos(AntiDdos antiDdos)
        {
            CommonModels.Update(antiDdos, jsonDB.AntiDdos, HttpContext, updateType: UpdateType.skip);
            return new SecurityToAntiDdosController().Save(antiDdos, null, UpdateIgnoreToIP: false, IsAPI: true);
        }


        public JsonResult AntiVirus(AntiVirus av)
        {
            CommonModels.Update(av, jsonDB.AntiVirus, HttpContext, updateType: UpdateType.skip);
            return new SecurityToAntiVirusController().Save(av, IsAPI: true);
        }
    }
}
