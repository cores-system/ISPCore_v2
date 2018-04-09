namespace ISPCore.Models.RequestsFilter.Domains
{
    public class LimitRequest
    {
        public int Id { get; set; }
        public int DomainId { get; set; }

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
