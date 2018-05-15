using FluentFTP;
using ISPCore.Engine.Base;
using ISPCore.Engine.Hash;
using ISPCore.Models.SyncBackup.Tasks;
using ISPCore.Models.SyncBackup.ToolsEngine;
using KoenZomers.OneDrive.Api;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using System.Web;
using WebDav;

namespace ISPCore.Engine.SyncBackup
{
    public class RemoteServer
    {
        #region Локальные переменные
        TypeSunc typeSunc;
        SftpClient sftp;
        FtpClient ftp;
        WebDavClient webDav;
        OneDriveGraphApi oneDrive;
        bool IsConnectedToOneDrive;
        Models.SyncBackup.Tasks.WebDav webDavConfToReport;
        Report report;
        bool ConnectionError;
        #endregion

        #region RemoteServer
        /// <summary>
        /// Создать подключение с удаленым сервером
        /// </summary>
        /// <param name="_typeSunc">Тип синхронизации</param>
        /// <param name="ftpConf">Конфигурация удаленого сервера 'ftp/sftp'</param>
        /// <param name="webDavConf">Конфигурация удаленого сервера 'WebDav'</param>
        /// <param name="oneDriveConf">Конфигурация удаленого сервера 'OneDrive'</param>
        /// <param name="report">Класс для создания отчета ошибок синхронизации</param>
        public RemoteServer(TypeSunc _typeSunc, FTP ftpConf, Models.SyncBackup.Tasks.WebDav webDavConf, OneDrive oneDriveConf, Report _report, out string NewRefreshTokenToOneDrive)
        {
            NewRefreshTokenToOneDrive = null;

            try
            {
                typeSunc = _typeSunc;
                report = _report;
                webDavConfToReport = webDavConf;
                switch (typeSunc)
                {
                    case TypeSunc.SFTP:
                        {
                            sftp = new SftpClient(ftpConf.HostOrIP, ftpConf.GetToPort(_typeSunc), ftpConf.Login, ftpConf.Passwd);
                            sftp.Connect();
                            break;
                        }

                    case TypeSunc.FTP:
                        {
                            try
                            {
                                // create an FTP client
                                ftp = new FtpClient(ftpConf.HostOrIP, ftpConf.GetToPort(_typeSunc), ftpConf.Login, ftpConf.Passwd);

                                // begin connecting to the server
                                ftp.Connect();
                            }
                            catch
                            {
                                ftp = new FtpClient(ftpConf.HostOrIP, ftpConf.GetToPort(_typeSunc), ftpConf.Login, ftpConf.Passwd);
                                ftp.EncryptionMode = FtpEncryptionMode.Explicit;
                                ftp.SslProtocols = SslProtocols.Tls;
                                ftp.ValidateCertificate += new FtpSslValidation(OnValidateCertificate);
                                ftp.Connect();

                                void OnValidateCertificate(FtpClient control, FtpSslValidationEventArgs e)
                                {
                                    // add logic to test if certificate is valid here
                                    e.Accept = true;
                                }
                            }
                            break;
                        }

                    case TypeSunc.WebDav:
                        {
                            webDav = new WebDavClient(new WebDavClientParams
                            {
                                BaseAddress = new Uri(webDavConf.url),
                                Credentials = new NetworkCredential(webDavConf.Login, webDavConf.Passwd),
                                PreAuthenticate = true,
                                UseDefaultCredentials = false  // Yandex.Disk на linux так работает
                            });
                            break;
                        }

                    case TypeSunc.OneDrive:
                        {
                            /// <summary>
                            /// Version 2.0.1.0
                            /// https://github.com/KoenZomers/OneDriveAPI
                            /// </summary>
                            oneDrive = new OneDriveGraphApi(oneDriveConf.ApplicationId);
                            oneDrive.AuthenticateUsingRefreshToken(oneDriveConf.RefreshToken).Wait();

                            if (!string.IsNullOrWhiteSpace(oneDrive.AccessToken.RefreshToken))
                            {
                                IsConnectedToOneDrive = true;
                                NewRefreshTokenToOneDrive = oneDrive.AccessToken.RefreshToken;
                            }
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                report.Connect(_typeSunc, ftpConf, webDavConf, oneDriveConf, ex.ToString());
                ConnectionError = true;
            }
        }
        #endregion

        #region IsConnected
        /// <summary>
        /// Статус подключения к удаленому серверу
        /// </summary>
        public bool IsConnected
        {
            get
            {
                if (ConnectionError)
                    return false;

                try
                {
                    switch (typeSunc)
                    {
                        case TypeSunc.SFTP: { return sftp.IsConnected; }
                        case TypeSunc.FTP: { return ftp.IsConnected; }
                        case TypeSunc.WebDav:
                            {
                                var res = webDav.Propfind("/").Result;
                                if (res.StatusCode != 401)
                                    return true;

                                report.Connect(typeSunc, null, webDavConfToReport, null, res);
                                return false;
                            }
                        case TypeSunc.OneDrive: { return IsConnectedToOneDrive; }
                    }

                    return true;
                }
                catch { return false; }
            }
        }
        #endregion

        #region Disconnect
        /// <summary>
        /// Отключится от сервера и очистить ресурсы
        /// </summary>
        public void Disconnect()
        {
            try
            {
                switch (typeSunc)
                {
                    case TypeSunc.SFTP:
                        {
                            sftp.Disconnect();
                            sftp.Dispose();
                            break;
                        }

                    case TypeSunc.FTP:
                        {
                            ftp.Disconnect();
                            ftp.Dispose();
                            break;
                        }

                    case TypeSunc.WebDav:
                        {
                            webDav.Dispose();
                            break;
                        }

                    case TypeSunc.OneDrive:
                        {
                            oneDrive.GetSignOutUri();
                            break;
                        }
                }
            }
            catch { }
        }
        #endregion

        #region ListDirectoryAndFiles
        /// <summary>
        /// Имена файлов и папок на удаленом сервере
        /// </summary>
        /// <param name="folder">Папка на удаленом сервере</param>
        /// <returns>Имена файлов и папок</returns>
        public (List<string> Directory, List<FileModel> Files) ListDirectoryAndFiles(string folder)
        {
            try
            {
                var Directory = new List<string>();  // Список папок
                var Files = new List<FileModel>();   // Список файлов

                switch (typeSunc)
                {
                    #region SFTP
                    case TypeSunc.SFTP:
                        {
                            foreach (var item in sftp.ListDirectory(folder))
                            {
                                if (item.IsDirectory)
                                {
                                    if (item.Name != ".." && item.Name != ".")
                                        Directory.Add(item.Name);
                                }
                                else
                                {
                                    Files.Add(new FileModel()
                                    {
                                        RemoteCreated = item.LastWriteTime,
                                        RemoteLastModified = item.LastWriteTime,
                                        FileSize = item.Length,
                                        Name = item.Name
                                    });
                                }
                            }
                            return (Directory, Files);
                        }
                    #endregion

                    #region FTP/FTPS
                    case TypeSunc.FTP:
                        {
                            foreach (var item in ftp.GetListing(folder))
                            {
                                if (item.Type == FtpFileSystemObjectType.Directory)
                                {
                                    Directory.Add(item.Name);
                                }
                                else
                                {
                                    Files.Add(new FileModel()
                                    {
                                        RemoteCreated = item.Modified,
                                        RemoteLastModified = item.Modified,
                                        FileSize = item.Size,
                                        Name = item.Name
                                    });
                                }
                            }
                            return (Directory, Files);
                        }
                    #endregion

                    #region WebDav
                    case TypeSunc.WebDav:
                        {
                            var res = webDav.Propfind(folder).Result;
                            if (res.IsSuccessful)
                            {
                                foreach (var item in res.Resources)
                                {
                                    if (string.IsNullOrWhiteSpace(item.ContentType))
                                    {
                                        if (folder != HttpUtility.UrlDecode(item.Uri))
                                            Directory.Add(HttpUtility.UrlDecode(item.DisplayName));
                                    }
                                    else
                                    {
                                        Files.Add(new FileModel()
                                        {
                                            RemoteCreated = (DateTime)item.CreationDate,
                                            RemoteLastModified = (DateTime)item.LastModifiedDate,
                                            FileSize = (long)item.ContentLength,
                                            Name = HttpUtility.UrlDecode(item.DisplayName)
                                        });
                                    }
                                }
                            }
                            else { report.Base("ListDirectoryAndFiles", folder, res); }

                            // Отдаем ответ
                            return (Directory, Files);
                        }
                    #endregion

                    #region OneDrive
                    case TypeSunc.OneDrive:
                        {
                            foreach (var item in (folder == "/" ? oneDrive.GetDriveRootChildren().Result.Collection : oneDrive.GetChildrenByPath(folder).Result.Collection))
                            {
                                if (item.Folder != null)
                                {
                                    Directory.Add(item.Name);
                                }
                                else
                                {
                                    Files.Add(new FileModel()
                                    {
                                        RemoteCreated = item.CreatedDateTime.DateTime,
                                        RemoteLastModified = item.LastModifiedDateTime.DateTime,
                                        FileSize = item.Size,
                                        Name = item.Name
                                    });
                                }
                            }

                            // Отдаем ответ
                            return (Directory, Files);
                        }
                        #endregion
                }
            }
            catch(Exception ex)
            {
                report.Base("ListDirectoryAndFiles", folder, ex.ToString());
            }

            return (new List<string>(), new List<FileModel>());
        }
        #endregion

        #region ListAllDirectory
        /// <summary>
        /// Получить список всех папок на удаленом сервере
        /// </summary>
        /// <param name="path">Папка на удаленом сервере</param>
        /// <param name="folders">Список найденых папок</param>
        public void ListAllDirectory(string path, ref List<DirectoryModel> folders)
        {
            try
            {
                switch (typeSunc)
                {
                    #region SFTP
                    case TypeSunc.SFTP:
                        {
                            // Получаем список всех файлов и папок
                            foreach (var item in sftp.ListDirectory(path))
                            {
                                if (item.IsDirectory)
                                {
                                    if (item.Name != ".." && item.Name != ".")
                                    {
                                        // Добовляем найденую папку в список 'folders'
                                        folders.Add(new DirectoryModel()
                                        {
                                            RemoteCreated = default(DateTime),
                                            RemoteLastModified = item.LastWriteTime,
                                            Folder = Tools.ConvertPatchToUnix(item.FullName)
                                        });

                                        // Получаем список папок и файлов внутри найденой папки
                                        ListAllDirectory(item.FullName, ref folders);
                                    }
                                }
                            }
                            break;
                        }
                    #endregion

                    #region FTP/FTPS
                    case TypeSunc.FTP:
                        {
                            // Получаем список всех папок
                            foreach (var item in ftp.GetListing(path))
                            {
                                if (item.Type == FtpFileSystemObjectType.Directory)
                                {
                                    // Полный путь к папке
                                    string FullName = Tools.ConvertPatchToUnix(path) + $"{item.Name}/";

                                    // Добовляем найденую папку в список 'folders'
                                    folders.Add(new DirectoryModel()
                                    {
                                        RemoteCreated = default(DateTime),
                                        RemoteLastModified = item.Modified,
                                        Folder = FullName
                                    });

                                    // Получаем список папок внутри найденой папки
                                    ListAllDirectory(FullName, ref folders);
                                }
                            }
                            break;
                        }
                    #endregion

                    #region WebDav
                    case TypeSunc.WebDav:
                        {
                            var res = webDav.Propfind(path).Result;
                            if (res.IsSuccessful)
                            {
                                // Получаем список всех файлов и папок
                                foreach (var item in res.Resources)
                                {
                                    if (string.IsNullOrWhiteSpace(item.ContentType) && !string.IsNullOrWhiteSpace(item.Uri))
                                    {
                                        if (path != HttpUtility.UrlDecode(item.Uri))
                                        {
                                            // Добовляем найденую папку в список 'folders'
                                            folders.Add(new DirectoryModel()
                                            {
                                                RemoteCreated = (DateTime)item.CreationDate,
                                                RemoteLastModified = (DateTime)item.LastModifiedDate,
                                                Folder = Tools.ConvertPatchToUnix(HttpUtility.UrlDecode(item.Uri))
                                            });

                                            // Получаем список папок и файлов внутри найденой папки
                                            ListAllDirectory(item.Uri, ref folders);
                                        }
                                    }
                                }
                            }
                            else { report.Base("ListAllDirectory", path, res); }
                            break;
                        }
                    #endregion

                    #region OneDrive
                    case TypeSunc.OneDrive:
                        {
                            // Получаем список всех файлов и папок
                            foreach (var item in (path == "/" ? oneDrive.GetDriveRootChildren().Result.Collection : oneDrive.GetChildrenByPath(path).Result.Collection))
                            {
                                if (item.Folder != null)
                                {
                                    // Полный путь к папке
                                    string FullName = Tools.ConvertPatchToUnix(path) + $"{item.Name}/";

                                    // Добовляем найденую папку в список 'folders'
                                    folders.Add(new DirectoryModel()
                                    {
                                        RemoteCreated = item.CreatedDateTime.DateTime,
                                        RemoteLastModified = item.LastModifiedDateTime.DateTime,
                                        Folder = FullName
                                    });

                                    // Получаем список папок и файлов внутри найденой папки
                                    ListAllDirectory(FullName, ref folders);
                                }
                            }
                            break;
                        }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                report.Base("ListAllDirectory", path, ex.ToString());
            }
        }
        #endregion

        #region CreateDirectory
        /// <summary>
        /// Создать папку на удаленом сервере
        /// </summary>
        /// <param name="path">Полный путь папки на сервере</param>
        /// <param name="NotReport">Не заносить ошибку в отчет</param>
        public bool CreateDirectory(string path, bool NotReport = false)
        {
            try
            {
                switch (typeSunc)
                {
                    case TypeSunc.SFTP:
                        {
                            sftp.CreateDirectory(path);
                            return true;
                        }

                    case TypeSunc.FTP:
                        {
                            ftp.CreateDirectory(path);
                            return true;
                        }

                    case TypeSunc.WebDav:
                        {
                            var res = webDav.Mkcol(path).Result;
                            if (res.StatusCode == 201)
                                return true;

                            if (!NotReport)
                                report.Base("CreateDirectory", path, res);
                            return createDirectory(path);
                        }

                    case TypeSunc.OneDrive:
                        {
                            oneDrive.GetFolderOrCreate(path).Wait();
                            return true;
                        }
                }
            }
            catch(Exception ex)
            {
                if (!NotReport)
                    report.Base("CreateDirectory", path, ex.ToString());
            }
            return false;
        }

        /// <summary>
        /// Создать полный путь к папке включая саму папку
        /// </summary>
        /// <param name="path">Полный путь папки на сервере</param>
        private bool createDirectory(string path)
        {
            try
            {
                switch (typeSunc)
                {
                    case TypeSunc.FTP:
                    case TypeSunc.SFTP: return false;

                    #region WebDav
                    case TypeSunc.WebDav:
                        {
                            string dirPath = "/";
                            foreach (string DirName in path.Split('/'))
                            {
                                if (string.IsNullOrWhiteSpace(DirName))
                                    continue;

                                dirPath += DirName + "/";
                                var res = webDav.Mkcol(dirPath).Result;

                                // 201 - Папка создана
                                // 409 - Папка уже существует
                                // 409 - Conflict
                                if (res.StatusCode == 201 || res.StatusCode == 405 || res.StatusCode == 409)
                                    continue;

                                // Отчет 
                                report.Base("createDirectory", dirPath, res);

                                // Любая ошибка кроме 201/405/409
                                return false;
                            }

                            // Успех
                            return true;
                        }
                    #endregion
                }
            }
            catch { }
            return false;
        }
        #endregion

        #region Rename
        /// <summary>
        /// Переименовать файл или папку на удаленом сервере
        /// </summary>
        /// <param name="oldPath">Полный путь к текущей папке</param>
        /// <param name="newPath">Полный путь к новой папке</param>
        public bool Rename(string oldPath, string newPath)
        {
            try
            {
                switch (typeSunc)
                {
                    case TypeSunc.SFTP:
                        {
                            sftp.RenameFile(oldPath, newPath);
                            break;
                        }

                    case TypeSunc.FTP:
                        {
                            ftp.Rename(oldPath, newPath);
                            break;
                        }

                    case TypeSunc.WebDav:
                        {
                            var res = webDav.Move(oldPath, newPath).Result;
                            if (!res.IsSuccessful)
                                report.Rename(oldPath, newPath, res);
                            break;
                        }

                    case TypeSunc.OneDrive:
                        {
                            oneDrive.Rename(oldPath, Path.GetFileName(newPath)).Wait();
                            break;
                        }
                }

                return true;
            }
            catch (Exception ex)
            {
                report.Rename(oldPath, newPath, ex.ToString());
                return false;
            }
        }
        #endregion

        #region DeleteFile
        /// <summary>
        /// Удалить файл
        /// </summary>
        /// <param name="path">Полный путь к файлу</param>
        public bool DeleteFile(string path)
        {
            try
            {
                switch (typeSunc)
                {
                    #region SFTP
                    case TypeSunc.SFTP:
                        {
                            // Удаляем файл
                            sftp.Delete(path);
                            break;
                        }
                    #endregion

                    #region FTP/FTPS
                    case TypeSunc.FTP:
                        {
                            // Удаляем файл
                            ftp.DeleteFile(path);
                            break;
                        }
                    #endregion

                    case TypeSunc.WebDav:
                        {
                            var res = webDav.Delete(path).Result;
                            if (!res.IsSuccessful)
                                report.Base("DeleteFile", path, res);
                            break;
                        }

                    case TypeSunc.OneDrive:
                        {
                            oneDrive.Delete(path).Wait();
                            break;
                        }
                }

                return true;
            }
            catch (Exception ex)
            {
                report.Base("DeleteFile", path, ex.ToString());
                return false;
            }
        }
        #endregion

        #region DeleteDirectory/ListDirectoryAndDeleteFile
        /// <summary>
        /// Удалить дирикторию
        /// </summary>
        /// <param name="path">Полный путь к папке</param>
        public bool DeleteDirectory(string path)
        {
            try
            {
                switch (typeSunc)
                {
                    #region SFTP
                    case TypeSunc.SFTP:
                        {
                            // Получаем список всех папок внутри и удаляем все файлы внутри папок
                            var folders = new List<string>();
                            folders.Add(path);
                            ListDirectoryAndDeleteFile(path, ref folders);
                            folders.Reverse();

                            // Удаляем папки
                            foreach (var folder in folders)
                            {
                                try
                                {
                                    sftp.DeleteDirectory(folder);
                                }
                                catch (Exception ex)
                                {
                                    report.Base("DeleteDirectory", path, ex.ToString());
                                }
                            }
                            break;
                        }
                    #endregion

                    #region FTP/FTPS
                    case TypeSunc.FTP:
                        {
                            try
                            {
                                ftp.DeleteDirectory(path);
                            }
                            catch (Exception ex)
                            {
                                report.Base("DeleteDirectory", path, ex.ToString());
                            }
                            break;
                        }
                    #endregion

                    case TypeSunc.WebDav:
                        {
                            var res = webDav.Delete(path).Result;
                            if (!res.IsSuccessful)
                                report.Base("DeleteDirectory", path, res);
                            break;
                        }

                    case TypeSunc.OneDrive:
                        {
                            oneDrive.Delete(path).Wait();
                            break;
                        }
                }

                return true;
            }
            catch (Exception ex)
            {
                report.Base("DeleteDirectory", path, ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Получить список всех папок внутри 'path' и удалить все файлы внутри найденых папок
        /// </summary>
        /// <param name="path">Полный путь к папке</param>
        /// <param name="folders">Список найденых папок</param>
        private void ListDirectoryAndDeleteFile(string path, ref List<string> folders)
        {
            try
            {
                switch (typeSunc)
                {
                    case TypeSunc.SFTP:
                        {
                            // Получаем список всех файлов и папок
                            foreach (var item in sftp.ListDirectory(path))
                            {
                                if (item.IsDirectory)
                                {
                                    if (item.Name != ".." && item.Name != ".")
                                    {
                                        folders.Add(item.FullName);                                // Добовляем найденую папку в список 'folders'
                                        ListDirectoryAndDeleteFile(item.FullName, ref folders);    // Получаем список папок и файлов внутри найденой папки
                                    }
                                }
                                else
                                {
                                    // Удаляем файл
                                    DeleteFile(item.FullName);
                                }
                            }
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                report.Base("ListDirectoryAndDeleteFile", path, ex.ToString());
            }
        }
        #endregion

        #region UploadFile
        /// <summary>
        /// Передать локальный файл на удаленый сервер
        /// </summary>
        /// <param name="LocalFile">Полный путь к локальному файлу</param>
        /// <param name="RemoteFile">Полный путь к удаленому файлу</param>
        /// <param name="EncryptionAES">Использовать шифрование AES 256</param>
        /// <param name="PasswdAES">Пароль для шифрования файлов</param>
        public bool UploadFile(string LocalFile, string RemoteFile, bool EncryptionAES, string PasswdAES, out long FileSizeToAES)
        {
            FileSizeToAES = -1;
            try
            {
                #region EncryptionAES = true
                if (EncryptionAES)
                {
                    using (var сryptoBox = new CryptoBox(PasswdAES, LocalFile))
                    {
                        using (var FileStream = сryptoBox.OpenRead(out string error))
                        {
                            if (FileStream == null)
                            {
                                report.UploadFile(LocalFile, RemoteFile, EncryptionAES, error);
                                return false;
                            }

                            // Новое расширение файла
                            FileSizeToAES = FileStream.Length;
                            string NewRemoteFile = Regex.Replace(RemoteFile, "([0-9]+)$", FileSizeToAES.ToString());

                            // Заливаем на нужный сервер
                            switch (typeSunc)
                            {
                                #region SFTP
                                case TypeSunc.SFTP:
                                    {
                                        sftp.UploadFile(FileStream, NewRemoteFile, true);
                                        return true;
                                    }
                                #endregion

                                #region OneDrive
                                case TypeSunc.OneDrive:
                                    {
                                        oneDrive.UploadFile(FileStream, Path.GetFileName(NewRemoteFile), Path.GetDirectoryName(NewRemoteFile)).Wait();
                                        return true;
                                    }
                                #endregion

                                #region FTP
                                case TypeSunc.FTP:
                                    {
                                        ftp.Upload(FileStream, NewRemoteFile, FtpExists.Overwrite, true);
                                        return true;
                                    }
                                #endregion

                                #region WebDav
                                case TypeSunc.WebDav:
                                    {
                                        var res = webDav.PutFile(NewRemoteFile, FileStream).Result;
                                        if (res.IsSuccessful)
                                            return true;

                                        report.UploadFile(LocalFile, NewRemoteFile, EncryptionAES, res);
                                        return false;
                                    }
                                #endregion
                            }
                        }
                    }
                }
                #endregion

                #region EncryptionAES = false
                else
                {
                    switch (typeSunc)
                    {
                        #region SFTP
                        case TypeSunc.SFTP:
                            {
                                using (var file = File.OpenRead(LocalFile))
                                {
                                    sftp.UploadFile(file, RemoteFile, true);
                                }

                                return true;
                            }
                        #endregion

                        #region FTP
                        case TypeSunc.FTP:
                            {
                                using (var LocalFileStream = File.OpenRead(LocalFile))
                                {
                                    ftp.Upload(LocalFileStream, RemoteFile, FtpExists.Overwrite, true);
                                }

                                return true;
                            }
                        #endregion

                        #region WebDav
                        case TypeSunc.WebDav:
                            {
                                using (var file = File.OpenRead(LocalFile))
                                {
                                    var res = webDav.PutFile(RemoteFile, file).Result;
                                    if (res.IsSuccessful)
                                        return true;

                                    report.UploadFile(LocalFile, RemoteFile, EncryptionAES, res);
                                    return false;
                                }
                            }
                        #endregion

                        #region OneDrive
                        case TypeSunc.OneDrive:
                            {
                                oneDrive.UploadFileAs(LocalFile, Path.GetFileName(RemoteFile), Path.GetDirectoryName(RemoteFile)).Wait();
                                return true;
                            }
                        #endregion
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                report.UploadFile(LocalFile, RemoteFile, EncryptionAES, ex.ToString());
            }

            return false;
        }
        #endregion

        #region DownloadFile
        /// <summary>
        /// Загрузить файл с удаленого сервера
        /// </summary>
        /// <param name="LocalFile">Полный путь к локальному файлу</param>
        /// <param name="RemoteFile">Полный путь к удаленому файлу</param>
        /// <param name="EncryptionAES">Использовать шифрование AES 256</param>
        /// <param name="PasswdAES">Пароль для шифрования файлов</param>
        /// <param name="FileSize">Размер файла</param>
        public bool DownloadFile(string LocalFile, string RemoteFile, bool EncryptionAES, string PasswdAES, long FileSize)
        {
            // Временный файл
            string tmpFile = $"{Folders.Temp.SyncBackup}/{md5.text(RemoteFile)}";

            // Путь к конечному файлу
            string targetFile = EncryptionAES ? tmpFile : LocalFile;

            try
            {
                #region Удаляем файлы для перезаписи
                if (File.Exists(LocalFile))
                    File.Delete(LocalFile);

                if (File.Exists(tmpFile))
                    File.Delete(tmpFile);
                #endregion

                #region Загружаем файл
                switch (typeSunc)
                {
                    case TypeSunc.SFTP:
                        {
                            using (FileStream LocalStream = new FileStream(targetFile, FileMode.Create, FileAccess.Write))
                            {
                                sftp.DownloadFile(RemoteFile, LocalStream);
                                break;
                            }
                        }

                    case TypeSunc.FTP:
                        {
                            ftp.DownloadFile(targetFile, RemoteFile, overwrite: true);
                            break;
                        }

                    case TypeSunc.OneDrive:
                        {
                            var item = oneDrive.GetItem(RemoteFile).Result;
                            if (item == null)
                            {
                                report.DownloadFile(targetFile, RemoteFile, EncryptionAES, FileSize, "OneDrive.GetItem(RemoteFile) == null");
                                return false;
                            }

                            oneDrive.DownloadItemAndSaveAs(item, targetFile).Wait();
                            break;
                        }

                    case TypeSunc.WebDav:
                        {
                            var res = webDav.GetRawFile(RemoteFile).Result;
                            if (!res.IsSuccessful)
                            {
                                report.DownloadFile(targetFile, RemoteFile, EncryptionAES, FileSize, (res.Description, res.StatusCode));
                                return false;
                            }

                            using (FileStream LocalStream = new FileStream(targetFile, FileMode.Create, FileAccess.Write))
                            {
                                CopyStream(res.Stream, LocalStream, FileSize);
                                break;
                            }
                        }
                }
                #endregion

                #region CryptoBox
                if (EncryptionAES)
                {
                    using (var сryptoBox = new CryptoBox(PasswdAES, LocalFile, _tmpFile: tmpFile))
                    {
                        if (!сryptoBox.Decrypt(out string error))
                        {
                            report.DownloadFile(LocalFile, RemoteFile, EncryptionAES, FileSize, error);
                            return false;
                        }
                    }
                }
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                report.DownloadFile(targetFile, RemoteFile, EncryptionAES, FileSize, ex.ToString());
            }

            return false;
        }
        #endregion

        #region CopyStream
        private void CopyStream(Stream inStream, Stream OutStream, long FileSize)
        {
            int read = 0;
            do
            {
                // Считываем поток
                byte[] buffer = new byte[FileSize > 81920 ? 81920 : FileSize];
                read = inStream.Read(buffer, 0, buffer.Length);
                OutStream.Write(buffer, 0, read);
                FileSize -= read;
            }
            while (read != 0 && FileSize > 0);
        }
        #endregion
    }
}
