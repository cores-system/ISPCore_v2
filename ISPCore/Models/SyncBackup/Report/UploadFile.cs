namespace ISPCore.Models.SyncBackup.Report
{
    public class UploadFile
    {
        /// <summary>
        /// Полный путь к локальному файлу
        /// </summary>
        public string LocalFile { get; set; }

        /// <summary>
        /// Полный путь к удаленому файлу
        /// </summary>
        public string RemoteFile { get; set; }

        /// <summary>
        /// Использовать шифрование AES 256
        /// </summary>
        public bool EncryptionAES { get; set; }
    }
}
