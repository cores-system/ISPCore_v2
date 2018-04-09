using System;

namespace ISPCore.Models.api
{
    public class ProjectChange
    {
        public int Id { get; set; }

        /// <summary>
        /// success  - Изменен или исправлен фунционал
        /// info     - Добавлен новый функционал
        /// primary  - Релиз 
        /// warning  - Исправлена ошибка в безопасности
        /// </summary>
        public ProjectChangeType Type { get; set; }

        /// <summary>
        /// Версия продукта
        /// </summary>
        public string vers { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// Время записи
        /// </summary>
        public DateTime Time { get; set; }
    }

    public enum ProjectChangeType
    {
        success = 0,   // Изменен или исправлен фунционал
        warning = 1,   // Исправлена ошибка
        info = 2,      // Добавлен новый функционал
        primary = 3    // Релиз
    }
}
