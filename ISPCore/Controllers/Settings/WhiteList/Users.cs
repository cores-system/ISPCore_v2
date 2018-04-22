using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases.json;
using ISPCore.Engine;
using ISPCore.Engine.Hash;
using ISPCore.Engine.Auth;
using ISPCore.Engine.Base;
using System.Collections.Generic;
using ISPCore.Models.Base.WhiteList;
using ISPCore.Models.Response;
using ISPCore.Models.Databases.Interface;
using ISPCore.Models.Databases;
using ISPCore.Models.Auth;

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
            
            // Список Id WhiteList
            IDictionary<string, IId> NewWhiteList = null;

            // Записываем даннные из whiteList
            jsonDB.WhiteList.Values.UpdateOrAddRange(whiteList, out NewWhiteList);

            // Создаем новые Id
            foreach (var item in whiteList)
            {
                if (item.Value != null && item.Value.Id == 0)
                    item.Value.Id = int.Parse(Generate.Passwd(6, true));
            }

            // Сохраняем значения
            jsonDB.WhiteList.LastUpdateToConf = DateTime.Now;
            jsonDB.Save();

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
            jsonDB.WhiteList.Values.RemoveAll(i => i.Id == Id);
            jsonDB.WhiteList.LastUpdateToConf = DateTime.Now;

            // Успех
            jsonDB.Save();
            return Json(new TrueOrFalse(true));
        }
        #endregion
    }
}
