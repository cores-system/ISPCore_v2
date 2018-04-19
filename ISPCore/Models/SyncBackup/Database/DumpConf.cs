using ISPCore.Engine.Databases;
using ISPCore.Models.Databases.Interface;
using ISPCore.Models.SyncBackup.Database.Enums;

namespace ISPCore.Models.SyncBackup.Database
{
    public class DumpConf : IUpdate
    {
        public int Id { get; set; }
        public int TaskId { get; set; }

        /// <summary>
        /// Список игнорируемых баз
        /// </summary>
        public string IgnoreDatabases { get; set; }

        /// <summary>
        /// Список баз для бекапа
        /// </summary>
        public string DumpDatabases { get; set; }

        /// <summary>
        /// Локальная дириктория
        /// </summary>
        public string Whence { get; set; }

        /// <summary>
        /// Использовать сжатие
        /// </summary>
        public CompressionType Compression { get; set; }

        /// <summary>
        /// Дописывать время бекапа
        /// </summary>
        public bool AddBackupTime { get; set; } = true;

        /// <summary>
        /// Обновить поля
        /// </summary>
        /// <param name="item">Новые данные</param>
        void IUpdate.Update(dynamic item) => CommonModels.Update(this, item);
    }
}
