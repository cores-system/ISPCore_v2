using System.Runtime.Serialization;

namespace ISPCore.Models.FileManager
{
    [DataContract]
    internal class RootModel : BaseModel
    {
        //[DataMember(Name = "volumeId")]
        [DataMember(Name = "volumeid")] // https://github.com/Studio-42/elFinder/issues/2403
        public string VolumeId { get; set; }

        [DataMember(Name = "dirs")]
        public byte Dirs { get; set; }
    }
}
