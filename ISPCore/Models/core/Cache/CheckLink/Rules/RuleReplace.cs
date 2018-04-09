namespace ISPCore.Models.core.Cache.CheckLink.Rules
{
    public class RuleReplace : Models.RequestsFilter.Base.Rules.RuleReplace
    {
        /// <summary>
        /// GET аргументы
        /// </summary>
        public string GetArgsToRegex { get; set; } = "^$";

        /// <summary>
        /// POST аргументы
        /// </summary>
        public string PostArgsToRegex { get; set; } = "^$";
    }
}
