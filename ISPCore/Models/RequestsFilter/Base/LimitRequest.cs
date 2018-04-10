using ISPCore.Models.RequestsFilter.Base.Enums;

namespace ISPCore.Models.RequestsFilter.Base
{
    public class LimitRequest
    {
        /// <summary>
        /// Метод блокировки при достижении лимита запросов
        /// </summary>
        public LimitToBlockType BlockType { get; set; } = LimitToBlockType._403;

        /// <summary>
        /// Количество запросов перед повторной проверкой reCAPTCHA
        /// </summary>
        public int MaxRequestToAgainСheckingreCAPTCHA { get; set; } = 300;

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
