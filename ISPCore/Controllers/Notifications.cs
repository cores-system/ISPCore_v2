using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases;
using Microsoft.EntityFrameworkCore;
using ISPCore.Engine.Common.Views;
using ISPCore.Models.Notification;

namespace ISPCore.Controllers
{
    public class NotificationsController : ControllerToDB
    {
        [HttpGet]
        public IActionResult Index(bool ajax, int page = 1)
        {
            // Убираем количиство новых уведомлений
            if (jsonDB.Base.CountNotification != 0)
            {
                jsonDB.Base.CountNotification = 0;
                jsonDB.Save();
            }

            // Выводим контент
            var navPage = new NavPage<Notation>(coreDB.Notations.AsNoTracking().Include(n => n.More), HttpContext, 15, page);
            return View(navPage, ajax);
        }
    }
}
