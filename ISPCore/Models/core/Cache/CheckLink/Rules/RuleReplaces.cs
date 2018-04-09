using System.Collections.Concurrent;

namespace ISPCore.Models.core.Cache.CheckLink.Rules
{
    public class RuleReplaces
    {
        /// <summary>
        /// Быстрая проверка GET запроса
        /// </summary>
        public string RuleGetToRegex { get; set; } = "^$";

        /// <summary>
        /// Быстрая проверка POST запроса
        /// </summary>
        public string RulePostToRegex { get; set; } = ".*";

        /// <summary>
        /// Список правил
        /// </summary>
        public ConcurrentBag<RuleReplace> Rules { get; set; } = new ConcurrentBag<RuleReplace>();
    }
}
