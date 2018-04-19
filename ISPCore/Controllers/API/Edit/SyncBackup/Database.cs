using ISPCore.Engine.Databases;
using ISPCore.Models.Databases;
using ISPCore.Models.Response;
using ISPCore.Models.SyncBackup.Database;
using ISPCore.Models.SyncBackup.Database.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

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
            if (coreDB.SyncBackup_db_Tasks.Where(i => i.Id == task.Id).Include(i => i.Conf).Include(i => i.MySQL).FirstOrDefault() is Task item)
            {
                #region Проверка данных
                if (string.IsNullOrWhiteSpace(task.Description))
                    return Json(new Text("Имя задания не может быть пустым"));

                switch (task.TypeDb)
                {
                    case TypeDb.MySQL:
                        {
                            if (string.IsNullOrWhiteSpace(item.MySQL.Host) || string.IsNullOrWhiteSpace(item.MySQL.User) || string.IsNullOrWhiteSpace(item.MySQL.Password))
                                return Json(new Text("Настройки 'MySQL' имеют недопустимое значение"));
                            break;
                        }
                }
                #endregion

                return Edit(item, task);
            }

            return Json(new Text("Задание не найдено"));
        }
        #endregion

        #region DumpConf
        public JsonResult DumpConf(int Id, DumpConf conf)
        {
            // Поиск задания
            if (coreDB.SyncBackup_db_Tasks.Where(i => i.Id == Id).Include(i => i.Conf).FirstOrDefault() is Task item)
            {
                // Проверка данных
                if (string.IsNullOrWhiteSpace(conf.Whence))
                    return Json(new Text("Локальный каталог не может быть пустым"));

                return Edit(item.Conf, conf);
            }

            return Json(new Text("Задание не найдено"));
        }
        #endregion

        #region MySQL
        public JsonResult MySQL(int Id, MySQL mysql)
        {
            // Поиск задания
            if (coreDB.SyncBackup_db_Tasks.Where(i => i.Id == Id).Include(i => i.MySQL).FirstOrDefault() is Task item)
            {
                // Проверка данных
                if (string.IsNullOrWhiteSpace(mysql.Host) || string.IsNullOrWhiteSpace(mysql.User) || string.IsNullOrWhiteSpace(mysql.Password))
                    return Json(new Text("Настройки 'MySQL' имеют недопустимое значение"));

                return Edit(item.MySQL, mysql);
            }

            return Json(new Text("Задание не найдено"));
        }
        #endregion
    }
}
