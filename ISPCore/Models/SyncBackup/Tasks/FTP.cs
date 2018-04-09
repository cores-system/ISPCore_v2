using ISPCore.Engine.Databases;
using ISPCore.Models.Databases.Interface;

namespace ISPCore.Models.SyncBackup.Tasks
{
    public class FTP : IUpdate
    {
        public int Id { get; set; }
        public int TaskId { get; set; }

        private int _activeConnections;

        /// <summary>
        /// Адрес сервера 'sftp/ftp/ftps'
        /// </summary>
        public string HostOrIP { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Passwd { get; set; }

        /// <summary>
        /// Порт удаленного сервера 'sftp/ftp/ftps'
        /// </summary>
        public int port { get; set; }

        /// <summary>
        /// Возвращает указаный порт или порт по умолчанию
        /// </summary>
        /// <param name="typeSunc">Тип синхронизации 'FTP/SFTP'</param>
        public int GetToPort(TypeSunc typeSunc)
        {
            if (port != 0)
                return port;

            return typeSunc == TypeSunc.FTP ? 21 : 22;
        }

        /// <summary>
        /// Количиство активных подключений к 'sftp/ftp/ftps'
        /// </summary>
        public int ActiveConnections
        {
            get { return _activeConnections > 0 ? _activeConnections > 10 ? 10 : _activeConnections : 1; }
            set { _activeConnections = value; }
        }

        /// <summary>
        /// Обновить поля
        /// </summary>
        /// <param name="item">Новые данные</param>
        void IUpdate.Update(dynamic item) => CommonModels.Update(this, item);
    }
}
