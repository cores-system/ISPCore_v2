using ISPCore.Models.Databases.Interface;

namespace ISPCore.Models.RequestsFilter.Base
{
    public class RuleArg : IRule
    {
        public int Id { get; set; }

        /// <summary>
        /// Имя правила
        /// </summary>
        public string Name { get; set; }

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
