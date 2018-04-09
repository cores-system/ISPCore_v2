using System;

namespace ISPCore.Models.RequestsFilter.Access
{
    public class AccessIPModel
    {
        /// <summary>
        /// IP адрес
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// Адрес сайта, для которого открыт доступ
        /// </summary>
        public string host { get; set; }

        /// <summary>
        /// До какого периода открыт доступ
        /// </summary>
        public DateTime Expires { get; set; }

        /// <summary>
        /// Режим доступа
        /// </summary>
        public AccessType accessType { get; set; }
    }
}
