using ISPCore.Engine.Databases;
using ISPCore.Models.Databases;
using ISPCore.Models.Response;
using ISPCore.Models.SyncBackup.Database;
using ISPCore.Models.SyncBackup.Database.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Trigger = ISPCore.Models.Triggers.Events.SyncBackup.Database;

namespace ISPCore.Controllers
{
    public class ApiEditBackupDatabase : ControllerToDB
    {
        #region Edit
        public JsonResult Edit<T>(T oldItem, T newItem) where T : class
        {
            if (newItem == null)
                return Json(new TrueOrFalse(false));

            // Обновляем настройки
            CommonModels.Update(oldItem, newItem, HttpContext);

            // Сохраняем базу
            coreDB.SaveChanges();

            // Успех
            return Json(new TrueOrFalse(true));
        }
        #endregion

        #region Task
        public JsonResult Task(Task task)
        {
            // Поиск задания
            if (coreDB.SyncBackup_db_Tasks.Where(i => i.Id == task.Id).Include(i => i.DumpConf).Include(i => i.ConnectionConf).FirstOrDefault() is Task item)
            {
                #region Проверка данных
                if (string.IsNullOrWhiteSpace(task.Description))
                    return Json(new Text("Имя задания не может быть пустым"));

                switch (task.TypeDb)
                {
                    case TypeDb.MySQL:
                        {
                            if (string.IsNullOrWhiteSpace(item.ConnectionConf.Host) || string.IsNullOrWhiteSpace(item.ConnectionConf.User) || string.IsNullOrWhiteSpace(item.ConnectionConf.Password))
                                return Json(new Text($"Настройки '{task.TypeDb}' имеют недопустимое значение"));
                            break;
                        }
                }
                #endregion

                Trigger.OnChange((task.Id, 0));
                return Edit(item, task);
            }

            return Json(new Text("Задание не найдено"));
        }
        #endregion

        #region DumpConf
        public JsonResult DumpConf(int Id, DumpConf conf)
        {
            // Поиск задания
            if (coreDB.SyncBackup_db_Tasks.Where(i => i.Id == Id).Include(i => i.DumpConf).FirstOrDefault() is Task item)
            {
                // Проверка данных
                if (string.IsNullOrWhiteSpace(conf.Whence))
                    return Json(new Text("Локальный каталог не может быть пустым"));

                Trigger.OnChange((Id, 0));
                return Edit(item.DumpConf, conf);
            }

            return Json(new Text("Задание не найдено"));
        }
        #endregion

        #region ConnectionConf
        public JsonResult ConnectionConf(int Id, ConnectionConf conf)
        {
            // Поиск задания
            if (coreDB.SyncBackup_db_Tasks.Where(i => i.Id == Id).Include(i => i.ConnectionConf).FirstOrDefault() is Task item)
            {
                // Проверка данных
                if (string.IsNullOrWhiteSpace(conf.Host) || string.IsNullOrWhiteSpace(conf.User) || string.IsNullOrWhiteSpace(conf.Password))
                    return Json(new Text($"Настройки '{item.TypeDb.ToString()}' имеют недопустимое значение"));

                Trigger.OnChange((Id, 0));
                return Edit(item.ConnectionConf, conf);
            }

            return Json(new Text("Задание не найдено"));
        }
        #endregion
    }
}
