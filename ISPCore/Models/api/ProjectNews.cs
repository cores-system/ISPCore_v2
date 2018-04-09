using System;

namespace ISPCore.Models.api
{
    public class ProjectNews
    {
        public int Id { get; set; }

        /// <summary>
        /// Категория (акции, новости и т.д)
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Описание ссылки
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Ссылка на новость
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Время записи
        /// </summary>
        public DateTime Time { get; set; }
    }
}
