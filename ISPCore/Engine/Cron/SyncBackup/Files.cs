using System;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;
using System.Collections.Generic;
using ISPCore.Engine.SyncBackup;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ISPCore.Models.SyncBackup.ToolsEngine;
using ISPCore.Engine.Base;
using ISPCore.Models.SyncBackup.Operation;
using ISPCore.Models.SyncBackup.Tasks;
using ISPCore.Models.Databases;
using ISPCore.Models.Base;
using ISPCore.Engine.Base.SqlAndCache;
using System.Text;
using Trigger = ISPCore.Models.Triggers.Events.SyncBackup.Files;

namespace ISPCore.Engine.Cron.SyncBackup
{
    public class Files
    {
        #region Run
        static bool IsRun = false;
        public static void Run(CoreDB coreDB, IMemoryCache memoryCache)
        {
            if (IsRun)
                return;
            IsRun = true;

            #region Очистка базы 'Операции' и папки 'ReportSync'
            if (memoryCache.TryGetValue("CronSyncBackupIO:ClearDB", out DateTime CronSyncBackupClearDB))
            {
                // Если дата отличается от текущей
                if (CronSyncBackupClearDB.Day != DateTime.Now.Day)
                {
                    // Меняем режим доступа к SQL
                    SqlToMode.SetMode(SqlMode.Read);

                    // Обновляем кеш
                    memoryCache.Set("CronSyncBackupIO:ClearDB", DateTime.Now);

                    // Чистим базу
                    foreach (var note in coreDB.SyncBackup_Notations.AsNoTracking())
                    {
                        // Если записи больше 90 дней
                        if ((DateTime.Now - note.Time).TotalDays > 90)
                        {
                            // Удаляем заметку
                            coreDB.SyncBackup_Notations.RemoveAttach(coreDB, note.Id);
                        }
                    }

                    // Меняем режим доступа к SQL
                    SqlToMode.SetMode(SqlMode.ReadOrWrite);

                    // Удаляем старые файлы
                    foreach (var intFile in Directory.GetFiles(Folders.ReportSync, "*.*"))
                    {
                        try
                        {
                            if ((DateTime.Now - File.GetLastWriteTime(intFile)).TotalDays > 90)
                                File.Delete(intFile);
                        }
                        catch { }
                    }

                    // Раз в сутки
                    GC.Collect(GC.MaxGeneration);
                }
            }
            else
            {
                // Создаем кеш задним числом
                memoryCache.Set("CronSyncBackupIO:ClearDB", DateTime.Now.AddDays(-1));
            }
            #endregion

            // Меняем режим доступа к SQL
            SqlToMode.SetMode(SqlMode.Read);

            // Получаем весь список заданий
            var Alltasks = coreDB.SyncBackup_Tasks.Include(f => f.FTP).Include(o => o.OneDrive).Include(dav => dav.WebDav).Include(ignr => ignr.IgnoreFileOrFolders).ToList();

            // Меняем режим доступа к SQL
            SqlToMode.SetMode(SqlMode.ReadOrWrite);

            // Проходим задания
            foreach (var task in Alltasks)
            {
                // Пропускаем задания которые не требуют выполнения
                if (task.JobStatus != JobStatus.on || task.LastSync > DateTime.Now.AddMinutes(-task.SuncTime))
                    continue;

                // Кеш 
                DateTime NewCacheSync = DateTime.Now;
                bool IsOk = false;

                #region Добовляем задание в WorkNote
                CancellationToken cancellationToken = new CancellationToken();
                var WorkNoteNotation = new Notation()
                {
                    TaskId = task.Id,
                    Category = "Бэкап",
                    Msg = $"Задание: {task.Description}",
                    Time = DateTime.Now,
                    More = new List<More>()
                    {
                        new More("Состояние", "Выполняется")
                    }
                };
                CoreDB.SyncBackupWorkNote.Add(WorkNoteNotation, cancellationToken);
                #endregion

                // Обновляем CacheSync
                if (task.CacheSync > task.CacheExpires)
                {
                    SqlToMode.SetMode(SqlMode.Read);
                    task.CacheSync = default(DateTime);
                    task.CacheExpires = DateTime.Now.AddDays(12);
                    coreDB.SaveChanges();
                    SqlToMode.SetMode(SqlMode.ReadOrWrite);
                }

                // 
                Trigger.OnStartJob((task.Id, task.TypeSunc));

                // Создание отчета по ошибкам
                Report report = new Report(task);

                // Выполняем задание
                Sync(task, new RemoteServer(task.TypeSunc, task.FTP, task.WebDav, task.OneDrive, report, out string NewRefreshTokenToOneDrive), WorkNoteNotation, out List <More> ResponseNameAndValue, ref IsOk);

                // Сохраняем отчет об ошибках (если есть ошибки)
                report.SaveAndDispose(ref ResponseNameAndValue);

                // Чистим WorkNote
                CoreDB.SyncBackupWorkNote.Take(cancellationToken);

                // Меняем режим доступа к SQL
                SqlToMode.SetMode(SqlMode.Read);

                // Добовляем задание в список завершеных операций
                coreDB.SyncBackup_Notations.Add(new Notation()
                {
                    TaskId = task.Id,
                    Category = "Бэкап",
                    Msg = $"Задание: {task.Description}",
                    Time = DateTime.Now,
                    More = ResponseNameAndValue,
                });

                // Завершаем задание
                if (IsOk)
                {
                    task.LastSync = DateTime.Now;
                    task.CacheSync = NewCacheSync;
                }

                // Меняем токен
                if (!string.IsNullOrWhiteSpace(NewRefreshTokenToOneDrive))
                    task.OneDrive.RefreshToken = NewRefreshTokenToOneDrive;

                coreDB.SaveChanges();

                // Меняем режим доступа к SQL
                SqlToMode.SetMode(SqlMode.ReadOrWrite);

                // 
                Trigger.OnStopJob((task.Id, task.TypeSunc, IsOk));
            }
            
            IsRun = false;
        }
        #endregion

