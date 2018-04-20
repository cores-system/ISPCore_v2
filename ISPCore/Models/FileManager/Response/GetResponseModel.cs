using System.Runtime.Serialization;

namespace ISPCore.Models.FileManager.Response
{
    [DataContract]
    internal class GetResponseModel
    {
        [DataMember(Name = "content")]
        public string Content { get; set; }

        [DataMember(Name = "encoding")]
        public string Encoding { get; set; } = "utf-8";
    }
}
