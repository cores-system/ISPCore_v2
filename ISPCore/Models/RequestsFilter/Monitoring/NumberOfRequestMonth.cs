namespace ISPCore.Models.RequestsFilter.Monitoring
{
    public class NumberOfRequestMonth : NumberOfRequestBase
    {
        /// <summary>
        /// Общее количество запросов
        /// </summary>
        public long allRequests { get; set; }
    }
}
