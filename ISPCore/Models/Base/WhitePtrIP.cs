using System;

namespace ISPCore.Models.Base
{
    public class WhitePtrIP
    {
        public int Id { get; set; }

        /// <summary>
        /// IP-адрес
        /// </summary>
        public string IPv4Or6 { get; set; }

        /// <summary>
        /// До какого времени действительна запись
        /// </summary>
        public DateTime Expires { get; set; }
    }
}
