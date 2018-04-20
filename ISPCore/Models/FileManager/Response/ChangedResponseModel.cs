using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ISPCore.Models.FileManager.Response
{
    [DataContract]
    internal class ChangedResponseModel
    {
        public ChangedResponseModel()
        {
            Changed = new List<FileModel>();
        }

        [DataMember(Name = "changed")]
        public List<FileModel> Changed { get; private set; }
    }
}
