using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases.json;
using ISPCore.Engine.Hash;
using ISPCore.Engine.Auth;
using ISPCore.Engine.Base;
using ISPCore.Models.Response;
using ISPCore.Models.Databases;
using ISPCore.Models.Auth;

namespace ISPCore.Controllers
{
    public class SettingsToBaseController : ControllerToDB
    {
        [HttpGet]
        public IActionResult Index(bool ajax)
        {
            ViewData["salt"] = PasswdTo.salt;
            ViewData["ajax"] = ajax;
            return View("/Views/Settings/Base.cshtml", jsonDB);
        }


        [HttpPost]
        public JsonResult Save(Base bs, API api, Security sc, BruteForceConf BrutConf, string PasswdRoot = null, string Passwd2FA = null, string salt = null, bool IsAPI = false)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            #region Обновляем базу
            jsonDB.Base = bs;
            jsonDB.Security = sc;
            jsonDB.API = api;
            jsonDB.BruteForceConf = BrutConf;
            jsonDB.Save();
            #endregion

            // Меняем соль
            if (!string.IsNullOrWhiteSpace(salt))
            {
                if (salt.Length < 18)
                    return Json(new Text("Соль должена состоять минимум из 18 символов"));

                PasswdTo.salt = salt;
            }

            #region Меняем пароль root
            if (!string.IsNullOrWhiteSpace(PasswdRoot))
            {
                if (PasswdRoot.Length < 6)
                    return Json(new Text("Пароль 'Root' должен состоять минимум из 6 символов"));
                
                // Меняем пароль в файле
                System.IO.File.WriteAllText(Folders.Passwd + "/root", SHA256.Text(PasswdRoot));

                // Сессия
                string authSession = md5.text(DateTime.Now.ToBinary().ToString() + PasswdTo.salt);

                // Создаем сессию в базе
                coreDB.Auth_Sessions.Add(new AuthSession()
                {
                    IP = HttpContext.Connection.RemoteIpAddress.ToString(),
                    Session = authSession,
                    HashPasswdToRoot = SHA256.Text(SHA256.Text(PasswdRoot) + PasswdTo.salt),
                    Expires = DateTime.Now.AddDays(10)
                });
                coreDB.SaveChanges();

                // Ставим куки
                HttpContext.Response.Cookies.Append("authSession", authSession);
            }
            #endregion

            // Меняем пароль 2FA
            if (!string.IsNullOrWhiteSpace(Passwd2FA))
            {
                if (Passwd2FA.Length < 6)
                    return Json(new Text("Пароль '2FA' должен состоять минимум из 6 символов"));

                System.IO.File.WriteAllText(Folders.Passwd + "/2fa", SHA256.Text(Passwd2FA));
            }

            // Ответ
            if (IsAPI)
                return Json(new TrueOrFalse(true));

            // Успех
            return Json(new Text("Настройки успешно сохранены"));
        }
    }
}
