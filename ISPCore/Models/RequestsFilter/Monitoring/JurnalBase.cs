using ISPCore.Models.Databases.Interface;

namespace ISPCore.Models.RequestsFilter.Monitoring
{
    public class JurnalBase :  Models.Base.JurnalBase, ITime
    {
        /// <summary>
        /// Имя сайта
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Метод запроса - "POST/GET"
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// url запроса, "/admin.php"
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// Данные POST запроса (не используется)
        /// </summary>
        public string FormData { get; set; }

        /// <summary>
        /// UserAgent пользователя
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// Реффер пользователя
        /// </summary>
        public string Referer { get; set; }
    }
}
