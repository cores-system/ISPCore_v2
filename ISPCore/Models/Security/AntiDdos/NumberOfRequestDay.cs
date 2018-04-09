using System;

namespace ISPCore.Models.Security.AntiDdos
{
    public class NumberOfRequestDay
    {
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int CountBlocked { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long value { get; set; }
    }
}
