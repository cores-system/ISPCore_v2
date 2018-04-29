using ISPCore.Models.RequestsFilter.Base.Enums;
using ISPCore.Models.RequestsFilter.Monitoring;
using System;

namespace ISPCore.Models.core.Cache.CheckLink
{
    public class ResponseView
    {
        public bool IsErrorRule { get; set; }
        public DateTime CacheTime { get; set; }
        public TypeRequest TypeRequest { get; set; }
        public ActionCheckLink ActionCheckLink { get; set; }


        public bool Is303 { get; set; }
        public string ContentType { get; set; }
        public string kode { get; set; }
        public string ResponceUri { get; set; }
    }
}
