namespace ISPCore.Models.RequestsFilter.Domains
{
    public class LimitRequest : Base.LimitRequest
    {
        public int Id { get; set; }
        public int DomainId { get; set; }
    }
}
