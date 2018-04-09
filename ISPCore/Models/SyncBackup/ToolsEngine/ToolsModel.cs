using ISPCore.Engine.SyncBackup;
using System.Collections.Generic;

namespace ISPCore.Models.SyncBackup.ToolsEngine
{
    public class ToolsModel
    {
        /// <summary>
        /// Максимальное количиство потоков
        /// </summary>
        public int ActiveConnections { get; set; }

        /// <summary>
        /// Удаленный сервер - SDK
        /// </summary>
        public RemoteServer serv { get; set; }

        /// <summary>
        /// Папка на удаленном сервере - (полный путь /home/s1/
        /// </summary>
        public string RemoteFolder { get; set; }

        /// <summary>
        /// Папка на локальном сервере - (полный путь /home/s1/)
        /// </summary>
        public string LocalFolder { get; set; }

        /// <summary>
        /// Список папок с ошибкой синхронизации - (обновляет список)
        /// </summary>
        public List<string> NewListErrorLocalFolders = new List<string>();
    }
}
