namespace ISPCore.Models.SyncBackup.ToolsEngine
{
    public class FileModel : DirectoryAndFileBase
    {
        /// <summary>
        /// Размер файла на удаленом серверер
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// Имя файла на удаленом сервере
        /// </summary>
        public string Name { get; set; }
    }
}
