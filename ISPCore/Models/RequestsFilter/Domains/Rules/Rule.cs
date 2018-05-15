namespace ISPCore.Models.RequestsFilter.Domains.Rules
{
    public class Rule : Base.Rule
    {
        /// <summary>
        /// Id домена к которому принадлежит правило
        /// </summary>
        public int DomainId { get; set; }
    }
}