        #region Sync
        /// <summary>
        /// Сихронизация задания
        /// </summary>
        /// <param name="task">Задания</param>
        /// <param name="serv">Удаленный сервер</param>
        /// <param name="WorkNoteNotation">Текущее задание в WorkNote</param>
        /// <param name="NameAndValue">Колекция для ответа в журнал</param>
        /// <param name="IsOk">Выполнено задание или нет</param>
        private static void Sync(Task task, RemoteServer serv, Notation WorkNoteNotation, out List<More> NameAndValue, ref bool IsOk)
        {
            #region Ошибка подключения
            if (!serv.IsConnected)
            {
                NameAndValue = new List<More>()
                {
                    new More("Состояние", $"Ошибка подключения к {task.TypeSunc.ToString()}")
                };
                return;
            }
            #endregion

            try
            {
                #region Получаем список папок в которых были ошибки при синхронизации
                string PathSyncToErrorLocalFolder = $"{Folders.Sync}/tk-{task.Id}.ErrorLocalFolders.json";
                var NewListErrorLocalFolders = new List<string>();    // Новый список папок с ошибкой синхронизации
                var oldListErrorLocalFolders = new List<string>();    // Текущий список папок с ошибкой синхронизации
                if (File.Exists(PathSyncToErrorLocalFolder))
                    oldListErrorLocalFolders = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(PathSyncToErrorLocalFolder));
                #endregion

                #region Переменные
                // Список новых папок 
                var ListNewLocalFolders = new List<string>();

                // Время старта задания
                var TimeStartTask = DateTime.Now;

                // Количиство созданных папок и загруженых файлов
                int CountUploadToFilesOK = 0, CountUploadToFilesAll = 0, CountCreateToDirectoryOk = 0, CountCreateToDirectoryAll = 0;

                // Количиство провереных обьектов - (папок/файлов)
                int CountToCheckedObject = 0;

                // Общий размер переданых файлов в byte
                long CountUploadToBytes = 0;

                // Отчет созданных папок
                string ReportNameToCreateFolders = $"tk-{task.Id}_{DateTime.Now.ToString("dd-MM-yyy_HH-mm")}-{Generate.Passwd(6)}.folders.txt";
                StreamWriter ReportToCreateFolders = new StreamWriter($"{Folders.ReportSync}/{ReportNameToCreateFolders}", false, Encoding.UTF8);

                // Отчет загруженных файлов
                string ReportNameToUploadFiles = $"tk-{task.Id}_{DateTime.Now.ToString("dd-MM-yyy_HH-mm")}-{Generate.Passwd(6)}.files.txt";
                StreamWriter ReportToUploadFiles = new StreamWriter($"{Folders.ReportSync}/{ReportNameToUploadFiles}", false, Encoding.UTF8);
                #endregion

                // Получаем список всех папок
                foreach (var LocalFolder in SearchDirectory.Get(task.Whence))
                {
                    CountToCheckedObject++;

                    // Если папка в списке игнорируемых папок
                    if (task.IgnoreFileOrFolders.Exists(i => Regex.IsMatch(LocalFolder, i.Patch.Replace("\\", "/"), RegexOptions.IgnoreCase)))
                        continue;
                    
                    // Проверяем папку, нужно ее синхронизировать или нет
                    if (CacheLastWriteTimeToFiles(LocalFolder, task.CacheSync, ref CountToCheckedObject) ||          // Если дата изменения любого файла внутри папки LocalFolder выше чем CacheSync
                        Directory.GetCreationTime(LocalFolder) > task.CacheSync ||                                   // Если дата создания папки выше CacheSync
                        Directory.GetLastWriteTime(LocalFolder) > task.CacheSync ||                                  // Если дата изменения в папке выше CacheSync
                        ListNewLocalFolders.Exists(i => LocalFolder.Contains(i)) ||                                  // Если эта папка является новой, нужно сихронизировать все включая все подпапки в ней
                        oldListErrorLocalFolders.Exists(i => LocalFolder == i))                                      // Список папок в котрых в прошлый раз была ошибка при синхронизации
                    {
                        #region Переменные
                        // Количиство потоков
                        int ActiveConnections = 1;
                        switch (task.TypeSunc)
                        {
                            case TypeSunc.OneDrive:
                                ActiveConnections = 10;
                                break;
                            case TypeSunc.SFTP:
                                ActiveConnections = task.FTP.ActiveConnections;
                                break;
                        }

                        // Расширения файлов и папок
                        string SyncRemoveFileAddExtension = ".remove";
                        string SyncRemoveFoldersExtension = $".SyncRemove.{DateTime.Now.ToBinary()}";
                        #endregion

                        #region Локальный метод - "GetRemoteFolder"
                        string GetRemoteFolder()
                        {
                            // Удаленный каталог
                            string where = Tools.ConvertPatchToUnix(task.Where);

                            // Локальный каталог
                            string whence = Tools.ConvertPatchToUnix(task.Whence);

                            // Результат
                            return LocalFolder.Replace(whence, where);
                        }
                        #endregion

                        // Папка на удаленном сервере Linux
                        string RemoteFolder = GetRemoteFolder();

                        // Создаем папку на удаленом сервере
                        serv.CreateDirectory(RemoteFolder, NotReport: true);

                        // Список файлов и папок
                        var ListRemoteServer = serv.ListDirectoryAndFiles(RemoteFolder);
                        var ListLocalDirectoryToName = Directory.GetDirectories(LocalFolder, "*", SearchOption.TopDirectoryOnly).Select(i => Path.GetFileName(i)).ToList();
                        var ListLocalFilesToName = Directory.GetFiles(LocalFolder, "*", SearchOption.TopDirectoryOnly).
                                                   Where(dir => !task.IgnoreFileOrFolders.Exists(i => Regex.IsMatch(dir, i.Patch, RegexOptions.IgnoreCase))).
                                                   Select(dir => Path.GetFileName(dir)).ToList();

                        // Модель Tools
                        ToolsModel md = new ToolsModel()
                        {
                            serv = serv,
                            ActiveConnections = ActiveConnections,
                            LocalFolder = LocalFolder,
                            RemoteFolder = RemoteFolder,
                            NewListErrorLocalFolders = NewListErrorLocalFolders
                        };

                        // Переименовывем папки в 'SyncRemoveFoldersExtension' на удаленом сервере - (если их нету на локальном)
                        Tools.RenameToRemoveDirectory(md, ListRemoteServer.Directory, SyncRemoveFoldersExtension);

                        // Переименовывем файлы в 'SyncRemoveFileAddExtension' на удаленом сервере - (если их нету на локальном)
                        Tools.RenameToRemoveFiles(md, ListRemoteServer.Files, SyncRemoveFileAddExtension);

                        // Создаем папки на удаленом сервере - (папки есть на локальном но нету на удаленом сервере)
                        foreach (string createFolder in Tools.CreateToDirectory(md, ListRemoteServer.Directory, ListLocalDirectoryToName, ref CountCreateToDirectoryOk, ref CountCreateToDirectoryAll, ListNewLocalFolders))
                        {
                            // Сохраняем список созданных папок
                            ReportToCreateFolders.WriteLine(createFolder);
                        }

                        // Удаляем файлы которые не до конца загружены
                        Tools.DeleteFilesToErrorUpload(md, ref ListRemoteServer.Files, SyncRemoveFileAddExtension);

                        // Загружаем файлы на удаленный сервер - (если файла нету на сервере)
                        foreach (string uploadFile in Tools.UploadToFiles(md, ListRemoteServer.Files, ListLocalFilesToName, task.EncryptionAES, task.PasswdAES, ref CountUploadToFilesOK, ref CountUploadToFilesAll, ref CountUploadToBytes))
                        {
                            // Сохраняем список загруженных файлов
                            ReportToUploadFiles.WriteLine(uploadFile);
                        }

                        #region Очистка старых бекапов
                        if (task.CountActiveBackup >= 1)
                        {
                            if (task.CountActiveBackup == 1)
                            {
                                // Удаляем все папки и файлы с пометкй "SyncRemoveFoldersExtension или SyncRemoveFoldersExtension"
                                Tools.DeleteFilesOrDirectoryToRemove(md);
                            }

                            // Удаляем старые бекапы
                            Tools.DeleteFilesToActiveBackup(md, task.CountActiveBackup);
                        }

                        // Удаляем старые по времени бекапы
                        if (task.CountActiveDayBackup > 0)
                            Tools.DeleteFilesToActiveDayBackup(md, task.CountActiveDayBackup);
                        #endregion

                        #region Обновляем WorkNote
                        WorkNoteNotation.More = new List<More>
                        {
                            new More("Состояние", $"Выполняется {GetToWorkTime("")}"),
                            new More("Проверено объектов", $"{CountToCheckedObject:N0}")
                        };

                        if (CountUploadToFilesOK > 0)
                        {
                            WorkNoteNotation.More.Add(new More("Передано данных", ToSize(CountUploadToBytes)));
                            WorkNoteNotation.More.Add(new More("Загружено файлов", $"{CountUploadToFilesOK:N0}"));
                        }

                        if (CountCreateToDirectoryOk > 0)
                            WorkNoteNotation.More.Add(new More("Создано папок", $"{CountCreateToDirectoryOk:N0}"));
                        #endregion
                    }
                }

                // Закрываем потоки
                ReportToCreateFolders.Dispose();
                ReportToUploadFiles.Dispose();

                // Сохраняем новый список папок с ошибками, вместо старого
                File.WriteAllText(PathSyncToErrorLocalFolder, JsonConvert.SerializeObject(NewListErrorLocalFolders));

                #region Локальный метод GetToWorkTime
                string GetToWorkTime(string arg)
                {
                    var WorkTime = (DateTime.Now - TimeStartTask);
                    if (WorkTime.TotalSeconds <= 60)
                        return "";

                    if (WorkTime.TotalMinutes <= 60)
                        return $"{arg}{(int)WorkTime.TotalMinutes} {EndOfText.get("минуту", "минуты", "минут", (int)WorkTime.TotalMinutes)}";

                    return $"{arg}{(int)WorkTime.TotalHours} {EndOfText.get("час", "часа", "часов", (int)WorkTime.TotalHours)}";
                }
                #endregion

                #region Локальный метод ToSize
                string ToSize(double bytes)
                {
                    string[] Suffix = { "Byte", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
                    int index = 0;
                    while (bytes >= 1024)
                    {
                        bytes /= 1024;
                        index++;
                    }

                    return $"{bytes:N3} {Suffix[index]}";
                }
                #endregion

                #region Выполнено
                NameAndValue = new List<More>()
                {
                    new More("Состояние", $"Выполнено {GetToWorkTime("за ")}"),
                    new More("Проверено объектов", $"{CountToCheckedObject:N0}")
                };

                if (CountCreateToDirectoryAll > 0)
                    NameAndValue.Add(new More("Создано папок", $"<a href='/reports/sync/{ReportNameToCreateFolders}' target='_blank'>{CountCreateToDirectoryOk:N0} из {CountCreateToDirectoryAll:N0}</a>"));

                if (CountUploadToFilesAll > 0)
                {
                    NameAndValue.Add(new More("Загружено файлов", $"<a href='/reports/sync/{ReportNameToUploadFiles}' target='_blank'>{CountUploadToFilesOK:N0} из {CountUploadToFilesAll:N0}</a>"));
                    NameAndValue.Add(new More("Передано данных", ToSize(CountUploadToBytes)));
                }
                #endregion

                IsOk = true;
            }
            catch (Exception ex)
            {
                NameAndValue = new List<More>()
                {
                    new More("Состояние", "Ошибка при выполнении задания"),
                    new More("Код", ex.ToString())
                };
            }

            // Отключаемся от сервера
            serv.Disconnect();
        }
        #endregion

