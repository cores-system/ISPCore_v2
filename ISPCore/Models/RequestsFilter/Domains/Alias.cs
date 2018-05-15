using ISPCore.Models.Databases.Interface;

namespace ISPCore.Models.RequestsFilter.Domains
{
    public class Alias : IId
    {
        public int Id { get; set; }

        /// <summary>
        /// Id домена
        /// </summary>
        public int DomainId { get; set; }

        /// <summary>
        /// Алиасы домена
        /// </summary>
        public string host { get; set; }

        /// <summary>
        /// Папка домена
        /// </summary>
        public string Folder { get; set; }
    }
}
