using ISPCore.Models.RequestsFilter.Base.Enums;

namespace ISPCore.Models.Base
{
    public class AntiBotBase
    {
        int _hourCacheToBot, _hourCacheToUser, _waitUser, _countBackgroundRequest, _backgroundHourCacheToIP;
        string _backgroundCheckToAddExtensions;

        /// <summary>
        /// Тип проверки запросов
        /// </summary>
        public AntiBotType type { get; set; } = AntiBotType.Off;

        /// <summary>
        /// Режим проверки бота
        /// true  - Сначала пропустить бота, потом проверить
        /// false - Сначала проверить бота, потом пропустить
        /// </summary>
        public bool FirstSkipToBot { get; set; } = true;

        /// <summary>
        /// Редиректить пользователя на оригинальный домен при использовании аноминайзера
        /// </summary>
        public bool RewriteToOriginalDomain { get; set; } = true;

        /// <summary>
        /// На сколько часов кешировать IP бота
        /// </summary>
        public int HourCacheToBot
        {
            get
            {
                if (0 >= _hourCacheToBot)
                    return 216; // 9 дней

                return _hourCacheToBot;
            }
            set { _hourCacheToBot = value; }
        }

        /// <summary>
        /// На сколько часов выставлять валидные Cookie пользователю
        /// Cookie привязаны в IP пользователя
        /// </summary>
        public int HourCacheToUser
        {
            get
            {
                if (0 >= _hourCacheToUser)
                    return 12;

                return _hourCacheToUser;
            }
            set { _hourCacheToUser = value; }
        }

        /// <summary>
        /// Сколько милисекунд ждать перед установкой куков
        /// </summary>
        public int WaitUser
        {
            get
            {
                if (0 >= _waitUser)
                    return 2800;

                return _waitUser;
            }
            set { _waitUser = value; }
        }

        /// <summary>
        /// Добовляет на страницу пользовательский код
        /// </summary>
        public string AddCodeToHtml { get; set; } = string.Empty;

        #region Фоновая проверка
        /// <summary>
        /// Выполнять проверку пользователей в фоновом режиме
        /// </summary>
        public bool BackgroundCheck { get; set; }

        /// <summary>
        /// Количиство запросов в фоновом режиме
        /// Миниму: 2
        /// </summary>
        public int CountBackgroundRequest
        {
            get
            {
                if (2 >= _countBackgroundRequest)
                    return 2;

                return _countBackgroundRequest;
            }
            set { _countBackgroundRequest = value; }
        }

        /// <summary>
        /// Список дополнительных расширений для проверки
        /// </summary>
        public string BackgroundCheckToAddExtensions
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_backgroundCheckToAddExtensions))
                    return string.Empty;

                return _backgroundCheckToAddExtensions;
            }
            set { _backgroundCheckToAddExtensions = value; }
        }

        /// <summary>
        /// На сколько часов кешировать IP
        /// </summary>
        public int BackgroundHourCacheToIP
        {
            get
            {
                if (0 >= _backgroundHourCacheToIP)
                    return 36;

                return _backgroundHourCacheToIP;
            }
            set { _backgroundHourCacheToIP = value; }
        }
        #endregion
    }
}
