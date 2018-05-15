using ISPCore.Models.Base;
using ISPCore.Models.core.Cache.CheckLink;
using System;

namespace ISPCore.Models.Databases.json
{
    public class AntiBot : AntiBotBase
    {
        /// <summary>
        /// Включить/Выключить
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Список доменов где можно вывести капчу если "type = reCAPTCHA"
        /// </summary>
        public string DomainsToreCAPTCHA { get; set; }

        /// <summary>
        /// Настройки лимитирования запросов
        /// </summary>
        public LimitRequest limitRequest { get; set; } = new LimitRequest();

        /// <summary>
        /// Время обновления настроек
        /// </summary>
        public DateTime LastUpdateToConf { get; set; } = DateTime.Now;

        /// <summary>
        /// Клонирование обьекта
        /// </summary>
        public AntiBot Clone()
        {
            return (AntiBot)this.MemberwiseClone();
        }
    }
}
