using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Response;
using ISPCore.Engine.core;

namespace ISPCore.Controllers.core
{
    public class CoreCheckCookieController : Controller
    {
        public JsonResult Index(string IP)
        {
            if (string.IsNullOrWhiteSpace(IP))
                IP = HttpContext.Connection.RemoteIpAddress.ToString();

            if (AntiBot.IsValidCookie(HttpContext, IP))
                return Json(new TrueOrFalse(true));

            if (HttpContext.Request.Cookies.TryGetValue("isp.ValidCookie", out _))
                return Json(new Text("Упс! Кажется что-то пошло не так"));

            return Json(new Text("В вашем браузере отключена поддержка Cookie"));
        }
    }
}
