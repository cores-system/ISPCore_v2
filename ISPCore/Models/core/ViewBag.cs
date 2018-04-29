using System;

namespace ISPCore.Models.core
{
    public class ViewBag
    {
        public bool DebugEnabled { get; set; }

        public bool IsErrorRule { get; set; }

        public string IP { get; set; }
        public string uri { get; set; }
        public string jsonDomain { get; set; }
        public string antiBotToGlobalConf { get; set; }
        public string FormData { get; set; }
        public string method { get; set; }
        public string host { get; set; }
        public string Referer { get; set; }
        public string UserAgent { get; set; }

        public string CoreAPI { get; set; }
        public string ErrorTitleException { get; set; }
        public string ErrorRuleException { get; set; }

        public bool IsCacheView { get; set; }
        public DateTime CreateCacheView { get; set; }
    }
}
