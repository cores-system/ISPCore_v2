using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases;
using System.Collections.Generic;
using ISPCore.Models.RequestsFilter.Domains;
using System.Text.RegularExpressions;
using ISPCore.Engine.core.Cache.CheckLink;
using ISPCore.Engine.Databases;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ISPCore.Models.Response;
using ISPCore.Engine.Base;
using Microsoft.AspNetCore.Http;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using ISPCore.Engine.Hash;

namespace ISPCore.Controllers
{
    public class RequestsFilterToDomainBaseController : ControllerToDB
    {
        [HttpGet]
        public IActionResult Index(int Id, bool ajax)
        {
            ViewData["Id"] = Id;
            ViewData["ajax"] = ajax;
            ViewData["RequestsFilter_Templates"] = coreDB.RequestsFilter_Templates;
            return View("~/Views/RequestsFilter/Domain/Index.cshtml", FindAndInclude(coreDB.RequestsFilter_Domains.AsNoTracking(), Id));
        }


        [HttpGet]
        public IActionResult Faq(int Id, bool ajax)
        {
            ViewData["Id"] = Id;
            ViewData["ajax"] = ajax;
            return View("~/Views/RequestsFilter/Domain/FAQ.cshtml");
        }


        #region Save
        [HttpPost]
        public JsonResult Save(Domain domain, IDictionary<string, TemplateId> templates = null)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            #region Проверка данных на правильность
            // Проверяем имя домена на null
            if (string.IsNullOrWhiteSpace(domain?.host))
                return Json(new Text("Имя домена не может быть пустым"));

            // Форматируем host
            domain.host = Regex.Replace(domain.host.ToLower().Trim(), "^www\\.", "");

            // Проверяем нету ли в имени домена лишних символов
            if (!Regex.IsMatch(domain.host, "^[a-z0-9-\\.]+$", RegexOptions.IgnoreCase))
                return Json(new Text($"Домен {domain.host} не должен содержать тип протокола или url"));
            #endregion

            // Пароль 2FA
            if (!string.IsNullOrWhiteSpace(domain.Auth2faToPasswd))
                domain.Auth2faToPasswd = domain.Auth2faToPasswd.StartsWith("sha256:") ? domain.Auth2faToPasswd.Replace("sha256:", "") : SHA256.Text(domain.Auth2faToPasswd);

            // Новый домен
            if (domain.Id == 0)
            {
                // AntiBotHashKey
                domain.AntiBot.HashKey = Generate.Passwd(12);

                // Добовляем в базу
                coreDB.RequestsFilter_Domains.Add(domain);

                // Сохраняем базу
                coreDB.SaveChanges();

                // Создаем шаблоны
                coreDB.RequestsFilter_Domain_TemplatesId.AddRange(domain.Id, templates, out _);

                // Сохраняем базу
                coreDB.SaveChanges();

                // Удаляем кеш для домена
                ISPCache.RemoveDomain(domain.Id);

                // Отдаем ответ
                return Json(new RewriteToId(domain.Id));
            }

            // Существующий
            else
            {
                // Поиск домена
                var FindDomain = coreDB.RequestsFilter_Domains.Where(i => i.Id == domain.Id).Include(i => i.Aliases).FirstOrDefault();
                if (FindDomain == null)
                    return Json(new Text("Домен не найден"));

                // Обновляем параметры домена
                CommonModels.Update(FindDomain, domain);

                // Удаляем текущие шаблоны
                coreDB.RequestsFilter_Domain_TemplatesId.RemoveAll(i => i.DomainId == domain.Id);

                // Записываем новые шаблоны
                coreDB.RequestsFilter_Domain_TemplatesId.AddRange(domain.Id, templates, out _);

                // Сохраняем базу
                coreDB.SaveChanges();

                // Удаляем кеш для домена
                ISPCache.RemoveDomain(domain.Id);

                // Отдаем сообщение и Id новых алиасов
                return Json(new Text("Настройки домена сохранены"));
            }
        }
        #endregion

        #region Remove
        [HttpPost]
        public JsonResult Remove(int Id)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            // Удаляем домен
            if (coreDB.RequestsFilter_Domains.RemoveAttach(coreDB, Id))
            {
                // Удаляем кеш для домена
                ISPCache.RemoveDomain(Id);

                // Отдаем результат
                return Json(new TrueOrFalse(true));
            }

            return Json(new Text("Ошибка ;("));
        }
        #endregion

        #region FindAndInclude
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="Id"></param>
        private Domain FindAndInclude(IQueryable<Domain> db, int Id)
        {
            return db.Where(i => i.Id == Id).Include(i => i.Aliases).Include(c => c.Templates).FirstOrDefault();
        }
        #endregion

        #region Export
        [HttpGet]
        public string Export(int Id)
        {
            // Поиск домена
            if (coreDB.RequestsFilter_Domains.FindAndInclude(Id) is var domain && domain == null)
                return JsonConvert.SerializeObject(new Text("Домен не найден"), Formatting.Indented);

            // Отдаем результат
            return Regex.Replace(JsonConvert.SerializeObject(domain, Formatting.Indented), "\"(Id|DomainId)\": +[0-9]+,", "\"$1\": 0,", RegexOptions.IgnoreCase);
        }
        #endregion

        #region Import
        [HttpPost]
        public JsonResult Import() => Json(Import(HttpContext));

        public TrueOrFalse Import(HttpContext context)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return new TrueOrFalse(false);
            #endregion

            bool res = false;
            if (context.Request.Form.Files.Count == 1)
            {
                using (MemoryStream mem = new MemoryStream())
                {
                    // Получаем файл
                    context.Request.Form.Files[0].CopyTo(mem);
                    var domain = JsonConvert.DeserializeObject<Domain>(Encoding.UTF8.GetString(mem.ToArray()));

                    // Добовляем в базу
                    coreDB.RequestsFilter_Domains.Add(domain);

                    // Сохраняем базу
                    coreDB.SaveChanges();
                    res = true;
                }
            }

            // Отдаем результат
            return new TrueOrFalse(res);
        }
        #endregion
    }
}
