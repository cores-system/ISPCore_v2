using System;
using System.IO;
using System.Linq;
using System.Text;
using ISPCore.Engine.Base;
using ISPCore.Engine.Common.Views;
using ISPCore.Engine.Triggers;
using ISPCore.Models.Databases;
using ISPCore.Models.RequestsFilter.Templates;
using ISPCore.Models.Response;
using ISPCore.Models.Triggers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ISPCore.Controllers
{
    public class ToolsToTriggerSettings : ControllerToDB
    {
        public IActionResult Index(bool ajax, int Id = 0)
        {
            ViewData["Id"] = Id;
            ViewData["ajax"] = ajax;
            return View("~/Views/Tools/Trigger/Base.cshtml", RegisteredTriggers.List().Where(i => i.Id == Id).FirstOrDefault());
        }

        #region Save
        [HttpPost]
        public JsonResult Save(TriggerConf tgr, bool IsAPI = false)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            // Проверка данных
            if (string.IsNullOrWhiteSpace(tgr.TriggerName))
                return Json(new Text("Описание триггера не может быть пустым"));

            // Новый триггер
            if (tgr.Id == 0)
            {
                // Модель
                TriggerConf triggerConf = new TriggerConf()
                {
                    TriggerName = tgr.TriggerName,
                    Author = tgr.Author,
                    IsActive = tgr.IsActive
                };

                // Сохраняем файл
                System.IO.File.WriteAllText($"{Folders.Triggers}/{triggerConf.Id}.conf", JsonConvert.SerializeObject(triggerConf, Formatting.Indented));

                // Обновляем базу
                RegisteredTriggers.UpdateDB();

                // Отдаем ответ
                return Json(new RewriteToId(triggerConf.Id));
            }

            // Существующий
            else
            {
                // Поиск триггера
                var FindTrigger = RegisteredTriggers.List().Where(i => i.Id == tgr.Id).FirstOrDefault();
                if (FindTrigger == null)
                    return Json(new Text("Триггер не найден"));

                // Обновляем параметры
                FindTrigger.TriggerName = tgr.TriggerName;
                FindTrigger.Author = tgr.Author;
                FindTrigger.IsActive = tgr.IsActive;

                // Сохраняем файл
                System.IO.File.WriteAllText(FindTrigger.TriggerFile, JsonConvert.SerializeObject(FindTrigger, Formatting.Indented));

                // Обновляем базу
                RegisteredTriggers.UpdateDB();

                // Отдаем сообщение
                return Json(new Text("Настройки успешно сохранены"));
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
            
            if (RegisteredTriggers.List().Where(i => i.Id == Id).FirstOrDefault() is TriggerConf tgr)
            {
                RegisteredTriggers.Remove(tgr);
                return Json(new TrueOrFalse(true));
            }

            return Json(new Text("Ошибка ;("));
        }
        #endregion

        #region Export
        [HttpGet]
        public string Export(int Id)
        {
            if (RegisteredTriggers.List().Where(i => i.Id == Id).FirstOrDefault() is TriggerConf tgr)
                return System.IO.File.ReadAllText(tgr.TriggerFile);

            return "Ошибка ;(";
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
                    var tgr = JsonConvert.DeserializeObject<TriggerConf>(Encoding.UTF8.GetString(mem.ToArray()));

                    // Меняем Id
                    tgr.Id = int.Parse(Generate.Passwd(6, IsNumberCode: true));

                    // Сохраняем файл
                    System.IO.File.WriteAllText($"{Folders.Triggers}/{tgr.Id}.conf", JsonConvert.SerializeObject(tgr, Formatting.Indented));

                    // Обновляем базу
                    RegisteredTriggers.UpdateDB();

                    // Успех
                    res = true;
                }
            }

            // Отдаем результат
            return new TrueOrFalse(res);
        }
        #endregion
    }
}
