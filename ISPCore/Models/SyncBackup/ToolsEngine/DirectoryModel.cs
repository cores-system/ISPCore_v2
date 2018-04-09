namespace ISPCore.Models.SyncBackup.ToolsEngine
{
    public class DirectoryModel : DirectoryAndFileBase
    {
        /// <summary>
        /// Полный путь к папке на удаленом сервере
        /// </summary>
        public string Folder { get; set; }
    }
}
