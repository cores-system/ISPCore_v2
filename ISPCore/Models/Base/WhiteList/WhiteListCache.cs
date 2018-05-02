using System;

namespace ISPCore.Models.Base.WhiteList
{
    public class WhiteListCache
    {
        /// <summary>
        /// Белый список PTR/Regex
        /// </summary>
        public string PtrRegex = "^$";

        /// <summary>
        /// Белый список IP/Regex
        /// </summary>
        //public string IpRegex = "^$";

        /// <summary>
        /// Белый список UserAgent/Regex
        /// </summary>
        public string UserAgentRegex = "^$";

        /// <summary>
        /// Время обновления настроек
        /// </summary>
        public DateTime LastUpdateToConf { get; set; } = DateTime.Now;
    }
}
