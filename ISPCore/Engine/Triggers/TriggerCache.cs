using System;
using Microsoft.Extensions.Caching.Memory;

namespace ISPCore.Engine.Triggers
{
    public class TriggerCache
    {
        #region TriggerCache
        IMemoryCache memoryCache;

        public TriggerCache()
        {
            memoryCache = Service.Get<IMemoryCache>();
        }
        #endregion

        #region Get
        /// <summary>
        /// Получить значение
        /// </summary>
        /// <typeparam name="T">Тип данных</typeparam>
        /// <param name="key">Ключ</param>
        public T Get<T>(string key)
        {
            if (memoryCache.TryGetValue(key, out T res))
                return res;

            return default(T);
        }
        #endregion

        #region Set
        /// <summary>
        /// Установить значение 
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="ob">Данные</param>
        /// <param name="seconds">Сколько секунд хранить кеш</param>
        public void Set(string key, object ob, int seconds)
        {
            memoryCache.Set(key, ob, TimeSpan.FromSeconds(seconds));
        }
        #endregion

        #region Contains
        /// <summary>
        /// Проверить значение 
        /// </summary>
        /// <param name="key">Ключ</param>
        public bool Contains(string key)
        {
            return memoryCache.TryGetValue(key, out _);
        }
        #endregion
    }
}
