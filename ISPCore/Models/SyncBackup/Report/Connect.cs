using ISPCore.Models.SyncBackup.Tasks;

namespace ISPCore.Models.SyncBackup.Report
{
    public class Connect
    {
        /// <summary>
        /// Тип синхронизации
        /// </summary>
        public TypeSunc typeSunc { get; set; }

        /// <summary>
        /// Конфигурация удаленого сервера 'ftp/sftp'
        /// </summary>
        public FTP ftpConf { get; set; }

        /// <summary>
        /// Конфигурация удаленого сервера 'WebDav'
        /// </summary>
        public Models.SyncBackup.Tasks.WebDav webDavConf { get; set; }

        /// <summary>
        /// Конфигурация удаленого сервера 'OneDrive'
        /// </summary>
        public OneDrive oneDriveConf { get; set; }
    }
}
