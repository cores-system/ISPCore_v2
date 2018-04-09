using System;

namespace ISPCore.Models.Base
{
    public class JurnalBase
    {
        public int Id { get; set; }

        /// <summary>
        /// IP адрес пользователя
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// Страна
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Город
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Регион
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Время записи
        /// </summary>
        public DateTime Time { get; set; }
    }
}
