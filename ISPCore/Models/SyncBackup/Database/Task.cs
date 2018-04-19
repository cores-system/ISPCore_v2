using System;
using ISPCore.Models.Base;
using ISPCore.Models.Databases.Interface;
using ISPCore.Models.SyncBackup.Database.Enums;

namespace ISPCore.Models.SyncBackup.Database
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
        /// Тип базы 'MySQL/other'
        /// </summary>
        public TypeDb TypeDb { get; set; } = TypeDb.MySQL;

        /// <summary>
        /// Дата последней синхронизации
        /// </summary>
        public DateTime LastSync { get; set; }

        /// <summary>
        /// Пользовательское описание текущего задания
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Интервал синхронизации в минутах
        /// </summary>
        public int SuncTime
        {
            get { return 0 >= _suncTime ? 60 : _suncTime; }
            set { _suncTime = value; }
        }

        /// <summary>
        /// Настройки
        /// </summary>
        public DumpConf Conf { get; set; } = new DumpConf();

        /// <summary>
        /// Настройки для подключения к MySQL
        /// </summary>
        public MySQL MySQL { get; set; } = new MySQL();
    }
}
