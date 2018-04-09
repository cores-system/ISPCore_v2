using System;
using ISPCore.Models.Databases.Interface;

namespace ISPCore.Models.Base.Notification
{
    public class NotationBase : IId
    {
        public int Id { get; set; }

        /// <summary>
        /// Категория
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// Время записи
        /// </summary>
        public DateTime Time { get; set; }
    }
}
