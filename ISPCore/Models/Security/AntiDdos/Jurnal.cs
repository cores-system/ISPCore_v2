using ISPCore.Models.Base;

namespace ISPCore.Models.Security.AntiDdos
{
    public class Jurnal : JurnalBase
    {
        /// <summary>
        /// PTR запись
        /// </summary>
        public string HostName { get; set; }
    }
}
