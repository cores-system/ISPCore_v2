namespace ISPCore.Models.RequestsFilter.Domains.Rules
{
    public class RuleArg : Base.RuleArg
    {
        /// <summary>
        /// Id домена к которому принадлежит правило
        /// </summary>
        public int DomainId { get; set; }
    }
}
