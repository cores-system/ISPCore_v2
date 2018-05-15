using System;

namespace ISPCore.Models.RequestsFilter.Monitoring
{
    public class NumberOfRequestBase
    {
        public int Id { get; set; }

        /// <summary>
        /// Время создания записи
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Количество ответов 200
        /// </summary>
        public long Count200 { get; set; }

        /// <summary>
        /// Количество ответов 303
        /// </summary>
        public long Count303 { get; set; }

        /// <summary>
        /// Количество ответов 403
        /// </summary>
        public long Count403 { get; set; }

        /// <summary>
        /// Количество блокировок
        /// </summary>
        public long Count401 { get; set; }

        /// <summary>
        /// Количество ошибок
        /// </summary>
        public long Count500 { get; set; }

        /// <summary>
        /// Количество авторизаций
        /// </summary>
        public long Count2FA { get; set; }

        /// <summary>
        /// Количество ответов 401 от IPtables
        /// </summary>
        public long CountIPtables { get; set; }
    }
}
