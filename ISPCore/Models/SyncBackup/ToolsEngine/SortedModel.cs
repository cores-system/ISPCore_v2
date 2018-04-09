using System;

namespace ISPCore.Models.SyncBackup.ToolsEngine
{
    public class SortedModel : FileModel
    {
        /// <summary>
        /// Оригинальное время модификации файла
        /// </summary>
        public DateTime LocalLastWriteTime { get; set; }
    }
}
