using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases.json;
using ISPCore.Engine;
using ISPCore.Engine.Databases;
using ISPCore.Models.Security;
using ISPCore.Models.Databases.Enums;

namespace ISPCore.Controllers
{
    public class ApiEditSettingsToBase : Controller
    {
        JsonDB jsonDB;
        public ApiEditSettingsToBase()
        {
            jsonDB = Service.Get<JsonDB>();
        }


        public JsonResult API(API api)
        {
            CommonModels.Update(api, jsonDB.API, HttpContext, updateType: UpdateType.skip);
            return new SettingsToBaseController().Save(jsonDB.Base, api, jsonDB.Security, jsonDB.BruteForceConf, null, IsAPI: true);
        }
        

        public JsonResult Base(Base bs)
        {
            CommonModels.Update(bs, jsonDB.Base, HttpContext, updateType: UpdateType.skip);
            return new SettingsToBaseController().Save(bs, jsonDB.API, jsonDB.Security, jsonDB.BruteForceConf, null, IsAPI: true);
        }


        public JsonResult Security(Security sc)
        {
            CommonModels.Update(sc, jsonDB.Security, HttpContext, updateType: UpdateType.skip);
            return new SettingsToBaseController().Save(jsonDB.Base, jsonDB.API, sc, jsonDB.BruteForceConf, null, IsAPI: true);
        }

        public JsonResult BruteForce(BruteForceConf conf)
        {
            CommonModels.Update(conf, jsonDB.BruteForceConf, HttpContext, updateType: UpdateType.skip);
            return new SettingsToBaseController().Save(jsonDB.Base, jsonDB.API, jsonDB.Security, conf, null, IsAPI: true);
        }

        public JsonResult Passwd(string PasswdRoot, string Passwd2FA, string salt) => new SettingsToBaseController().Save(jsonDB.Base, jsonDB.API, jsonDB.Security, jsonDB.BruteForceConf, PasswdRoot, Passwd2FA, salt, IsAPI: true);


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
