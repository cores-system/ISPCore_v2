using System.Runtime.Serialization;

namespace ISPCore.Models.FileManager
{
    [DataContract]
    internal class FileModel : BaseModel
    {
        /// <summary>
        ///  Hash of parent directory. Required except roots dirs.
        /// </summary>
        [DataMember(Name = "phash")]
        public string ParentHash { get; set; }
    }
}
