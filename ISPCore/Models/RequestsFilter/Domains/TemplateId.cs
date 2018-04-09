namespace ISPCore.Models.RequestsFilter.Domains
{
    public class TemplateId : Databases.Interface.IId
    {
        public int Id { get; set; }
        public int DomainId { get; set; }

        public int Template { get; set; }
    }
}
