using ISPCore.Models.Databases.Interface;
using System.Collections.Generic;
using ISPCore.Models.RequestsFilter.Templates.Rules;

namespace ISPCore.Models.RequestsFilter.Templates
{
    public class Template : IId
    {
        public int Id { get; set; }

        /// <summary>
        /// Имя шаблона
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Список правил
        /// </summary>
        public List<Rule> Rules { get; set; } = new List<Rule>();

        /// <summary>
        /// Список правил для замены ответа
        /// </summary>
        public List<RuleReplace> RuleReplaces { get; set; } = new List<RuleReplace>();

        /// <summary>
        /// Список правил для переопределения
        /// </summary>
        public List<RuleOverride> RuleOverrides { get; set; } = new List<RuleOverride>();

        /// <summary>
        /// Список допустимых аргументов в правиле
        /// </summary>
        public List<RuleArg> RuleArgs { get; set; } = new List<RuleArg>();
    }
}
