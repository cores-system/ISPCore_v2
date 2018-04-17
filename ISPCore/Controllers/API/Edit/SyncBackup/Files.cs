using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases;
using ISPCore.Models.Response;
using ISPCore.Engine.Databases;
using ISPCore.Models.SyncBackup.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ISPCore.Controllers
{
    public class ApiEditBackupFiles : ControllerToDB
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
            if (coreDB.SyncBackup_Tasks.Where(i => i.Id == task.Id).Include(i => i.FTP).Include(i => i.WebDav).Include(i => i.OneDrive).FirstOrDefault() is Task item)
            {
                #region Проверка данных
                if (string.IsNullOrWhiteSpace(item.Description) && string.IsNullOrWhiteSpace(task.Description) || (HttpContext.Request.Query.TryGetValue("description", out _) && string.IsNullOrWhiteSpace(task.Description)))
                    return Json(new Text("Имя задания не может быть пустым"));

                if (string.IsNullOrWhiteSpace(item.Whence) && string.IsNullOrWhiteSpace(task.Whence) || (HttpContext.Request.Query.TryGetValue("whence", out _) && string.IsNullOrWhiteSpace(task.Whence)))
                    return Json(new Text("Локальный каталог не может быть пустым"));

                if (string.IsNullOrWhiteSpace(item.Where) && string.IsNullOrWhiteSpace(task.Where) || (HttpContext.Request.Query.TryGetValue("where", out _) && string.IsNullOrWhiteSpace(task.Where)))
                    return Json(new Text("Удаленный каталог не может быть пустым"));
                
                if (HttpContext.Request.Query.TryGetValue("passwdaes", out _) && string.IsNullOrWhiteSpace(task.PasswdAES))
                    return Json(new Text("Пароль для шифрования файлов не может быть пустым"));

                if (HttpContext.Request.Query.TryGetValue("encryptionaes", out _) && task.EncryptionAES && (string.IsNullOrWhiteSpace(item.PasswdAES) && string.IsNullOrWhiteSpace(task.PasswdAES)))
                    return Json(new Text("Пароль для шифрования файлов не может быть пустым"));

                if (HttpContext.Request.Query.TryGetValue("typesunc", out _))
                {
                    switch (task.TypeSunc)
                    {
                        case TypeSunc.SFTP:
                        case TypeSunc.FTP:
                            {
                                if (string.IsNullOrWhiteSpace(item.FTP.HostOrIP) || string.IsNullOrWhiteSpace(item.FTP.Login) || string.IsNullOrWhiteSpace(item.FTP.Passwd))
                                    return Json(new Text("Настройки 'FTP/SFTP' имеют недопустимое значение"));
                                break;
                            }
                        case TypeSunc.WebDav:
                            {
                                if (string.IsNullOrWhiteSpace(item.WebDav.url) || string.IsNullOrWhiteSpace(item.WebDav.Login) || string.IsNullOrWhiteSpace(item.WebDav.Passwd))
                                    return Json(new Text("Настройки 'WebDav' имеют недопустимое значение"));
                                break;
                            }
                        case TypeSunc.OneDrive:
                            {
                                if (string.IsNullOrWhiteSpace(item.OneDrive.ApplicationId) || string.IsNullOrWhiteSpace(item.OneDrive.RefreshToken))
                                    return Json(new Text("Настройки 'OneDrive' имеют недопустимое значение"));
                                break;
                            }
                    }
                }
                #endregion

                return Edit(item, task);
            }

            return Json(new Text("Задание не найдено"));
        }
        #endregion

        #region FTP
        public JsonResult FTP(int Id, FTP ftp)
        {
            // Поиск задания
            if (coreDB.SyncBackup_Tasks.Where(i => i.Id == Id).Include(i => i.FTP).FirstOrDefault() is Task item)
            {
                #region Проверка данных
                if (string.IsNullOrWhiteSpace(item.FTP.HostOrIP) && string.IsNullOrWhiteSpace(ftp.HostOrIP) || (HttpContext.Request.Query.TryGetValue("hostorip", out _) && string.IsNullOrWhiteSpace(ftp.HostOrIP)))
                    return Json(new Text("Настройки 'FTP/SFTP' имеют недопустимое значение, укажите HostOrIP"));

                if (string.IsNullOrWhiteSpace(item.FTP.Login) && string.IsNullOrWhiteSpace(ftp.Login) || (HttpContext.Request.Query.TryGetValue("login", out _) && string.IsNullOrWhiteSpace(ftp.Login)))
                    return Json(new Text("Настройки 'FTP/SFTP' имеют недопустимое значение, укажите Login"));

                if (string.IsNullOrWhiteSpace(item.FTP.Passwd) && string.IsNullOrWhiteSpace(ftp.Passwd) || (HttpContext.Request.Query.TryGetValue("passwd", out _) && string.IsNullOrWhiteSpace(ftp.Passwd)))
                    return Json(new Text("Настройки 'FTP/SFTP' имеют недопустимое значение, укажите Passwd"));
                #endregion

                return Edit(item.FTP, ftp);
            }

            return Json(new Text("Задание не найдено"));
        }
        #endregion

        #region WebDav
        public JsonResult WebDav(int Id, Models.SyncBackup.Tasks.WebDav webDav)
        {
            // Поиск задания
            if (coreDB.SyncBackup_Tasks.Where(i => i.Id == Id).Include(i => i.WebDav).FirstOrDefault() is Task item)
            {
                #region Проверка данных
                if (string.IsNullOrWhiteSpace(item.WebDav.url) && string.IsNullOrWhiteSpace(webDav.url) || (HttpContext.Request.Query.TryGetValue("url", out _) && string.IsNullOrWhiteSpace(webDav.url)))
                    return Json(new Text("Настройки 'WebDav' имеют недопустимое значение, укажите url"));

                if (string.IsNullOrWhiteSpace(item.WebDav.Login) && string.IsNullOrWhiteSpace(webDav.Login) || (HttpContext.Request.Query.TryGetValue("login", out _) && string.IsNullOrWhiteSpace(webDav.Login)))
                    return Json(new Text("Настройки 'WebDav' имеют недопустимое значение, укажите Login"));

                if (string.IsNullOrWhiteSpace(item.WebDav.Passwd) && string.IsNullOrWhiteSpace(webDav.Passwd) || (HttpContext.Request.Query.TryGetValue("passwd", out _) && string.IsNullOrWhiteSpace(webDav.Passwd)))
                    return Json(new Text("Настройки 'WebDav' имеют недопустимое значение, укажите Passwd"));
                #endregion

                return Edit(item.WebDav, webDav);
            }

            return Json(new Text("Задание не найдено"));
        }
        #endregion

        #region OneDrive
        public JsonResult OneDrive(int Id, OneDrive oneDrive)
        {
            // Поиск задания
            if (coreDB.SyncBackup_Tasks.Where(i => i.Id == Id).Include(i => i.OneDrive).FirstOrDefault() is Task item)
            {
                #region Проверка данных
                if (string.IsNullOrWhiteSpace(item.OneDrive.ApplicationId) && string.IsNullOrWhiteSpace(oneDrive.ApplicationId) || (HttpContext.Request.Query.TryGetValue("applicationid", out _) && string.IsNullOrWhiteSpace(oneDrive.ApplicationId)))
                    return Json(new Text("Настройки 'OneDrive' имеют недопустимое значение, укажите ApplicationId"));

                if (string.IsNullOrWhiteSpace(item.OneDrive.RefreshToken) && string.IsNullOrWhiteSpace(oneDrive.RefreshToken) || (HttpContext.Request.Query.TryGetValue("refreshtoken", out _) && string.IsNullOrWhiteSpace(oneDrive.RefreshToken)))
                    return Json(new Text("Настройки 'OneDrive' имеют недопустимое значение, укажите RefreshToken"));
                #endregion

                return Edit(item.OneDrive, oneDrive);
            }

            return Json(new Text("Задание не найдено"));
        }
        #endregion
    }
}
