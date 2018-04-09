namespace ISPCore.Models.SyncBackup.Report
{
    public class Rename
    {
        /// <summary>
        /// Полный путь к текущей папке
        /// </summary>
        public string oldPath { get; set; }

        /// <summary>
        /// Полный путь к новой папке
        /// </summary>
        public string newPath { get; set; }
    }
}
