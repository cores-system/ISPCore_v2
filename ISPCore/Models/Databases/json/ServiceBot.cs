namespace ISPCore.Models.Databases.json
{
    public class ServiceBot
    {
        public TelegramBot Telegram { get; set; } = new TelegramBot();
        public EmailBot Email { get; set; } = new EmailBot();
        public SmsBot SMS { get; set; } = new SmsBot();
    }

    public class TelegramBot
    {
        /// <summary>
        /// Токен
        /// </summary>
        public string Token { get; set; }
    }

    public class EmailBot
    {
        /// <summary>
        /// Адрес почтового сервера
        /// </summary>
        public string ConnectUrl { get; set; }

        /// <summary>
        /// Порт почтового сервера
        /// </summary>
        public int ConnectPort { get; set; }

        /// <summary>
        /// Безопасное подключение
        /// </summary>
        public bool useSsl { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Passwd { get; set; }
    }

    public class SmsBot
    {
        /// <summary>
        /// https://smspilot.ru/my-settings.php?tab=acc
        /// </summary>
        public string apikey { get; set; }
    }
}
