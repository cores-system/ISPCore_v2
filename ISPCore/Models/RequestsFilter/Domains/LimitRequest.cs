namespace ISPCore.Models.RequestsFilter.Domains
{
    public class LimitRequest : Base.LimitRequest
    {
        public int Id { get; set; }

        /// <summary>
        /// Id домена
        /// </summary>
        public int DomainId { get; set; }
    }
}
