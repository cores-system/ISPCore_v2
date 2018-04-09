using ISPCore.Models.Base;

namespace ISPCore.Models.RequestsFilter.Domains
{
    public class AntiBot : AntiBotBase
    {
        public int Id { get; set; }
        public int DomainId { get; set; }

        /// <summary>
        /// Использовать глобальные настройки AntiBot вместо локальных
        /// </summary>
        public bool UseGlobalConf { get; set; }

        /// <summary>
        /// Клонирование обьекта
        /// </summary>
        public AntiBot Clone()
        {
            return (AntiBot)this.MemberwiseClone();
        }
    }
}
