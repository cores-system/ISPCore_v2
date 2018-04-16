using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Engine.Auth;
using ISPCore.Models.Databases;
using ISPCore.Models.Response;
using ISPCore.Models.Databases.Enums;
using ISPCore.Models.Auth;

namespace ISPCore.Controllers
{
    public class AuthToConfirmController : ControllerToDB
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View("/Views/Auth/Confirm.cshtml");
        }


        [HttpPost]
        public JsonResult Unlock(string code)
        {
            // Проверка кода
            GoogleTo2FA TwoFacAuth = new GoogleTo2FA();
            if (TwoFacAuth.ValidateTwoFactorPIN(PasswdTo.Google2FA, code))
            {
                // Берем сессию из кук 
                if (HttpContext.Request.Cookies.TryGetValue("authSession", out string authSession))
                {
                    // Текущая сессия
                    if (coreDB.Auth_Sessions.FindItem(i => i.Session == authSession, TrackingType.Tracking) is AuthSession item)
                    {
                        // 2FA пройдена
                        item.Confirm2FA = true;
                        coreDB.SaveChanges();

                        // Успех
                        return Json(new TrueOrFalse(true));
                    }
                }
            }

            // Ошибка
            return Json(new Text("Неверный код"));
        }
    }
}
