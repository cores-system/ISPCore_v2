using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Engine.Databases;
using System.Collections.Generic;
using ISPCore.Models.Databases;
using ISPCore.Models.SyncBackup.Tasks;
using ISPCore.Models.Response;
using ISPCore.Engine.Base;

namespace ISPCore.Controllers
{
    public class SyncBackupFilesToTask : ControllerToDB
    {
        [HttpGet]
        public IActionResult Index(bool ajax, int Id)
        {
            ViewData["Id"] = Id;
            ViewData["ajax"] = ajax;
            return View("~/Views/SyncBackup/Files/Task.cshtml", coreDB.SyncBackup_Tasks.FindAndInclude(Id, AsNoTracking: true));
        }

        #region Save
        [HttpPost]
        public JsonResult Save(Task task, FTP ftp, Models.SyncBackup.Tasks.WebDav webDav, OneDrive oneDrive, IDictionary<string, IgnoreFileOrFolders> ignr = null)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            #region Проверка данных
            if (string.IsNullOrWhiteSpace(task.Description))
                return Json(new Text("Имя задания не может быть пустым"));

            if (string.IsNullOrWhiteSpace(task.Whence))
                return Json(new Text("Локальный каталог не может быть пустым"));

            if (string.IsNullOrWhiteSpace(task.Where))
                return Json(new Text("Удаленный каталог не может быть пустым"));

            switch (task.TypeSunc)
            {
                case TypeSunc.SFTP:
                case TypeSunc.FTP:
                    {
                        if (string.IsNullOrWhiteSpace(ftp.HostOrIP) || string.IsNullOrWhiteSpace(ftp.Login) || (task.Id == 0 && string.IsNullOrWhiteSpace(ftp.Passwd)))
                            return Json(new Text("Настройки 'FTP/SFTP' имеют недопустимое значение"));
                        break;
                    }
                case TypeSunc.WebDav:
                    {
                        if (string.IsNullOrWhiteSpace(webDav.url) || string.IsNullOrWhiteSpace(webDav.Login) || (task.Id == 0 && string.IsNullOrWhiteSpace(webDav.Passwd)))
                            return Json(new Text("Настройки 'WebDav' имеют недопустимое значение"));
                        break;
                    }
                case TypeSunc.OneDrive:
                    {
                        if (string.IsNullOrWhiteSpace(oneDrive.ApplicationId) || (task.Id == 0 && string.IsNullOrWhiteSpace(oneDrive.RefreshToken)))
                            return Json(new Text("Настройки 'OneDrive' имеют недопустимое значение"));
                        break;
                    }
            }
            #endregion

            // Уленный сервер
            task.FTP = ftp;
            task.WebDav = webDav;
            task.OneDrive = oneDrive;

            // Новое задание 
            if (task.Id == 0)
            {
                // Проверка данных AES256
                if (task.EncryptionAES && string.IsNullOrWhiteSpace(task.PasswdAES))
                    return Json(new Text("Пароль для шифрования файлов не может быть пустым"));

                // Добовляем в базу
                coreDB.SyncBackup_Tasks.Add(task);

                // Сохраняем базу
                coreDB.SaveChanges();

                // Список игнорируемых файлов и папок
                coreDB.SyncBackup_Task_IgnoreFileOrFolders.AddRange(task.Id, ignr, out _);

                // Сохраняем базу
                coreDB.SaveChanges();

                // Отдаем Id записи в базе
                return Json(new RewriteToId(task.Id));
            }

            // Старое задание
            else
            {
                // Поиск задания
                if (coreDB.SyncBackup_Tasks.FindAndInclude(task.Id) is var FindTask && FindTask == null)
                    return Json(new Text("Задание не найдено"));

                #region Используем старый пароль для шифрования файлов
                if (task.EncryptionAES)
                {
                    if (string.IsNullOrWhiteSpace(task.PasswdAES))
                    {
                        if (!string.IsNullOrWhiteSpace(FindTask.PasswdAES))
                        {
                            task.PasswdAES = FindTask.PasswdAES;
                        }
                        else
                        {
                            return Json(new Text("Пароль для шифрования файлов не может быть пустым"));
                        }
                    }
                }
                #endregion

                #region Используем старый пароль для 'SFTP/FTP/WebDav/OneDrive'
                switch (task.TypeSunc)
                {
                    case TypeSunc.SFTP:
                    case TypeSunc.FTP:
                        {
                            if (string.IsNullOrWhiteSpace(task.FTP.Passwd))
                            {
                                if (!string.IsNullOrWhiteSpace(FindTask.FTP.Passwd))
                                {
                                    task.FTP.Passwd = FindTask.FTP.Passwd;
                                }
                                else
                                {
                                    return Json(new Text("Пароль для 'FTP/SFTP' не может быть пустым"));
                                }
                            }
                            break;
                        }
                    case TypeSunc.WebDav:
                        {
                            if (string.IsNullOrWhiteSpace(task.WebDav.Passwd))
                            {
                                if (!string.IsNullOrWhiteSpace(FindTask.WebDav.Passwd))
                                {
                                    task.WebDav.Passwd = FindTask.WebDav.Passwd;
                                }
                                else
                                {
                                    return Json(new Text("Пароль для 'WebDav' не может быть пустым"));
                                }
                            }
                            break;
                        }
                    case TypeSunc.OneDrive:
                        {
                            if (string.IsNullOrWhiteSpace(task.OneDrive.RefreshToken))
                            {
                                if (!string.IsNullOrWhiteSpace(FindTask.OneDrive.RefreshToken))
                                {
                                    task.OneDrive.RefreshToken = FindTask.OneDrive.RefreshToken;
                                }
                                else
                                {
                                    return Json(new Text("Пароль для 'OneDrive' не может быть пустым"));
                                }
                            }
                            break;
                        }
                }
                #endregion

                // Обновляем параметры задания
                CommonModels.Update(FindTask, task);

                // Удаляем список игнорируемых файлов
                coreDB.SyncBackup_Task_IgnoreFileOrFolders.RemoveAll(i => i.TaskId == task.Id);

                // Добовляем список игнорируемых файлов
                coreDB.SyncBackup_Task_IgnoreFileOrFolders.AddRange(task.Id, ignr, out _);

                // Обновляем кеш
                FindTask.CacheExpires = DateTime.Now.AddDays(12);

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
            if (coreDB.SyncBackup_Tasks.RemoveAttach(coreDB, Id))
                return Json(new TrueOrFalse(true));

            return Json(new Text("Ошибка ;("));
        }
        #endregion

        #region ClearingCache
        [HttpPost]
        public JsonResult ClearingCache(Task task, bool IsAPI = false)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            // Поиск задания
            if (coreDB.SyncBackup_Tasks.Find(task.Id) is var tk && tk == null)
                return Json(new Text("Задание не найдено"));

            // Сбиваем дату кеша
            tk.LastSync = default(DateTime);
            tk.CacheSync = default(DateTime);
            tk.CacheExpires = DateTime.Now.AddDays(12);

            // Сохраняем базу
            coreDB.SaveChanges();

            // Отдаем результат
            if (IsAPI)
                return Json(new TrueOrFalse(true));
            return Json(new Text("Кэш очищен"));
        }
        #endregion
    }
}