        #region GetLastWriteTimeToFiles
        /// <summary>
        /// Проверяет есть ли в папке файлы с датой модицикации выше чем дата CacheSync 
        /// </summary>
        /// <param name="LocalFolder">Локальная папка для проверки</param>
        /// <param name="CacheSync">Время прошлой синхронизации с сервером</param>
        /// <returns>True если в папке есть файл с датой модицикации выше чем дата CacheSync</returns>
        /// <param name="CountToCheckedObject">Количиство провереных объектов</param>
        private static bool CacheLastWriteTimeToFiles(string LocalFolder, DateTime CacheSync, ref int CountToCheckedObject)
        {
            // Достаем списк файлов
            var Files = Directory.GetFiles(LocalFolder, "*", SearchOption.TopDirectoryOnly);

            // Обновляем количиство провереных обьектов
            CountToCheckedObject += Files.Length;

            // Проверяем каждый файл
            foreach (var intFile in Files)
            {
                // Сравниваем дату модификации
                if (File.GetLastWriteTime(intFile) > CacheSync)
                    return true;
            }

            return false;
        }
        #endregion

        #region ClearingTemp
        private static bool IsClearTemp = false;
        public static void ClearingTemp(IMemoryCache memoryCache)
        {
            if (IsClearTemp)
                return;
            IsClearTemp = true;
            
            if (!memoryCache.TryGetValue("Cron-SyncBackup:ClearingTemp", out _))
            {
                memoryCache.Set("Cron-SyncBackup:ClearingTemp", (byte)1, TimeSpan.FromHours(8));

                foreach (string inFile in Directory.GetFiles(Folders.Temp.SyncBackup, "*.*"))
                {
                    // Если файл лежит больше суток
                    if (DateTime.Now.AddDays(-1) > File.GetLastWriteTime(inFile))
                        File.Delete(inFile);
                }
            }

            IsClearTemp = false;
        }
        #endregion
    }
}
