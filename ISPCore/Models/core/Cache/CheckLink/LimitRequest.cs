namespace ISPCore.Models.core.Cache.CheckLink
{
    public class LimitRequest
    {
        /// <summary>
        /// Включен/Выключен - режим лимитирования запросов
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Использовать глобальные настройки лимита вместо локальных
        /// </summary>
        public bool UseGlobalConf { get; set; }

        /// <summary>
        /// Максимальное количиство запросов в минуту 
        /// </summary>
        public int MinuteLimit { get; set; }

        /// <summary>
        /// Максимальное количиство запросов за час 
        /// </summary>
        public int HourLimit { get; set; }

        /// <summary>
        /// Максимальное количиство запросов за сутки 
        /// </summary>
        public int DayLimit { get; set; }
    }
}
