using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Engine.Hash;
using ISPCore.Engine.Auth;
using ISPCore.Engine.Base;
using ISPCore.Models.RequestsFilter.Domains;
using ISPCore.Engine;
using ISPCore.Models.Databases.json;
using ISPCore.Engine.Base.SqlAndCache;
using ISPCore.Models.Databases;
using ISPCore.Models.Auth;

namespace ISPCore.Controllers
{
    public class AuthToBaseController : ControllerToDB
    {
        [HttpGet]
        public IActionResult Index()
        {
            if (IsAuth.Auth(HttpContext.Request.Cookies, HttpContext.Connection.RemoteIpAddress.ToString(), out _))
                return LocalRedirect("/");
            
            return View("/Views/Auth/Base.cshtml");
        }

        #region Unlock
        [HttpPost]
        public JsonResult Unlock(string passwd)
        {
            // IP адрес пользователя
            string IP = HttpContext.Connection.RemoteIpAddress.ToString();

            // Проверяем пароль
            if (SHA256.Text(passwd) == PasswdTo.Root)
            {
                // Записываем в журнал
                JurnalAdd(IP, "Успешная авторизация");

                // Сессия
                string authSession = md5.text(DateTime.Now.ToBinary().ToString() + PasswdTo.salt);

                // Создаем сессию в базе
                coreDB.Auth_Sessions.Add(new AuthSession()
                {
                    IP = IP,
                    Session = authSession,
                    HashPasswdToRoot = SHA256.Text(SHA256.Text(passwd) + PasswdTo.salt),
                    Expires = DateTime.Now.AddDays(10)
                });
                coreDB.SaveChanges();

                // Ставим куки
                HttpContext.Response.Cookies.Append("authSession", authSession);

                // Удаляем список неудачных попыток
                LimitLogin.SuccessAuthorization(IP);

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

        #region SignOut
        [HttpGet]
        public LocalRedirectResult SignOut()
        {
            // Удаляем сессию в SQL
            if (HttpContext.Request.Cookies.TryGetValue("authSession", out string authSession))
            {
                using (var coreDB = Service.Get<CoreDB>())
                {
                    coreDB.Auth_Sessions.RemoveAll(i => i.Session == authSession);
                    coreDB.SaveChanges();
                }
            }

            // Удаляем куки
            HttpContext.Response.Cookies.Delete("authSession");
            return LocalRedirect("/auth");
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
