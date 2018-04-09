using System;
using ISPCore.Models.Base;

namespace ISPCore.Models.RequestsFilter.Domains
{
    public class AntiVirus : Security.AntiVirus
    { 
        public int Id { get; set; }
        public int DomainId { get; set; }
        private int _CheckEveryToMinute;

        /// <summary>
        /// Задание Включено/Отключено/Ошибка
        /// </summary>
        public JobStatus JobStatus { get; set; } = JobStatus.off;

        /// <summary>
        /// Дата последней проверки
        /// </summary>
        public DateTime LastRun { get; set; }

        /// <summary>
        /// Интервал проверки в минутах
        /// </summary>
        public int CheckEveryToMinute
        {
            get { return _CheckEveryToMinute == 0 ? 2880 : _CheckEveryToMinute; }
            set { _CheckEveryToMinute = value; }
        }
    }
}
