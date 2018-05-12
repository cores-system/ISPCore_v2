using ISPCore.Models.Base;

namespace ISPCore.Models.RequestsFilter.Domains
{
    public class AntiBot : AntiBotBase
    {
        public int Id { get; set; }
        public int DomainId { get; set; }
        private string _hashKey;

        /// <summary>
        /// Использовать глобальные настройки AntiBot вместо локальных
        /// </summary>
        public bool UseGlobalConf { get; set; }

        /// <summary>
        /// Hash Key
        /// </summary>
        public string HashKey
        {
            get
            {
                if (_hashKey == null)
                    return string.Empty;

                return _hashKey;
            }
            set { _hashKey = value; }
        }

        /// <summary>
        /// Клонирование обьекта
        /// </summary>
        public AntiBot Clone()
        {
            return (AntiBot)this.MemberwiseClone();
        }
    }
}
