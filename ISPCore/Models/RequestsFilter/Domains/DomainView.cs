namespace ISPCore.Models.RequestsFilter.Domains
{
    public class DomainView
    {
        public int Id { get; set; }

        /// <summary>
        /// Количество запросов за минуту
        /// </summary>
        public ulong ReqToMinute { get; set; }

        /// <summary>
        /// Количество запросов в секунду
        /// </summary>
        public int ReqToSecond
        {
            get
            {
                if (ReqToMinute == 0)
                    return 0;

                var res = (int)(ReqToMinute / 60);
                if (res > 0)
                    return res;

                return 0;
            }
        }

        /// <summary>
        /// Основной домен (без www)
        /// </summary>
        public string host { get; set; }

        /// <summary>
        /// Статус защиты
        /// </summary>
        public Protection Protect { get; set; }

        /// <summary>
        /// Имена шаблонов с дополнительными правилами
        /// </summary>
        public string Templates { get; set; }
    }
}
