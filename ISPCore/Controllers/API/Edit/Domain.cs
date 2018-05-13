using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.RequestsFilter.Domains;
using ISPCore.Models.Databases;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ISPCore.Engine.core.Cache.CheckLink;
using ISPCore.Models.Response;
using System.Text.RegularExpressions;
using ISPCore.Engine.Databases;
using ISPCore.Models.RequestsFilter.Domains.Log;
using Trigger = ISPCore.Models.Triggers.Events.RequestsFilter.Domain;

namespace ISPCore.Controllers
{
    public class ApiEditDomain : ControllerToDB
    {
        #region Edit
        public JsonResult Edit<T>(int DomainId, T oldItem, T newItem) where T : class
        {
            if (newItem == null)
                return Json(new TrueOrFalse(false));

            // Обновляем настройки
            CommonModels.Update(oldItem, newItem, HttpContext);

            // Сохраняем базу
            coreDB.SaveChanges();

            // Удаляем кеш для домена
            ISPCache.RemoveDomain(DomainId);

            // Успех
            return Json(new TrueOrFalse(true));
        }
        #endregion

        #region Base
        public JsonResult Base(int Id, Domain domain)
        {
            #region Проверка данных на правильность
            if (HttpContext.Request.Query.TryGetValue("host", out _))
            {
                // Проверяем имя домена на null
                if (string.IsNullOrWhiteSpace(domain?.host))
                    return Json(new Text("Имя домена не может быть пустым"));

                // Форматируем host
                domain.host = Regex.Replace(domain.host.ToLower().Trim(), "^www\\.", "");

                // Проверяем нету ли в имени домена лишних символов
                if (!Regex.IsMatch(domain.host, "^[a-z0-9-\\.]+$", RegexOptions.IgnoreCase))
                    return Json(new Text($"Домен {domain.host} не должен содержать тип протокола или url"));
            }
            #endregion

            // Поиск домена
            if (coreDB.RequestsFilter_Domains.Where(i => i.Id == Id).FirstOrDefault() is Domain item)
            {
                Trigger.OnChange((Id, "Base"));
                return Edit(Id, item, domain);
            }

            return Json(new Text("Домен не найден"));
        }
        #endregion

        #region LogSettings
        public JsonResult LogSettings(int Id, ConfToLog conf)
        {
            // Поиск домена
            if (coreDB.RequestsFilter_Domains.Where(i => i.Id == Id).Include(i => i.confToLog).FirstOrDefault() is Domain item)
            {
                Trigger.OnChange((Id, "LogSettings"));
                return Edit(Id, item.confToLog, conf);
            }

            return Json(new Text("Домен не найден"));
        }
        #endregion

        #region AntiVirus
        public JsonResult AntiVirus(int Id, AntiVirus av)
        {
            // Поиск домена
            if (coreDB.RequestsFilter_Domains.Where(i => i.Id == Id).Include(i => i.av).FirstOrDefault() is Domain item)
            {
                // Если не указан каталог для сканирования, то любые настройки запрещены
                if (string.IsNullOrWhiteSpace(item.av.path) && string.IsNullOrWhiteSpace(av.path) || (HttpContext.Request.Query.TryGetValue("path", out _) && string.IsNullOrWhiteSpace(av.path)))
                    return Json(new Text("Укажите каталог для сканирования"));

                Trigger.OnChange((Id, "av"));
                return Edit(Id, item.av, av);
            }

            return Json(new Text("Домен не найден"));
        }
        #endregion

        #region AntiBot
        public JsonResult AntiBot(int Id, AntiBot antiBot)
        {
            // Поиск домена
            if (coreDB.RequestsFilter_Domains.Where(i => i.Id == Id).Include(i => i.AntiBot).FirstOrDefault() is Domain item)
            {
                Trigger.OnChange((Id, "AntiBot"));
                return Edit(Id, item.AntiBot, antiBot);
            }

            return Json(new Text("Домен не найден"));
        }
        #endregion

        #region LimitRequest
        public JsonResult LimitRequest(int Id, LimitRequest limitRequest)
        {
            // Поиск домена
            if (coreDB.RequestsFilter_Domains.Where(i => i.Id == Id).Include(i => i.limitRequest).FirstOrDefault() is Domain item)
            {
                Trigger.OnChange((Id, "LimitRequest"));
                return Edit(Id, item.limitRequest, limitRequest);
            }

            return Json(new Text("Домен не найден"));
        }
        #endregion
    }
}
