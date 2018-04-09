using System;
using System.Collections.Concurrent;

namespace ISPCore.Models.core.Cache.CheckLink.Rules
{
    public class Rule
    {
        /// <summary>
        /// Список правил GET, с аргументами и без аргументов
        /// </summary>
        public string RuleGetToRegex { get; set; } = "^$";

        /// <summary>
        /// Список правил POST, без аргументов
        /// </summary>
        public string RulePostToRegex { get; set; } = "^$";

        /// <summary>
        /// Список правил POST, c аргументами GET, для быстрой проверки
        /// </summary>
        public string RuleArgsCheckPostToRegex { get; set; } = "^$";

        /// <summary>
        /// Список правил POST
        /// Проверяется если проверка 'RuleArgsCheckPostToRegex' положительная
        /// </summary>
        public ConcurrentBag<PostRule> postRules { get; set; } = new ConcurrentBag<PostRule>();
    }
}
