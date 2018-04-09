using System;

namespace ISPCore.Models.Security
{
    public class IPtables
    {
        public IPtables() { }
        public IPtables(string _Description, DateTime _TimeExpires)
        {
            Description = _Description;
            TimeExpires = _TimeExpires;
        }

        /// <summary>
        /// Причина блокировки
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// До какого времени заблокирован IP
        /// </summary>
        public DateTime TimeExpires { get; set; }
    }
}
