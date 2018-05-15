using ISPCore.Models.RequestsFilter.Base.Enums;
using ISPCore.Models.RequestsFilter.Monitoring;
using System;

namespace ISPCore.Models.core.Cache.CheckLink
{
    public class ResponseView
    {
        /// <summary>
        /// Ошибка в правиле
        /// </summary>
        public bool IsErrorRule { get; set; }

        /// <summary>
        /// Время создания кеша
        /// </summary>
        public DateTime CacheTime { get; set; }

        /// <summary>
        /// Тип ответа
        /// </summary>
        public TypeRequest TypeResponse { get; set; }

        /// <summary>
        /// Тип ответа
        /// </summary>
        public ActionCheckLink ActionCheckLink { get; set; }

        #region Код ответа 303
        /// <summary>
        /// Код ответа 303
        /// </summary>
        public bool Is303 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Пользовательский код
        /// </summary>
        public string kode { get; set; }

        /// <summary>
        /// Куда отправить запрос
        /// </summary>
        public string ResponceUri { get; set; }
        #endregion
    }
}
