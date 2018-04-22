using System;
using ISPCore.Models.SyncBackup.Report;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using ISPCore.Engine.Base;
using ISPCore.Models.SyncBackup.Tasks;
using ISPCore.Models.SyncBackup.Operation;

namespace ISPCore.Engine.SyncBackup
{
    public class Report
    {
        #region Report/SaveAndDispose
        Task task;
        List<BaseItem> db = new List<BaseItem>();
        public Report(Task _task)
        {
            task = _task;
        }

        public void SaveAndDispose(ref List<More> ResponseNameAndValue)
        {
            try
            {
                if (db.Count > 0)
                {
                    // Сохраняем отчет на диск
                    string FileName = $"tk-{task.Id}_{DateTime.Now.ToString("dd-MM-yyy_HH-mm")}-{Generate.Passwd(6)}.json";
                    string json = JsonConvert.SerializeObject(new Models.SyncBackup.Report.Base(task, db), Formatting.Indented);
                    string HideFiled = Regex.Replace(json, "(\"(Login|Passwd|PasswdAES|RefreshToken)\":) +\"[^\n\r]+\"(,)?", "$1 \"Скрытое поле\"$3", RegexOptions.IgnoreCase);
                    File.WriteAllText($"{Folders.ReportSync}/{FileName}", HideFiled, Encoding.UTF8);

                    // Чистим базу
                    db.Clear();
                    db = null;

                    // Заполняем ResponseNameAndValue
                    ResponseNameAndValue.Add(new More()
                    {
                        Name = "Ошибки",
                        Value = $"<a href='/reports/sync/{FileName}' target='_blank'>{FileName}</a>"
                    });
                }
            }
            catch { }
        }
        #endregion

        #region Connect
        /// <summary>
        /// Ошибка подключения
        /// </summary>
        /// <param name="_typeSunc">Тип синхронизации</param>
        /// <param name="_ftpConf">Конфигурация удаленого сервера 'ftp/sftp'</param>
        /// <param name="_webDavConf">Конфигурация удаленого сервера 'WebDav'</param>
        /// <param name="_response">Ответ клиента или ошибка try</param>
        public void Connect(TypeSunc _typeSunc, FTP _ftpConf, Models.SyncBackup.Tasks.WebDav _webDavConf, OneDrive oneDriveConf,  object _response)
        {
            db.Add(new BaseItem()
            {
                MethodName = "Connect",
                Response = _response,
                ArgNameAndValue = new Connect()
                {
                    ftpConf = _ftpConf,
                    typeSunc = _typeSunc,
                    webDavConf = _webDavConf,
                    oneDriveConf = oneDriveConf
                }
            });
        }
        #endregion

        #region Base
        /// <param name="_methodName">Имя метода</param>
        /// <param name="_ArgNameAndValue">Данные аргумента</param>
        /// <param name="_response">Ответ клиента или ошибка try</param>
        public void Base(string _methodName, string _ArgNameAndValue, object _response)
        {
            db.Add(new BaseItem()
            {
                MethodName = _methodName,
                ArgNameAndValue = _ArgNameAndValue,
                Response = _response
            });
        }
        #endregion

        #region Rename
        /// <summary>
        /// Ошибка при переименовании папки
        /// </summary>
        /// <param name="_oldPath">Полный путь к текущей папке</param>
        /// <param name="_newPath">Полный путь к новой папке</param>
        /// <param name="_response">Ответ клиента или ошибка try</param>
        public void Rename(string _oldPath, string _newPath, object _response)
        {
            db.Add(new BaseItem()
            {
                MethodName = "Rename",
                Response = _response,
                ArgNameAndValue = new Rename()
                {
                    oldPath = _oldPath,
                    newPath = _newPath
                }
            });
        }
        #endregion

        #region UploadFile
        /// <summary>
        /// Ошибка при загрузке файла на удаленый сервер
        /// </summary>
        /// <param name="_LocalFile">Полный путь к локальному файлу</param>
        /// <param name="_RemoteFile">Полный путь к удаленому файлу</param>
        /// <param name="_EncryptionAES">Использовать шифрование AES 256</param>
        /// <param name="_response">Ответ клиента или ошибка try</param>
        public void UploadFile(string _LocalFile, string _RemoteFile, bool _EncryptionAES, object _response)
        {
            db.Add(new BaseItem()
            {
                MethodName = "UploadFile",
                Response = _response,
                ArgNameAndValue = new UploadFile()
                {
                    LocalFile = _LocalFile,
                    RemoteFile = _RemoteFile,
                    EncryptionAES = _EncryptionAES
                }
            });
        }
        #endregion

        #region DownloadFile
        /// <summary>
        /// Ошибка при загрузке файла
        /// </summary>
        /// <param name="_LocalFile">Полный путь к локальному файлу</param>
        /// <param name="_RemoteFile">Полный путь к удаленому файлу</param>
        /// <param name="_EncryptionAES">Использовать шифрование AES 256</param>
        /// <param name="_FileSize">Размер файла</param>
        /// <param name="_response">Ответ клиента или ошибка try</param>
        public void DownloadFile(string _LocalFile, string _RemoteFile, bool _EncryptionAES, long _FileSize, object _response)
        {
            db.Add(new BaseItem()
            {
                MethodName = "DownloadFile",
                Response = _response,
                ArgNameAndValue = new DownloadFile()
                {
                    LocalFile = _LocalFile,
                    RemoteFile = _RemoteFile,
                    EncryptionAES = _EncryptionAES,
                    FileSize = _FileSize
                }
            });
        }
        #endregion
    }
}
