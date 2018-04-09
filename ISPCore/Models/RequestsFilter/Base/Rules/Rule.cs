using ISPCore.Models.Databases.Interface;

namespace ISPCore.Models.RequestsFilter.Base
{
    public class Rule : IRule
    {
        public int Id { get; set; }

        /// <summary>
        /// Статус правила
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Типа защиты
        /// </summary>
        public ActionCheckLink order { get; set; }

        /// <summary>
        /// Само правило (Regex)
        /// </summary>
        public string rule { get; set; }

        /// <summary>
        /// Для какого метода предназначено правило 
        /// </summary>
        public RequestMethod Method { get; set; }
    }
}
