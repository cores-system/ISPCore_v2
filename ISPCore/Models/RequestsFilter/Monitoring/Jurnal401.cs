using ISPCore.Models.Databases.Interface;
using System;

namespace ISPCore.Models.RequestsFilter.Monitoring
{
    public class Jurnal401 : JurnalBase
    {
        /// <summary>
        /// Причина блокировки
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// PTR запись - (для Антихостинг)
        /// </summary>
        public string Ptr { get; set; }
    }
}
