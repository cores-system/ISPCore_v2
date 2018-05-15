namespace ISPCore.Models.RequestsFilter.Domains.Rules
{
    public class RuleReplace : Base.Rules.RuleReplace
    {
        /// <summary>
        /// Id домена к которому принадлежит правило
        /// </summary>
        public int DomainId { get; set; }
    }
}
