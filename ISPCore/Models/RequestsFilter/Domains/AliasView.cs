namespace ISPCore.Models.RequestsFilter.Domains
{
    public class AliasView : Alias
    {
        /// <summary>
        /// Количество запросов за минуту
        /// </summary>
        public ulong ReqToMinute { get; set; }

        /// <summary>
        /// Количество запросов за минуту
        /// </summary>
        public string ReqMinuteToString => string.Format("{0:N0}", ReqToMinute);
    }
}
