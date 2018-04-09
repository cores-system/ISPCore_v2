namespace ISPCore.Models.SyncBackup.Tasks
{
    public class IgnoreFileOrFolders : Databases.Interface.IId
    {
        public int Id { get; set; }
        public int TaskId { get; set; }

        /// <summary>
        /// Путь к файлу или папке которую не нужно синхронизировать
        /// </summary>
        public string Patch { get; set; }
    }
}
