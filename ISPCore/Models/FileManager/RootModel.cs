using System.Runtime.Serialization;

namespace ISPCore.Models.FileManager
{
    [DataContract]
    internal class RootModel : BaseModel
    {
        [DataMember(Name = "volumeid")]
        public string VolumeId { get; set; }

        [DataMember(Name = "dirs")]
        public byte Dirs { get; set; }
    }
}
