using System;
using ISPCore.Models.Base;
using System.Collections.Generic;
using ISPCore.Models.Databases.Interface;

namespace ISPCore.Models.SyncBackup.Tasks
{
    public class Task : IId
    {
        public int Id { get; set; }

        private int _suncTime;

        /// <summary>
        /// Задание Включено/Отключено/Ошибка
        /// </summary>
        public JobStatus JobStatus { get; set; } = JobStatus.on;

        /// <summary>
        /// Использовать шифрование AES256
        /// </summary>
        public bool EncryptionAES { get; set; } = true;

        /// <summary>
        /// Пароль шифрования для AES256
        /// </summary>
        public string PasswdAES { get; set; }

        /// <summary>
        /// Тип синхронизации 'WebDav/FTP/SFTP'
        /// </summary>
        public TypeSunc TypeSunc { get; set; } = TypeSunc.FTP;

        /// <summary>
        /// Дата последней синхронизации
        /// </summary>
        public DateTime LastSync { get; set; }

        /// <summary>
        /// Для поиска папок которые изменились с момента последней синхронизации
        /// </summary>
        public DateTime CacheSync { get; set; }

        /// <summary>
        /// Время жизни кеша
        /// </summary>
        public DateTime CacheExpires { get; set; }

        /// <summary>
        /// Пользовательское описание текущего задания
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Локальная дириктория
        /// </summary>
        public string Whence { get; set; }

        /// <summary>
        /// Удаленный каталог
        /// </summary>
        public string Where { get; set; }

        /// <summary>
        /// Количиство активных бекапов .sync
        ///  0 - Неограничено
        ///  1 - Хранятся только актуальные файлы .ok
        /// >1 - Указаное количество
        /// </summary>
        public int CountActiveBackup { get; set; }

        /// <summary>
        /// Максимальное количиство дней которое хранить активный бекап
        ///  0 - Неограничено
        /// >0 - Указаное количество
        /// </summary>
        public int CountActiveDayBackup { get; set; }

        /// <summary>
        /// Интервал синхронизации в минутах
        /// </summary>
        public int SuncTime
        {
            get { return _suncTime == 0 ? 60 : _suncTime; }
            set { _suncTime = value; }
        }

        /// <summary>
        /// Список игнорируемых файлов и папок
        /// </summary>
        public List<IgnoreFileOrFolders> IgnoreFileOrFolders { get; set; } = new List<IgnoreFileOrFolders>();

        /// <summary>
        /// Настройки для FTP/SFTP
        /// </summary>
        public FTP FTP { get; set; } = new FTP();

        /// <summary>
        /// Настройки для WebDav
        /// </summary>
        public WebDav WebDav { get; set; } = new WebDav();

        /// <summary>
        /// Настройки для OneDrive
        /// </summary>
        public OneDrive OneDrive { get; set; } = new OneDrive();
    }
}
