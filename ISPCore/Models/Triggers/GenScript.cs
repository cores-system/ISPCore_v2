using ISPCore.Engine;
using ISPCore.Engine.Base.Notification;
using ISPCore.Engine.Triggers;
using System.Collections.Generic;

namespace ISPCore.Models.Triggers
{
    public class GenScript
    {
        /// <summary>
        /// Данные события
        /// </summary>
        private Dictionary<string, object> data { get; set; } = new Dictionary<string, object>();

        #region GetValue
        /// <summary>
        ///  Получить значение
        /// </summary>
        /// <typeparam name="T">Тип данных</typeparam>
        /// <param name="key">Ключ</param>
        public T GetValue<T>(string key)
        {
            try
            {
                return (T)data[key];
            }
            catch { return default(T); }
        }
        #endregion

        #region SetValue
        /// <summary>
        /// Установить значение 
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="ob">Данные</param>
        public void SetValue(string key, object ob)
        {
            this.data[key] = ob;
        }
        #endregion


        /// <summary>
        /// Bash/API/Browser/etc
        /// </summary>
        public Invoke Invoke { get; set; } = new Invoke();

        /// <summary>
        /// Отправка уведомлений
        /// </summary>
        public SendTo SendTo { get; set; } = new SendTo();

        /// <summary>
        /// Доступ к IMemoryCache
        /// </summary>
        public TriggerCache memoryCache { get; set; } = new TriggerCache();
    }
}
