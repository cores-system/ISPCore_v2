using System;
using ISPCore.Models.Databases.Interface;

namespace ISPCore.Models.Base
{
    public class WhitePtrIP : IId
    {
        public int Id { get; set; }

        /// <summary>
        /// IP-адрес
        /// </summary>
        public string IPv4Or6 { get; set; }

        /// <summary>
        /// PTR-адрес
        /// </summary>
        public string PTR { get; set; }

        /// <summary>
        /// До какого времени действительна запись
        /// </summary>
        public DateTime Expires { get; set; }
    }
}
