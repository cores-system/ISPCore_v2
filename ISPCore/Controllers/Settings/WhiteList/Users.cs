﻿using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases.json;
using ISPCore.Engine;
using ISPCore.Engine.Base;
using System.Collections.Generic;
using ISPCore.Models.Base.WhiteList;
using ISPCore.Models.Response;
using ISPCore.Models.Databases.Interface;
using ISPCore.Models.Databases;
using ISPCore.Engine.Base.SqlAndCache;
using ISPCore.Engine.Network;
using Microsoft.AspNetCore.Http;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using Trigger = ISPCore.Models.Triggers.Events.Settings.WhiteList;

namespace ISPCore.Controllers
{
    public class SettingsToUserWhiteList : ControllerToDB
    {
        [HttpGet]
        public IActionResult Index(bool ajax)
        {
            ViewData["ajax"] = ajax;
            return View("/Views/Settings/WhiteList/Users.cshtml", jsonDB);
        }

        #region Save
        [HttpPost]
        public JsonResult Save(IDictionary<string, WhiteListModel> whiteList)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            #region Проверка IP-адресов
            foreach (var item in whiteList)
            {
                if(!IPNetwork.CheckingSupportToIPv4Or6(item.Value.Value, out _))
                    return Json(new Text($"Not supported format: {item.Value.Value}"));
            }
            #endregion

            // Список Id WhiteList
            IDictionary<string, IId> NewWhiteList = null;

            // Записываем даннные из whiteList
            jsonDB.WhiteList.UpdateOrAddRange(whiteList, out NewWhiteList);

            // Создаем новые Id
            foreach (var item in whiteList)
            {
                if (item.Value != null && item.Value.Id == 0)
                    item.Value.Id = int.Parse(Generate.Passwd(6, true));
            }

            // Сохраняем значения
            jsonDB.Save();

            // Кеш настроек WhiteList
            WhiteUserList.UpdateCache();

            // 
            Trigger.OnChange((0, 0));

            // Отдаем сообщение и Id новых настроек WhiteList
            return Json(new UpdateToIds("Настройки успешно сохранены", 0, NewWhiteList));
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

            // База
            JsonDB jsonDB = Service.Get<JsonDB>();

            // Удаляем значение
            jsonDB.WhiteList.RemoveAll(i => i.Id == Id);
            jsonDB.Save();

            // Кеш настроек WhiteList
            WhiteUserList.UpdateCache();

            // 
            Trigger.OnChange((0, 0));

            // Успех
            return Json(new TrueOrFalse(true));
        }
        #endregion

        #region Export
        [HttpGet]
        public JsonResult Export()
        {
            return Json(jsonDB.WhiteList);
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
                    var mass = JsonConvert.DeserializeObject<List<WhiteListModel>>(Encoding.UTF8.GetString(mem.ToArray()));

                    // Добовляем в базу
                    jsonDB.WhiteList.AddRange(mass);

                    // Сохраняем базу
                    jsonDB.Save();
                    res = true;

                    // Кеш настроек WhiteList
                    WhiteUserList.UpdateCache();

                    // 
                    Trigger.OnChange((0, 0));
                }
            }

            // Отдаем результат
            return new TrueOrFalse(res);
        }
        #endregion
    }
}
