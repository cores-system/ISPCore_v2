using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Engine.Databases;
using ISPCore.Models.Databases;
using ISPCore.Models.Response;
using ISPCore.Engine.Base;
using ISPCore.Models.SyncBackup.Database;
using ISPCore.Models.SyncBackup.Database.Enums;

namespace ISPCore.Controllers
{
    public class SyncBackupDatabaseToTask : ControllerToDB
    {
        [HttpGet]
        public IActionResult Index(bool ajax, int Id)
        {
            ViewData["Id"] = Id;
            ViewData["ajax"] = ajax;
            return View("~/Views/SyncBackup/Database/Task.cshtml", coreDB.SyncBackup_db_Tasks.FindAndInclude(Id, AsNoTracking: true));
        }

        #region Save
        [HttpPost]
        public JsonResult Save(Task task, DumpConf conf, MySQL mysql)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            #region Проверка данных
            if (string.IsNullOrWhiteSpace(task.Description))
                return Json(new Text("Имя задания не может быть пустым"));

            if (string.IsNullOrWhiteSpace(conf.Whence))
                return Json(new Text("Локальный каталог не может быть пустым"));

            switch (task.TypeDb)
            {
                case TypeDb.MySQL:
                    {
                        if (string.IsNullOrWhiteSpace(mysql.Host) || string.IsNullOrWhiteSpace(mysql.User) || (task.Id == 0 && string.IsNullOrWhiteSpace(mysql.Password)))
                            return Json(new Text("Настройки 'MySQL' имеют недопустимое значение"));
                        break;
                    }
            }
            #endregion

            // Настройки
            task.Conf = conf;
            task.MySQL = mysql;

            // Новое задание 
            if (task.Id == 0)
            {
                // Добовляем в базу
                coreDB.SyncBackup_db_Tasks.Add(task);

                // Сохраняем базу
                coreDB.SaveChanges();

                // Отдаем Id записи в базе
                return Json(new RewriteToId(task.Id));
            }

            // Старое задание
            else
            {
                // Поиск задания
                if (coreDB.SyncBackup_db_Tasks.FindAndInclude(task.Id) is var FindTask && FindTask == null)
                    return Json(new Text("Задание не найдено"));

                #region Используем старый пароль для 'MySQL'
                switch (task.TypeDb)
                {
                    case TypeDb.MySQL:
                        {
                            if (string.IsNullOrWhiteSpace(task.MySQL.Password))
                            {
                                if (!string.IsNullOrWhiteSpace(FindTask.MySQL.Password))
                                {
                                    task.MySQL.Password = FindTask.MySQL.Password;
                                }
                                else
                                {
                                    return Json(new Text("Пароль для 'MySQL' не может быть пустым"));
                                }
                            }
                            break;
                        }
                }
                #endregion

                // Обновляем параметры задания
                CommonModels.Update(FindTask, task);

                // Сохраняем базу
                coreDB.SaveChanges();

                // Отдаем результат
                return Json(new Text("Задание сохранено"));
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

            // Удаляем задание
            if (coreDB.SyncBackup_db_Tasks.RemoveAttach(coreDB, Id))
                return Json(new TrueOrFalse(true));

            return Json(new Text("Ошибка ;("));
        }
        #endregion
    }
}
