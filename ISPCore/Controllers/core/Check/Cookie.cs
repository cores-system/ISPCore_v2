using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Response;
using ISPCore.Engine.core;
using Trigger = ISPCore.Models.Triggers.Events.core.AntiBot;

namespace ISPCore.Controllers.core
{
    public class CoreCheckCookieController : Controller
    {
        public JsonResult Index(string IP, string AntiBotHashKey)
        {
            if (string.IsNullOrWhiteSpace(IP))
                IP = HttpContext.Connection.RemoteIpAddress.ToString();

            if (AntiBot.IsValidCookie(HttpContext, IP, AntiBotHashKey, out _))
            {
                Trigger.OnCheckCookie((true, IP, HttpContext.Request.Host.Host));
                return Json(new TrueOrFalse(true));
            }

            if (HttpContext.Request.Cookies.TryGetValue("isp.ValidCookie", out _))
            {
                Trigger.OnCheckCookie((false, IP, HttpContext.Request.Host.Host));
                return Json(new Text("Упс! Кажется что-то пошло не так"));
            }

            return Json(new Text("В вашем браузере отключена поддержка Cookie"));
        }
    }
}
