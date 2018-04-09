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

namespace ISPCore.Controllers
{
    public class SettingsController : Controller
    {
        JsonDB jsonDB;
        public SettingsController()
        {
            jsonDB = Service.Get<JsonDB>();
        }

        [HttpGet]
        public IActionResult Index(bool ajax)
        {
            ViewData["salt"] = PasswdToMD5.salt;
            ViewData["ajax"] = ajax;
            return View(jsonDB);
        }

        #region Save
        [HttpPost]
        public JsonResult Save(Base bs, API api, Security sc, Telega tlg, BruteForceConf BrutConf, IDictionary<string, WhiteListModel> whiteList, string PasswdRoot = null, string Passwd2FA = null, string salt = null, bool IsAPI = false)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            #region Достаем даннные из whiteList
            // Список Id WhiteList
            IDictionary<string, IId> NewWhiteList = null;

            if (!IsAPI && whiteList != null)
            {
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
            }
            #endregion

            #region Обновляем базу
            jsonDB.Base = bs;
            jsonDB.Security = sc;
            jsonDB.API = api;
            jsonDB.TelegramBot = tlg;
            jsonDB.BruteForceConf = BrutConf;
            jsonDB.Save();
            TelegramBot.CreateToOnCallback();
            #endregion

            // Меняем соль
            if (!string.IsNullOrWhiteSpace(salt))
            {
                if (salt.Length < 18)
                    return Json(new Text("Соль должена состоять минимум из 18 символов"));

                PasswdToMD5.salt = salt;
            }

            // Меняем пароль root
            if (!string.IsNullOrWhiteSpace(PasswdRoot))
            {
                if (PasswdRoot.Length < 6)
                    return Json(new Text("Пароль 'Root' должен состоять минимум из 6 символов"));

                System.IO.File.WriteAllText(Folders.Passwd + "/root", md5.text(PasswdRoot));
                HttpContext.Response.Cookies.Append("auth", PasswdToMD5.Root);
            }

            // Меняем пароль 2FA
            if (!string.IsNullOrWhiteSpace(Passwd2FA))
            {
                if (Passwd2FA.Length < 6)
                    return Json(new Text("Пароль '2FA' должен состоять минимум из 6 символов"));

                System.IO.File.WriteAllText(Folders.Passwd + "/2fa", md5.text(Passwd2FA));
            }

            // Ответ
            if (IsAPI)
                return Json(new TrueOrFalse(true));

            // Отдаем сообщение и Id новых настроек WhiteList
            return Json(new UpdateToIds("Настройки успешно сохранены", 0, NewWhiteList));
        }
        #endregion

        #region RemoveWhiteList
        [HttpPost]
        public JsonResult RemoveWhiteList(int Id)
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
