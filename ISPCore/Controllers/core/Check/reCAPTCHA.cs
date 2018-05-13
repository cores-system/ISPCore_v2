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
using Trigger = ISPCore.Models.Triggers.Events.core;

namespace ISPCore.Controllers.core
{
    public class CoreCheckRecaptchaController : ControllerToDB
    {
        #region Base
        [HttpPost]
        async public Task<JsonResult> Base(string recaptchaKey, string IP, int HourCacheToUser, string hash)
        {
            var res = await Verify(recaptchaKey, IP, HourCacheToUser, hash);
            if (res.res)
            {
                // Валидные куки
                string cookie = Engine.core.AntiBot.GetValidCookie(HourCacheToUser, IP, "reCAPTCHA", null);

                // 
                Trigger.AntiBot.OnRecaptchaVerify((true, IP, HttpContext.Request.Host.Host, HourCacheToUser));
                Trigger.AntiBot.OnSetValidCookie((IP, HttpContext.Request.Host.Host, cookie, "reCAPTCHA", HourCacheToUser));

                // Отдаем ответ
                return Json(new { result = true, cookie = cookie, HourToCookie = HourCacheToUser });
            }

            // Ошибка
            return Json(res.ob);
        }
        #endregion

        #region LimitRequest
        [HttpPost]
        async public Task<JsonResult> LimitRequest(string recaptchaKey, string IP, int ExpiresToMinute, string hash)
        {
            var res = await Verify(recaptchaKey, IP, ExpiresToMinute, hash);
            if (res.res)
            {
                // Создаем кеш
                memoryCache.Set(KeyToMemoryCache.LimitRequestToreCAPTCHA(IP), (0, ExpiresToMinute), TimeSpan.FromMinutes(ExpiresToMinute));

                // 
                Trigger.LimitRequest.OnRecaptchaVerify((true, IP, HttpContext.Request.Host.Host, ExpiresToMinute));

                // Отдаем ответ
                return Json(new TrueOrFalse(true));
            }

            // Ошибка
            Trigger.LimitRequest.OnRecaptchaVerify((false, IP, HttpContext.Request.Host.Host, ExpiresToMinute));
            return Json(res.ob);
        }
        #endregion


        #region private - Verify
        async Task<(bool res, object ob)> Verify(string recaptchaKey, string IP, int expires, string hash)
        {
            #region Проверка параметров
            if (string.IsNullOrWhiteSpace(recaptchaKey))
                return (false, new Text("recaptchaKey == null"));

            if (string.IsNullOrWhiteSpace(IP))
                IP = HttpContext.Connection.RemoteIpAddress.ToString();

            if (hash != md5.text($"{IP}:{expires}:{PasswdTo.salt}"))
                return (false, new Text("hash error"));
            #endregion

            // Проверяем reCAPTCHA
            if (await Recaptcha.Verify(recaptchaKey, jsonDB.Security.reCAPTCHASecret))
                return (true, null);

            // Ошибка
            return (false, new Text("Verify == false"));
        }
        #endregion
    }
}
