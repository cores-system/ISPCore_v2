namespace ISPCore.Models.core.Cache.CheckLink
{
    public class LimitRequest : Models.RequestsFilter.Base.LimitRequest
    {
        /// <summary>
        /// Включен/Выключен - режим лимитирования запросов
        /// </summary>
        public bool IsEnabled { get; set; }
    }
}
