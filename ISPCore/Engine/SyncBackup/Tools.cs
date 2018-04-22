using ISPCore.Engine.Base;
using ISPCore.Models.SyncBackup.Operation;
using ISPCore.Models.SyncBackup.Tasks;
using ISPCore.Models.SyncBackup.ToolsEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ISPCore.Engine.SyncBackup
{
    public static class Tools
    {
        #region Recovery
        /// <summary>
        /// Восстановить данные
        /// </summary>
        /// <param name="task">Задания</param>
        /// <param name="serv">Удаленный сервер</param>
        /// <param name="WorkNoteNotation">Текущее задание в WorkNote</param>
        /// <param name="NameAndValue">Колекция для ответа в журнал</param>
        /// <param name="typeRecovery">Режим востановления файлов</param>
        /// <param name="DateRecovery">Отметка бекапа для востановления по дате</param>
        /// <param name="SearchToCurrentDirectory">Искать файлы только в текущем каталоге</param>
        public static void Recovery(Models.SyncBackup.Tasks.Task task, RemoteServer serv, Notation WorkNoteNotation, out List<More> NameAndValue, TypeRecovery typeRecovery, DateTime DateRecovery, bool SearchToCurrentDirectory)
        {
            try
            {
                #region Переменные
                // Список папок которые нужно пропустить
                var ListSkipFolders = new List<string>();

                // Время старта задания
                var TimeStartTask = DateTime.Now;

                // Количиство загруженых файлов и созданых папок
                int CountDownloadToFilesOK = 0, CountDownloadToFilesAll = 0, CountCreateToDirectoryOk = 0;

                // Общий размер загруженых файлов в byte
                long CountDownloadToBytes = 0;
                #endregion

                #region Получаем список папок
                // Текущая папка
                var RemoteFolders = new List<DirectoryModel>();
                RemoteFolders.Add(new DirectoryModel()
                {
                    Folder = task.Where,
                    RemoteCreated = default(DateTime),
                    RemoteLastModified = DateTime.Now,
                });

                // Все подкаталоги
                if (!SearchToCurrentDirectory)
                    serv.ListAllDirectory(task.Where, ref RemoteFolders);
                #endregion

                // Проходим список всех папок
                foreach (var RemoteFolder in RemoteFolders)
                {
                    // Папки и их подпапки которые не нужно восстанавливать
                    if (ListSkipFolders.Exists(i => RemoteFolder.Folder.Contains(i)))
                        continue;

                    // Пропускаем удаленые папки
                    if (typeRecovery == TypeRecovery.Topical && RemoteFolder.Folder.Contains(".SyncRemove"))
                        continue;

                    #region Локальный метод - "GetLocalFolder"
                    string GetLocalFolder()
                    {
                        // Удаленный каталог
                        string where = ConvertPatchToUnix(task.Where);

                        // Локальный каталог
                        string whence = ConvertPatchToUnix(task.Whence);

                        // Результат
                        return RemoteFolder.Folder.Replace(where, whence);
                    }
                    #endregion

                    // Локальная дириктория
                    string LocalFolder = GetLocalFolder();

                    #region Востановление по дате
                    if (typeRecovery == TypeRecovery.Date)
                    {
                        if (RemoteFolder.Folder.Contains(".SyncRemove"))
                        {
                            // Если дата модификации папки ниже отметки
                            if (RemoteFolder.RemoteLastModified < DateRecovery)
                            {
                                ListSkipFolders.Add(RemoteFolder.Folder);
                                continue;
                            }
                        }
                        else
                        {
                            // Если дата создания папки выше отметки
                            if (RemoteFolder.RemoteCreated > DateRecovery)
                            {
                                ListSkipFolders.Add(RemoteFolder.Folder);
                                continue;
                            }
                        }

                        // Удаляем 'SyncRemove' в 'LocalFolder'
                        LocalFolder = Regex.Replace(LocalFolder, @"\.SyncRemove\.-[0-9]+", "");
                    }
                    #endregion

                    // Создаем локальную папку
                    Directory.CreateDirectory(LocalFolder);
                    CountCreateToDirectoryOk++;

                    // Количиство потоков
                    int ActiveConnections = task.TypeSunc == TypeSunc.SFTP ? task.FTP.ActiveConnections : 1;

                    #region Загружаем файлы на локальный сервер
                    Parallel.ForEach(SortedFiles(serv.ListDirectoryAndFiles(RemoteFolder.Folder).Files, SortedToLocalTime: false), new ParallelOptions { MaxDegreeOfParallelism = ActiveConnections }, RemoteData =>
                    {
                        if (typeRecovery == TypeRecovery.Date)
                        {
                            // Ищем файл который ближе всего подходит к отметки бекапа
                            if (RemoteData.Value.FirstOrDefault(i => i.RemoteCreated < DateRecovery) is SortedModel item)
                            {
                                // Загружаем файл который подходит по дате
                                DownloadFile(item, RemoteData.Key);
                            }
                        }
                        else
                        {
                            // Загружаем актуальный файл
                            DownloadFile(RemoteData.Value[0], RemoteData.Key);

                            // Загружаем бекапы
                            if (typeRecovery == TypeRecovery.all)
                            {
                                // Пропускаем актуальный файл и загружаем остальные
                                foreach (var RemoteFile in RemoteData.Value.Skip(1))
                                {
                                    // Загружаем файл
                                    DownloadFile(RemoteFile, RemoteFile.Name.Replace(".sync.aes.", ".sync."));
                                }
                            }
                        }
                    });
                    #endregion

                    #region Обновляем WorkNote
                    if (CountDownloadToFilesOK > 0)
                    {
                        WorkNoteNotation.More = new List<More>
                        {
                            new More("Состояние", $"Выполняется {GetToWorkTime("")}"),
                            new More("Загружено файлов", $"{CountDownloadToFilesOK:N0}"),
                            new More("Получено данных", ToSize(CountDownloadToBytes))
                        };
                    }
                    #endregion

                    #region Локальный метод DownloadFile
                    void DownloadFile(FileModel RemoteFile, string LocalFileName)
                    {
                        // Пропускаем удаленые файлы
                        if (typeRecovery == TypeRecovery.Topical && RemoteFile.Name.Contains(".remove") || RemoteFile.FileSize < 1)
                            return;

                        // Пути к файлам
                        string LocalFilePatch = LocalFolder + LocalFileName;
                        string RemoteFilePatch = (RemoteFolder.Folder + "/" + RemoteFile.Name).Replace("//", "/");

                        // Проверяем на существование файла
                        if (!task.EncryptionAES && File.Exists(LocalFilePatch) && (new FileInfo(LocalFilePatch)).Length == RemoteFile.FileSize)
                            return;

                        int CountReDownload = 0;

                        // Загружаем удаленый файл в локальную папку
                        ReDownloadFile: if(serv.DownloadFile(LocalFilePatch, RemoteFilePatch, task.EncryptionAES, task.PasswdAES, RemoteFile.FileSize))
                        {
                            CountDownloadToFilesOK++;
                            CountDownloadToBytes += RemoteFile.FileSize;
                        }
                        else
                        {
                            if (++CountReDownload < 3)  // 3 попытки что-бы загрузить файл
                                goto ReDownloadFile;
                        }

                        CountDownloadToFilesAll++;
                    }
                    #endregion
                }

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
                    new More("Состояние", $"Выполнено {GetToWorkTime("за ")}")
                };

                if (CountCreateToDirectoryOk > 0)
                    NameAndValue.Add(new More("Создано папок", $"{CountCreateToDirectoryOk:N0}"));

                if (CountDownloadToFilesAll > 0)
                {
                    NameAndValue.Add(new More("Загружено файлов", $"{CountDownloadToFilesOK:N0} из {CountDownloadToFilesAll:N0}"));
                    NameAndValue.Add(new More("Получено данных", ToSize(CountDownloadToBytes)));
                }
                #endregion
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

        #region AddToListErrorLocalFolders
        /// <summary>
        /// Записывает папку в список папок с ошибкой синхронизации
        /// </summary>
        /// <param name="folder">Локальная папка</param>
        /// <param name="NewListErrorLocalFolders">Текущий список папок с ошибкой синхронизации</param>
        private static void AddToListErrorLocalFolders(string folder, ref List<string> NewListErrorLocalFolders)
        {
            if (!NewListErrorLocalFolders.Exists(i => folder == i))  // Если папки нету в списке
                NewListErrorLocalFolders.Add(folder);                // Добавить папку в список
        }
        #endregion

        #region RenameToRemoveDirectory
        /// <summary>
        /// Переименовывем папки в 'SyncRemoveFoldersExtension' на удаленом сервере - (если их нету на локальном)
        /// </summary>
        /// <param name="md">Стандартные параметры которые есть в каждом методе Tools</param>
        /// <param name="ListRemoteDirectoryToName">Список папок на удаленном сервере (имена)</param>
        /// <param name="SyncRemoveFoldersExtension">Расширение для удаленой папки</param>
        public static void RenameToRemoveDirectory(ToolsModel md, List<string> ListRemoteDirectoryToName, string SyncRemoveFoldersExtension)
        {
            bool IsErrorSync = false;
            Parallel.ForEach(ListRemoteDirectoryToName, new ParallelOptions { MaxDegreeOfParallelism = md.ActiveConnections }, RemoteDirectoryToName =>
            {
                // Пропускаем папки которые уже имею расширение 'SyncRemoveFoldersExtension'
                if (RemoteDirectoryToName.Contains(".SyncRemove"))
                    return;

                // Если папки нету на локальном сервере
                if (!Directory.Exists(md.LocalFolder + RemoteDirectoryToName))
                {
                    // Переименовываем папку на удаленном сервере, добовляя расширение 'SyncRemoveFoldersExtension'
                    if (!md.serv.Rename($"{md.RemoteFolder}{RemoteDirectoryToName}", $"{md.RemoteFolder}{RemoteDirectoryToName}{SyncRemoveFoldersExtension}"))
                        IsErrorSync = true;
                }
            });

            if (IsErrorSync) {                                                                 // Если была ошибка
                AddToListErrorLocalFolders(md.LocalFolder, ref md.NewListErrorLocalFolders);   // Добовляем папку в список папок с ошибкой синхронизаци
            }
        }
        #endregion

        #region RenameToRemoveFiles
        /// <summary>
        /// Переименовывем файлы в 'SyncRemoveFileAddExtension' на удаленом сервере - (если их нету на локальном)
        /// </summary>
        /// <param name="md">Стандартные параметры которые есть в каждом методе Tools</param>
        /// <param name="ListRemoteFiles">Список файлов на удаленом сервере</param>
        /// <param name="SyncRemoveFileAddExtension">Расширение для удаленого файла</param>
        public static void RenameToRemoveFiles(ToolsModel md, List<FileModel> ListRemoteFiles, string SyncRemoveFileAddExtension)
        {
            bool IsErrorSync = false;
            Parallel.ForEach(ListRemoteFiles, new ParallelOptions { MaxDegreeOfParallelism = md.ActiveConnections }, RemoteFile =>
            {
                // Пропускаем файлы которые уже имею расширение 'SyncRemoveFileAddExtension'
                if (RemoteFile.Name.Contains(".remove"))
                    return;

                // Если файла нету на локальном сервере
                if (!File.Exists(md.LocalFolder + Regex.Replace(RemoteFile.Name, @"\.sync\..*$", "")))
                {
                    // Переименовываем файл на удаленном сервере, добовляя расширение 'SyncRemoveFileAddExtension' 
                    if (!md.serv.Rename($"{md.RemoteFolder}{RemoteFile.Name}", $"{md.RemoteFolder}{RemoteFile.Name}{SyncRemoveFileAddExtension}"))
                        IsErrorSync = true;
                }
            });

            if (IsErrorSync) {                                                                 // Если была ошибка
                AddToListErrorLocalFolders(md.LocalFolder, ref md.NewListErrorLocalFolders);   // Добовляем папку в список папок с ошибкой синхронизаци
            }
        }
        #endregion

        #region CreateToDirectory
        /// <summary>
        /// Создает папки на удаленом сервере 
        /// Папки есть на локальном но нету на удаленом сервере
        /// </summary>
        /// <param name="md">Стандартные параметры которые есть в каждом методе Tools</param>
        /// <param name="ListRemoteDirectoryToName">Список папок на удаленном сервере (имена)</param>
        /// <param name="ListLocalDirectoryToName">Список папок на локальном сервере (имена)</param>
        /// <param name="CountCreateToDirectoryOK">Количиство успешно созданных папок</param>
        /// <param name="CountCreateToDirectoryAll">Общее количиство папок, которые нужно было создать</param>
        /// <param name="ListNewLocalFolders">Список новых папок - (обновляет список)</param>
        public static void CreateToDirectory(ToolsModel md, List<string> ListRemoteDirectoryToName, List<string> ListLocalDirectoryToName, 
                                             ref int CountCreateToDirectoryOK, ref int CountCreateToDirectoryAll, List<string> ListNewLocalFolders)
        {
            int CountCreateToDirectoryOKTmp = 0, CountCreateToDirectoryAllTmp = 0;

            // Считываем список папок на локальном сервере
            Parallel.ForEach(ListLocalDirectoryToName, new ParallelOptions { MaxDegreeOfParallelism = md.ActiveConnections }, LocalDirectoryName =>
            {
                // Проверяем есть ли имя локальной папки на удаленом сервере
                if (!ListRemoteDirectoryToName.Contains(LocalDirectoryName))
                {
                    #region Создаем папку на сервере и обновляем NewListErrorLocalFolders
                    if (md.serv.CreateDirectory($"{md.RemoteFolder}{LocalDirectoryName}"))                                    // Создаем папку в папке 'RemoteFolders' на удаленом сервере
                        CountCreateToDirectoryOKTmp++;                                                                        // Успех
                    else
                    {
                        // Папка не создана, добовляем основную папку 'LocalFolders/LocalDirectoryName' в список папок с ошибкой синхронизации
                        AddToListErrorLocalFolders($"{md.LocalFolder}{LocalDirectoryName}", ref md.NewListErrorLocalFolders);
                    }
                    #endregion

                    // Сумируем общее количиство папок, которые нужно было создать
                    CountCreateToDirectoryAllTmp++;

                    // Добовляем новую папку в лист
                    string NewFolder = $"{md.LocalFolder}{LocalDirectoryName}";             // Полный путь к локальной папке
                    if (!ListNewLocalFolders.Exists(i => NewFolder.Contains(i)))            // Если 'NewFolder' нету в списке новых папок
                    {
                        ListNewLocalFolders.Add(NewFolder);
                    }
                }
            });

            CountCreateToDirectoryOK += CountCreateToDirectoryOKTmp;
            CountCreateToDirectoryAll += CountCreateToDirectoryAllTmp;
        }
        #endregion

        #region DeleteFilesToErrorUpload
        /// <summary>
        /// Удаляем файлы которые не до конца загружены
        /// </summary>
        /// <param name="md">Стандартные параметры которые есть в каждом методе Tools</param>
        /// <param name="ListRemoteFiles">Список файлов на удаленом сервере</param>
        /// <param name="SyncRemoveFileAddExtension">Расширение для удаленого файла</param>
        public static void DeleteFilesToErrorUpload(ToolsModel md, ref List<FileModel> ListRemoteFiles, string SyncRemoveFileAddExtension)
        {
            // Список файлов которые нужно удалить из листа 'ListRemoteFiles'
            var ListRemoteFilesToDeleteItem = new List<FileModel>();

            bool IsErrorSync = false;
            Parallel.ForEach(ListRemoteFiles, new ParallelOptions { MaxDegreeOfParallelism = md.ActiveConnections }, RemoteFile =>
            {
                // Если у файла есть размер
                // Сравниваем имя файла с полученым размером, если размеры разные то удаляем файл
                if (RemoteFile.FileSize != -1 && !Regex.IsMatch(RemoteFile.Name, $"\\.{RemoteFile.FileSize}({SyncRemoveFileAddExtension})?$"))
                {
                    // Удаляем файл на удаленом сервере
                    if (md.serv.DeleteFile($"{md.RemoteFolder}{RemoteFile.Name}"))
                    {
                        // Заносим данные во временное хранилише 'ListRemoteFilesToDeleteItem'
                        ListRemoteFilesToDeleteItem.Add(RemoteFile);
                    }
                    else { IsErrorSync = true; }
                }
            });

            if (IsErrorSync) {                                                                 // Если была ошибка
                AddToListErrorLocalFolders(md.LocalFolder, ref md.NewListErrorLocalFolders);   // Добовляем папку в список папок с ошибкой синхронизаци
            }

            // Удаляем плохие файлы с листа
            foreach (var item in ListRemoteFilesToDeleteItem)
                ListRemoteFiles.Remove(item);
        }
        #endregion

        #region UploadToFiles
        /// <summary>
        /// Загружаем файлы на удаленный сервер - (если файла нету на сервере)
        /// </summary>
        /// <param name="md">Стандартные параметры которые есть в каждом методе Tools</param>
        /// <param name="ListRemoteFiles">Список файлов на удаленом сервере</param>
        /// <param name="ListLocalFilesToName">Список файлов на локальном сервере</param>
        /// <param name="EncryptionAES">Использовать шифрование файлов AES 256</param>
        /// <param name="PasswdAES">Пароль для шифрования файлов</param>
        /// <param name="CountUploadToFilesOK">Количиство успешно загруженых файлов на уделеный сервер</param>
        /// <param name="CountUploadToFilesAll">Общее количиство файлов, которые нужно было загрузить на удаленый сервер</param>
        /// <param name="CountUploadToBytes">Общий размер переданых файлов в byte</param>
        public static List<string> UploadToFiles(ToolsModel md, List<FileModel> ListRemoteFiles, List<string> ListLocalFilesToName, bool EncryptionAES, string PasswdAES, 
                                         ref int CountUploadToFilesOK, ref int CountUploadToFilesAll, ref long CountUploadToBytes)
        {
            long CountUploadToBytesTmp = 0;
            int CountUploadToFilesOKTmp = 0, CountUploadToFilesAllTmp = 0;
            List<string> uploadFiles = new List<string>();

            Parallel.ForEach(ListLocalFilesToName, new ParallelOptions { MaxDegreeOfParallelism = md.ActiveConnections }, FileName =>
            {
                string LocalFile = $"{md.LocalFolder}{FileName}";                                                           // Полный путь к локальному файлу
                FileInfo InfoLocalFile = new FileInfo(LocalFile);                                                           // Данные локального файла, 'LastWriteTime' и 'Length'
                string SyncExtension = $".sync{(EncryptionAES ? ".aes" : "")}.{InfoLocalFile.LastWriteTime.ToBinary()}";    // Расширение файла синхронизации
                string SyncExtensionCheck = $"{SyncExtension}{(EncryptionAES ? "" : $".{InfoLocalFile.Length}")}";          // Расширение файла синхронизации для проверки

                // Если локального файла нету на удаленом сервере
                if (!ListRemoteFiles.Exists(i => i.Name.Contains($"{FileName}{SyncExtensionCheck}")))
                {
                    // Загружаем файл на сервер
                    if (md.serv.UploadFile(LocalFile, $"{md.RemoteFolder}{FileName}{SyncExtension}.{InfoLocalFile.Length}", EncryptionAES, PasswdAES, out long FileSizeToAES))
                    {
                        CountUploadToFilesOKTmp++;
                        CountUploadToBytesTmp += FileSizeToAES == -1 ? InfoLocalFile.Length : FileSizeToAES;
                        uploadFiles.Add(LocalFile);
                    }
                    CountUploadToFilesAllTmp++;
                }
            });
            CountUploadToFilesOK += CountUploadToFilesOKTmp;
            CountUploadToFilesAll += CountUploadToFilesAllTmp;
            CountUploadToBytes += CountUploadToBytesTmp;
            
            if (CountUploadToFilesOKTmp != CountUploadToFilesAllTmp)                           // Если не все файлы залиты на удаленный сервер
                AddToListErrorLocalFolders(md.LocalFolder, ref md.NewListErrorLocalFolders);   // Добовляем папку в список папок с ошибкой синхронизаци

            // Список загруженных файлов
            return uploadFiles;
        }
        #endregion

        #region DeleteFilesOrDirectoryToRemove
        /// <summary>
        /// Удаляем папки и файлы с пометкй "remove/SyncRemove"
        /// </summary>
        /// <param name="md">Стандартные параметры которые есть в каждом методе Tools</param>
        public static void DeleteFilesOrDirectoryToRemove(ToolsModel md)
        {
            bool IsErrorSync = false;
            var res = md.serv.ListDirectoryAndFiles(md.RemoteFolder);
            var mass = res.Files.Select(n => n.Name).ToList();      // Список файлов на удаленом сервере - (имена)
            mass.AddRange(res.Directory);                           // Список папок на удаленом сервере  - (имена)

            Parallel.ForEach(mass, new ParallelOptions { MaxDegreeOfParallelism = md.ActiveConnections }, RemoteFileOrDirectoryName =>
            {
                // Если в имени есть расширение "remove"
                if (RemoteFileOrDirectoryName.Contains(".remove"))
                {
                    // Удаляем файл на удаленом сервере
                    if (!md.serv.DeleteFile($"{md.RemoteFolder}{RemoteFileOrDirectoryName}"))
                        IsErrorSync = true;
                }

                // Если в имени есть расширение "SyncRemove"
                if (RemoteFileOrDirectoryName.Contains(".SyncRemove"))
                {
                    // Удаляем  папку на удаленом сервере
                    if (!md.serv.DeleteDirectory($"{md.RemoteFolder}{RemoteFileOrDirectoryName}"))
                        IsErrorSync = true;
                }
            });

            if (IsErrorSync) {                                                                 // Если была ошибка
                AddToListErrorLocalFolders(md.LocalFolder, ref md.NewListErrorLocalFolders);   // Добовляем папку в список папок с ошибкой синхронизаци
            }
        }
        #endregion

        #region DeleteFilesToActiveBackup
        /// <summary>
        /// Удаляем старые бекапы
        /// </summary>
        /// <param name="md">Стандартные параметры которые есть в каждом методе Tools</param>
        /// <param name="CountActiveBackup">Максимальное количиство активных бекапов</param>
        public static void DeleteFilesToActiveBackup(ToolsModel md, int CountActiveBackup)
        {
            bool IsErrorSync = false;
            foreach (var data in SortedFiles(md.serv.ListDirectoryAndFiles(md.RemoteFolder).Files, SortedToLocalTime: false))
            {
                // Если колекция содержит больше файлов чем допустимо в 'CountActiveBackup'
                if (data.Value.Count > CountActiveBackup)
                {
                    Parallel.ForEach(data.Value.                                                                                 // Список файлов 'List<(DateTime LastWriteTime, string FullName)>'
                                          Skip(CountActiveBackup),                                                               // Пропускаем первые 'CountActiveBackup' файлов                    
                                          new ParallelOptions { MaxDegreeOfParallelism = md.ActiveConnections }, item =>         // Список оставшихся файлов
                    {
                        // Удаляем файл на удаленом сервере
                        if (!md.serv.DeleteFile($"{md.RemoteFolder}{item.Name}"))
                            IsErrorSync = true;
                    });
                }
            }

            if (IsErrorSync) {                                                                 // Если была ошибка
                AddToListErrorLocalFolders(md.LocalFolder, ref md.NewListErrorLocalFolders);   // Добовляем папку в список папок с ошибкой синхронизаци
            }
        }
        #endregion

        #region DeleteFilesToActiveDayBackup
        /// <summary>
        /// Удаляем старые по времени бекапы
        /// </summary>
        /// <param name="md"></param>
        /// <param name="CountActiveDayBackup"></param>
        public static void DeleteFilesToActiveDayBackup(ToolsModel md, int CountActiveDayBackup)
        {
            bool IsErrorSync = false;

            #region Удпляем файлы
            foreach (var data in SortedFiles(md.serv.ListDirectoryAndFiles(md.RemoteFolder).Files, SortedToLocalTime: false))
            {
                Parallel.ForEach(data.Value.                                                                            // Список файлов 'List<(DateTime LastWriteTime, string FullName)>'
                                      Skip(1).                                                                          // Пропускаем первый файл 
                                      Where(i => (DateTime.Now - i.RemoteCreated).TotalDays > CountActiveDayBackup),    // Получаем те файлы у которых истек срок хранения по времени              
                                      new ParallelOptions { MaxDegreeOfParallelism = md.ActiveConnections }, item =>    // Список оставшихся файлов
                {
                    // Удаляем файл на удаленом сервере
                    if (!md.serv.DeleteFile($"{md.RemoteFolder}{item.Name}"))
                        IsErrorSync = true;
                });
            }
            #endregion

            #region Удаляем папки
            foreach (var data in SortedDirectory(md.serv.ListDirectoryAndFiles(md.RemoteFolder).Directory))
            {
                Parallel.ForEach(data.Value.                                                                              // Список папок 
                                      Where(i => (DateTime.Now - i.DeleteTime).TotalDays > CountActiveDayBackup),         // Получаем те папки у которых истек срок хранения по времени              
                                      new ParallelOptions { MaxDegreeOfParallelism = md.ActiveConnections }, item =>      // Список оставшихся папок
                                      {
                                          // Удаляем папку на удаленом сервере
                                          if (!md.serv.DeleteDirectory($"{md.RemoteFolder}{item.Name}"))
                                              IsErrorSync = true;
                                      });
            }
            #endregion

            if (IsErrorSync) {                                                                 // Если была ошибка
                AddToListErrorLocalFolders(md.LocalFolder, ref md.NewListErrorLocalFolders);   // Добовляем папку в список папок с ошибкой синхронизаци
            }
        }
        #endregion

        #region SortedFiles
        /// <summary>
        /// Сортирует список файлов по дате
        /// </summary>
        /// <param name="ListRemoteFiles">Список файлов на удаленом сервере</param>
        private static Dictionary<string, List<SortedModel>> SortedFiles(List<FileModel> ListRemoteFiles, bool SortedToLocalTime = true)
        {
            // Колекция содержит 
            // 1) Имя файла без расширения
            // 2) Весь список файлов для имени '1'
            //    I.   RemoteLastModified - Время модификации файла на удаленом сервере
            //    II.  LastWriteTime      - Оригинальное время модификации файла
            //    III. Name               - Полное имя файла включая расширение sync
            //    IV.  Размер файла на удаленом сервере
            var mass = new Dictionary<string, List<SortedModel>>();
            var tmp = new Dictionary<string, List<SortedModel>>();

            #region Читаем список файлов на удаленом сервере и конвертируем в 'mass'
            foreach (var RemoteFile in ListRemoteFiles)
            {
                try
                {
                    var g = new Regex(@"^(.*)\.sync(\.aes)?\.(-[0-9]+)\.[0-9]+(.remove)?$").Match(RemoteFile.Name).Groups;
                    var FileName = g[1].Value;                                                                                 // Получаем имя файла без расширения
                    DateTime LastWriteTime = DateTime.FromBinary(long.Parse(g[3].Value));                                      // Получаем оригинальное время модификации файла

                    #region Заполняем 'tmp'
                    var model = new SortedModel()
                    {
                        LocalLastWriteTime = LastWriteTime,
                        FileSize = RemoteFile.FileSize,
                        Name = RemoteFile.Name,
                        RemoteCreated = RemoteFile.RemoteCreated,
                        RemoteLastModified = RemoteFile.RemoteLastModified
                    };


                    if (tmp.TryGetValue(FileName, out var item))
                    {
                        item.Add(model);
                    }
                    else
                    {
                        tmp.Add(FileName, new List<SortedModel>() { model });
                    }
                    #endregion
                }
                catch { }
            }
            #endregion

            // Сортируем список файлов по дате
            foreach (var data in tmp)
            {
                if (SortedToLocalTime) {
                    mass.Add(data.Key, data.Value.OrderByDescending(b => b.LocalLastWriteTime).ToList());
                }
                else
                {
                    mass.Add(data.Key, data.Value.OrderByDescending(b => b.RemoteCreated).ToList());
                }
            }

            // Отдаем отсортированый список файлов
            return mass;
        }
        #endregion

        #region SortedDirectory
        /// <summary>
        /// Сортирует список папок по дате
        /// </summary>
        /// <param name="ListRemoteDirectorys">Список папок на удаленом сервере</param>
        private static Dictionary<string, List<(string Name, DateTime DeleteTime)>> SortedDirectory(List<string> ListRemoteFolderss)
        {
            // Колекция содержит 
            // 1) Имя папки без расширения
            // 2) Весь список папок для имени '1'
            //    II.  LastWriteTime      - Время удаления папки
            //    III. Name               - Полное имя файла включая расширение sync
            var mass = new Dictionary<string, List<(string Name, DateTime DeleteTime)>>();
            var tmp = new Dictionary<string, List<(string Name, DateTime DeleteTime)>>();

            // Читаем список папок на удаленом сервере и конвертируем в 'mass'
            foreach (string RemoteFolder in ListRemoteFolderss)
            {
                try
                {
                    var g = new Regex(".SyncRemove.(-[0-9]+)$").Match(RemoteFolder).Groups;
                    var FileName = Path.GetFileName(Regex.Replace(RemoteFolder, ".SyncRemove.(-[0-9]+)$", ""));     // Получаем имя папки без расширения
                    DateTime RemoteDelete = DateTime.FromBinary(long.Parse(g[1].Value));                            // Получаем время удаления папки

                    #region Заполняем 'tmp'
                    var model = (Path.GetFileName(RemoteFolder), RemoteDelete);

                    if (tmp.TryGetValue(FileName, out var item))
                    {
                        item.Add(model);
                    }
                    else
                    {
                        tmp.Add(FileName, new List<(string Name, DateTime DeleteTime)>() { model });
                    }
                    #endregion
                }
                catch { }
            }

            // Сортируем список папок по дате
            foreach (var data in tmp) {
                mass.Add(data.Key, data.Value.OrderByDescending(b => b.DeleteTime).ToList());
            }

            // Отдаем отсортированый список папок
            return mass;
        }
        #endregion

        #region ConvertPatchToUnix
        public static string ConvertPatchToUnix(string patch)
        {
            return Regex.Replace(patch.Replace("\\", "/"), "/$", "") + "/";
        }
        #endregion
    }
}
