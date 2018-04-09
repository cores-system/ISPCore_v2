using System;
using Microsoft.Extensions.Caching.Memory;
using ISPCore.Models.Databases;
using ISPCore.Models.Base;

namespace ISPCore.Engine.Base.SqlAndCache
{
    public static class WhitePtr
    {
        static IMemoryCache memoryCache = Service.Get<IMemoryCache>();

        #region IsWhiteIP
        /// <summary>
        /// Проверка IP-адреса
        /// </summary>
        /// <param name="IPv4Or6">IP-адрес</param>
        public static bool IsWhiteIP(string IPv4Or6)
        {
            return memoryCache.TryGetValue(KeyToMemoryCache.WhitePtrIP(IPv4Or6), out _);
        }
        #endregion

        #region Add
        /// <summary>
        /// Добавить IP-адрес
        /// </summary>
        /// <param name="IPv4Or6">IP-адрес</param>
        /// <param name="Expires"></param>
        public static void Add(string IPv4Or6, DateTime Expires)
        {
            if (IsWhiteIP(IPv4Or6))
                return;

            // Добовляем IP в кеш
            memoryCache.Set(KeyToMemoryCache.WhitePtrIP(IPv4Or6), (byte)0, Expires);

            // Меняем режим работы с SQL
            SqlToMode.SetMode(SqlMode.Read);

            // Подключаемся к базе
            using (var coreDB = Service.Get<CoreDB>())
            {
                // Добовляем IP в базу
                coreDB.WhitePtrIPs.Add(new WhitePtrIP()
                {
                    IPv4Or6 = IPv4Or6,
                    Expires = Expires
                });

                // Сохраняем базу 
                coreDB.SaveChanges();
            }

            // Меняем режим работы с SQL
            SqlToMode.SetMode(SqlMode.ReadOrWrite);
        }
        #endregion
    }
}
