using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Engine.Base;
using System.Threading.Tasks;
using ISPCore.Models.Response;
using ISPCore.Engine.Hash;
using ISPCore.Engine.Auth;
using ISPCore.Models.Databases;
using Microsoft.Extensions.Caching.Memory;
using ISPCore.Engine.Base.SqlAndCache;

namespace ISPCore.Controllers.core
{
    public class CoreCheckRecaptchaController : ControllerToDB
    {
        #region Base
        [HttpPost]
        async public Task<JsonResult> Base(string recaptchaKey, string IP, int HourCacheToUser, string hash)
        {
            if (string.IsNullOrWhiteSpace(recaptchaKey))
                return Json(new Text("recaptchaKey == null"));

            if (hash != md5.text($"{IP}:{HourCacheToUser}:{PasswdTo.salt}"))
                return Json(new Text("hash error"));

            // Проверяем reCAPTCHA
            if (await Recaptcha.Verify(recaptchaKey, jsonDB.Security.reCAPTCHASecret))
            {
                // Валидные куки
                string cookie = Engine.core.AntiBot.GetValidCookie(HourCacheToUser, IP);

                // Отдаем ответ
                return Json(new { result = true, cookie = cookie, HourToCookie = HourCacheToUser });
            }
            
            // Ошибка
            return Json(new Text("Verify == false"));
        }
        #endregion

        #region LimitRequest
        [HttpPost]
        async public Task<JsonResult> LimitRequest(string recaptchaKey, string IP, int ExpiresToMinute, string hash)
        {
            if (string.IsNullOrWhiteSpace(recaptchaKey))
                return Json(new Text("recaptchaKey == null"));

            if (hash != md5.text($"{IP}{ExpiresToMinute}:{PasswdTo.salt}"))
                return Json(new Text("hash error"));

            // Проверяем reCAPTCHA
            if (await Recaptcha.Verify(recaptchaKey, jsonDB.Security.reCAPTCHASecret))
            {
                // Создаем кеш
                memoryCache.Set(KeyToMemoryCache.LimitRequestToreCAPTCHA(IP), (0, ExpiresToMinute), TimeSpan.FromMinutes(ExpiresToMinute));

                // Отдаем ответ
                return Json(new TrueOrFalse(true));
            }

            // Ошибка
            return Json(new Text("Verify == false"));
        }
        #endregion
    }
}
