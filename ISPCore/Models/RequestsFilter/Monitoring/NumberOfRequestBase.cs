using System;

namespace ISPCore.Models.RequestsFilter.Monitoring
{
    public class NumberOfRequestBase
    {
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Time { get; set; }

        public long Count200 { get; set; }
        public long Count303 { get; set; }
        public long Count403 { get; set; }
        public long Count401 { get; set; }
        public long Count500 { get; set; }
        public long Count2FA { get; set; }
    }
}
