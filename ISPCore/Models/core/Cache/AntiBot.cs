namespace ISPCore.Models.core.Cache
{
    public class AntiBotCacheToGlobalConf
    {
        /// <summary>
        /// Список доменов на которых можно вывести reCAPTCHA
        /// </summary>
        public string DomainsToreCaptchaRegex { get; set; } = "^$";

        /// <summary>
        /// Оригинальные настройки
        /// </summary>
        public Models.Databases.json.AntiBot conf { get; set; }
    }
}
