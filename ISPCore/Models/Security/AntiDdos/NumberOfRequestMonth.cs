using System;

namespace ISPCore.Models.Security.AntiDdos
{
    public class NumberOfRequestMonth
    {
        public int Id { get; set; }

        /// <summary>
        /// Время создания записи
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Количество блокировок
        /// </summary>
        public int CountBlocked { get; set; }

        /// <summary>
        /// Максимальное значение TCP/UPD 
        /// </summary>
        public long value { get; set; }
    }
}
