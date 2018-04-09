using System;

namespace ISPCore.Models.core.Cache.CheckLink.Common
{
    public class CacheValue
    {
        /// <summary>
        /// Текущее значение
        /// </summary>
        public int value { get; set; }

        /// <summary>
        /// Время жизни кеша
        /// </summary>
        public DateTime Expires { get; set; }
    }
}
