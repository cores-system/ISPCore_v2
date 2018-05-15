namespace ISPCore.Models.Databases.json
{
    public class Base
    {
        private string _CoreAPI;
        private int _CountParallel;

        /// <summary>
        /// Адрес к Core API
        /// </summary>
        public string CoreAPI
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_CoreAPI))
                    return "/core";

                return System.Text.RegularExpressions.Regex.Replace(_CoreAPI.Trim(), "/+$", "");
            }
            set { _CoreAPI = value; }
        }

        /// <summary>
        /// Количиство потоков в Parallel.ForEach
        /// </summary>
        public int CountParallel
        {
            get
            {
                if (_CountParallel < 1)
                    return 1;

                return _CountParallel;
            }
            set { _CountParallel = value; }
        }

        /// <summary>
        /// Автоматическое обновление ISPCore
        /// </summary>
        public bool AutoUpdate { get; set; } = true;

        /// <summary>
        /// Режим дебага
        /// </summary>
        public bool DebugEnabled { get; set; }

        /// <summary>
        /// Глобально останавлиывает запись любых логов
        /// </summary>
        public bool DisableWriteLog { get; set; }

        /// <summary>
        /// Показывать заглушку для доменов которые не закреплены
        /// /core/check/link
        /// </summary>
        public bool EnableToDomainNotFound { get; set; } = true;

        /// <summary>
        /// Авторизация 2FA
        /// </summary>
        public bool EnableTo2FA { get; set; }

        /// <summary>
        /// Количиство новых уведомлений
        /// </summary>
        public int CountNotification { get; set; }
    }
}
