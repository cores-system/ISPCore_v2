namespace ISPCore.Models.core.Cache.CheckLink.Rules
{
    public class PostRule
    {
        /// <summary>
        /// Список проверяемых POST аргументов
        /// </summary>
        public string RulePostToRegex  { get; set; } = "^$";

        /// <summary>
        /// Правило с аргументами GET
        /// </summary>
        public string rule { get; set; }
    }
}
