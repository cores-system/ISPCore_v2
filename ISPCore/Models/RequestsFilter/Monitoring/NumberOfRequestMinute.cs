namespace ISPCore.Models.RequestsFilter.Monitoring
{
    public class NumberOfRequestMinute
    {
        /// <summary>
        /// 
        /// </summary>
        public int DomainID { get; set; }

        /// <summary>
        /// Всего запросов
        /// </summary>
        public ulong NumberOfRequest { get; set; }

        /// <summary>
        /// Запросов с ответом 303
        /// </summary>
        public ulong Count303 { get; set; }
    }
}
