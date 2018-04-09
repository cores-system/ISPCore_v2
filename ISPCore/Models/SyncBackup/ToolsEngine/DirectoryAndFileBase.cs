using System;

namespace ISPCore.Models.SyncBackup.ToolsEngine
{
    public class DirectoryAndFileBase
    {
        /// <summary>
        /// Дата создания 'папки/файла' на удаленом сервере
        /// </summary>
        public DateTime RemoteCreated { get; set; }

        /// <summary>
        /// Дата модификации 'папки/файла' на удаленом сервере
        /// </summary>
        public DateTime RemoteLastModified { get; set; }
    }
}
