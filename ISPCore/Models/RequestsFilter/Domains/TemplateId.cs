namespace ISPCore.Models.RequestsFilter.Domains
{
    public class TemplateId : Databases.Interface.IId
    {
        public int Id { get; set; }

        /// <summary>
        /// Id домена
        /// </summary>
        public int DomainId { get; set; }

        /// <summary>
        /// Id шаблона
        /// </summary>
        public int Template { get; set; }
    }
}
