using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Engine;
using ISPCore.Models.Databases.json;
using ISPCore.Engine.Base;
using System.Threading.Tasks;
using ISPCore.Models.Response;
using ISPCore.Engine.Hash;
using ISPCore.Engine.Auth;

namespace ISPCore.Controllers.core
{
    public class CoreCheckRecaptchaController : Controller
    {
        [HttpPost]
        async public Task<JsonResult> Index(string recaptchaKey, int HourCacheToUser, string hash)
        {
            if (string.IsNullOrWhiteSpace(recaptchaKey))
                return Json(new Text("recaptchaKey == null"));

            if (hash != md5.text($"{HourCacheToUser}:{PasswdToMD5.salt}"))
                return Json(new Text("hash error"));

            // База
            var jsonDB = Service.Get<JsonDB>();

            // Проверяем reCAPTCHA
            if (await Recaptcha.Verify(recaptchaKey, jsonDB.Base.reCAPTCHASecret))
            {
                // Валидные куки
                string cookie = Engine.core.AntiBot.GetValidCookie(HourCacheToUser, HttpContext.Connection.RemoteIpAddress.ToString());

                // Отдаем ответ
                return Json(new { result = true, cookie = cookie, HourToCookie = HourCacheToUser });
            }
            
            // Ошибка
            return Json(new Text("Verify == false"));
        }
    }
}
