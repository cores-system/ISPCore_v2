using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Engine.Hash;
using ISPCore.Engine.Auth;
using ISPCore.Engine.Base;
using ISPCore.Models.RequestsFilter.Domains;
using ISPCore.Engine;
using ISPCore.Models.Databases.json;
using ISPCore.Engine.Base.SqlAndCache;

namespace ISPCore.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            if (IsAuth.Auth(HttpContext.Request.Cookies, HttpContext.Connection.RemoteIpAddress.ToString()))
                return LocalRedirect("/");
            
            return View();
        }

        #region Unlock
        [HttpPost]
        public JsonResult Unlock(string passwd)
        {
            // IP адрес пользователя
            string IP = HttpContext.Connection.RemoteIpAddress.ToString();

            // Проверяем пароль
            if (md5.text(md5.text(passwd) + PasswdToMD5.salt) == PasswdToMD5.Root)
            {

                // Записываем в журнал
                JurnalAdd(IP, "Успешная авторизация");

                // Ставим куки
                HttpContext.Response.Cookies.Append("auth", PasswdToMD5.Root);

                // Удаляем список неудачных попыток
                LimitLogin.SuccessAuthorization(IP);

                // Уведомление в TelegramBot
                if (!Service.Get<JsonDB>().TelegramBot.EnabledToAuth)
                    TelegramBot.SendMsg($"Успешная авторизация в 'ISPCore Panel'\n{IP}");

                // Отдаем результат
                return Json(new Models.Response.TrueOrFalse(true));
            }

            // Записываем в журнал
            JurnalAdd(IP, "Неудачная попытка авторизации");

            // Записываем в базу IP адрес пользователя, который ввел неправильно пароль
            LimitLogin.FailAuthorization(IP, TypeBlockIP.global);

            // Отдаем результат
            return Json(new Models.Response.Text("Неверный пароль"));
        }
        #endregion

        #region JurnalAdd
        private void JurnalAdd(string ip, string msg)
        {
            var geo = GeoIP2.City(ip);

            // Записываем данные в журнал
            WriteLogTo.SQL(new Models.Home.Jurnal()
            {
                IP = ip,
                Msg = msg,
                City = geo.City,
                Country = geo.Country,
                Region = geo.Region,
                Time = DateTime.Now
            });
        }
        #endregion
    }
}
