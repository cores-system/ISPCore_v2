using ISPCore.Engine;
using ISPCore.Engine.Base;
using ISPCore.Engine.Base.SqlAndCache;
using ISPCore.Engine.SyncBackup;
using ISPCore.Models.Base;
using ISPCore.Models.Databases;
using ISPCore.Models.Response;
using ISPCore.Models.SyncBackup.Operation;
using ISPCore.Models.SyncBackup.Tasks;
using ISPCore.Models.SyncBackup.ToolsEngine;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ISPCore.Controllers
{
    public class SyncBackupFilesToTools : Controller
    {
        [HttpGet]
        public IActionResult Index(bool ajax)
        {
            ViewData["ajax"] = ajax;
            return View("~/Views/SyncBackup/Files/Tools.cshtml");
        }
        

        [HttpPost]
        public JsonResult Recovery(Task task, TypeRecovery typeRecovery, IDictionary<string, string> nameAndValue, bool IsAPI = false)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            DateTime DateRecovery = default(DateTime);

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
                        if (string.IsNullOrWhiteSpace(task.FTP.HostOrIP) || string.IsNullOrWhiteSpace(task.FTP.Login) || string.IsNullOrWhiteSpace(task.FTP.Passwd))
                            return Json(new Text("Настройки 'FTP/SFTP' имеют недопустимое значение"));
                        break;
                    }
                case TypeSunc.WebDav:
                    {
                        if (string.IsNullOrWhiteSpace(task.WebDav.url) || string.IsNullOrWhiteSpace(task.WebDav.Login) || string.IsNullOrWhiteSpace(task.WebDav.Passwd))
                            return Json(new Text("Настройки 'WebDav' имеют недопустимое значение"));
                        break;
                    }
                case TypeSunc.OneDrive:
                    {
                        if (string.IsNullOrWhiteSpace(task.OneDrive.ApplicationId) || string.IsNullOrWhiteSpace(task.OneDrive.RefreshToken))
                            return Json(new Text("Настройки 'OneDrive' имеют недопустимое значение"));
                        break;
                    }
            }

            if (task.EncryptionAES && string.IsNullOrWhiteSpace(task.PasswdAES))
                return Json(new Text("Пароль для шифрования файлов не может быть пустым"));

            if (typeRecovery == TypeRecovery.Date && nameAndValue.TryGetValue("TypeRecoveryToDate", out string DateRecoveryTostring) && !DateTime.TryParse(DateRecoveryTostring, out DateRecovery))
                return Json(new Text("Отметка бэкапа имеет неправильный формат"));
            #endregion

            #region Добовляем задание в WorkNote
            CancellationToken cancellationToken = new CancellationToken();
            var WorkNoteNotation = new Notation()
            {
                TaskId = task.Id,
                Category = "Восстановление",
                Msg = $"Задание: {task.Description}",
                Time = DateTime.Now,
                More = new List<More>() { new More("Состояние", "Выполняется поиск всех папок") }
            };
            CoreDB.SyncBackupWorkNote.Add(WorkNoteNotation, cancellationToken);
            #endregion

            // Выполняем задание в потоке
            ThreadPool.QueueUserWorkItem(ob =>
            {
                // Создание отчета по ошибкам
                Report report = new Report(task);

                // Выполняем задание
                Tools.Recovery(task, new RemoteServer(task.TypeSunc, task.FTP, task.WebDav, task.OneDrive, report, out _), WorkNoteNotation, out List<More> ResponseNameAndValue, typeRecovery, DateRecovery);

                // Сохраняем отчет об ошибках (если есть ошибки)
                report.SaveAndDispose(ref ResponseNameAndValue);

                // Чистим WorkNote
                CoreDB.SyncBackupWorkNote.Take(cancellationToken);

                #region Сохраняем данные задание в базе
                SqlToMode.SetMode(SqlMode.Read);
                using (CoreDB coreDB = Service.Get<CoreDB>())
                {
                    // Добовляем задание в список завершеных операций
                    coreDB.SyncBackup_Notations.Add(new Notation()
                    {
                        TaskId = task.Id,
                        Category = "Восстановление",
                        Msg = $"Задание: {task.Description}",
                        Time = DateTime.Now,
                        More = ResponseNameAndValue,
                    });

                    // Сохраняем базу
                    coreDB.SaveChanges();
                }
                SqlToMode.SetMode(SqlMode.ReadOrWrite);
                #endregion
            });

            // Отдаем ответ
            if (IsAPI)
                return Json(new TrueOrFalse(true));
            return Json(new Text("Задание добавлено на обработку"));
        }
    }
}
