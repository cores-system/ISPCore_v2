﻿using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases.json;
using ISPCore.Engine.Hash;
using ISPCore.Engine.Auth;
using ISPCore.Engine.Base;
using ISPCore.Models.Response;
using ISPCore.Models.Databases;
using ISPCore.Models.Auth;
using ISPCore.Engine;
using ISPCore.Models.Databases.Enums;
using Trigger = ISPCore.Models.Triggers.Events.Settings.Base;

namespace ISPCore.Controllers
{
    public class SettingsToBaseController : ControllerToDB
    {
        [HttpGet]
        public IActionResult Index(bool ajax)
        {
            GoogleTo2FA TwoFacAuth = new GoogleTo2FA();
            var setupInfo = TwoFacAuth.GenerateSetupCode("ISPCore", HttpContext.Request.Host.Host, PasswdTo.Google2FA, 300, 300, useHttps: true);
            ViewBag.BarcodeImageUrl = setupInfo.QrCodeSetupImageUrl;

            ViewData["salt"] = PasswdTo.salt;
            ViewData["ajax"] = ajax;
            return View("/Views/Settings/Base.cshtml", jsonDB);
        }


        [HttpPost]
        public JsonResult Save(Base bs, API api, Security sc, Cache cache, BruteForceConf BrutConf, string PasswdRoot = null, string Passwd2FA = null, string salt = null, bool IsAPI = false)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            //
            bool EnableTo2FA = jsonDB.Base.EnableTo2FA;

            #region Обновляем базу
            jsonDB.Base = bs;
            jsonDB.Security = sc;
            jsonDB.Cache = cache;
            jsonDB.API = api;
            jsonDB.BruteForceConf = BrutConf;
            jsonDB.Save();
            #endregion

            // 
            Trigger.OnChange((0, 0));

            // Меняем соль
            if (!string.IsNullOrWhiteSpace(salt))
            {
                if (salt.Length < 18)
                    return Json(new Text("Соль должна состоять минимум из 18 символов"));

                Trigger.OnChangeSalt((PasswdTo.salt, salt));
                PasswdTo.salt = salt;
            }

            #region Меняем пароль root
            if (!string.IsNullOrWhiteSpace(PasswdRoot))
            {
                if (PasswdRoot.Length < 6)
                    return Json(new Text("Пароль 'Root' должен состоять минимум из 6 символов"));

                // 
                Trigger.OnChangePasswdRoot((PasswdTo.Root, PasswdRoot));

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

                Trigger.OnChangePasswd2FA((PasswdTo.FA, Passwd2FA));
                System.IO.File.WriteAllText(Folders.Passwd + "/2fa", SHA256.Text(Passwd2FA));
            }

            #region Включение 2FA
            if (!EnableTo2FA && bs.EnableTo2FA)
            {
                if (HttpContext.Request.Cookies.TryGetValue("authSession", out string authSession))
                {
                    using (var coreDB = Service.Get<CoreDB>())
                    {
                        if (coreDB.Auth_Sessions.FindItem(i => i.Session == authSession, TrackingType.Tracking) is AuthSession item)
                        {
                            item.Confirm2FA = true;
                            coreDB.SaveChanges();
                        }
                    }
                }
            }
            #endregion

            // Ответ
            if (IsAPI)
                return Json(new TrueOrFalse(true));

            // Успех
            return Json(new Text("Настройки успешно сохранены"));
        }
    }
}
