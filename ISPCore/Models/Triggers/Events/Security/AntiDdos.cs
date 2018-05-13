using System;
using System.Runtime.CompilerServices;

namespace ISPCore.Models.Triggers.Events.Security
{
    public class AntiDdos
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(int tmp1, int tmp2)> OnChange => (s) => Change?.Invoke(null, s);
        public static event EventHandler<ITuple> Change;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(int count, long MaxTcpOrUpd)> OnCountTcpOrUpd => (s) => CountTcpOrUpd?.Invoke(null, s);
        public static event EventHandler<ITuple> CountTcpOrUpd;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(string IP, string PtrHostName, int AddDays)> OnAddToWhitePtr => (s) => AddToWhitePtr?.Invoke(null, s);
        public static event EventHandler<ITuple> AddToWhitePtr;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(string IP, string PtrHostName, int BlockingMinute)> OnBlockedIP => (s) => BlockedIP?.Invoke(null, s);
        public static event EventHandler<ITuple> BlockedIP;
    }
}
