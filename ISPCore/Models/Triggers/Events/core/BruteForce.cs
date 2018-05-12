using System;
using System.Runtime.CompilerServices;

namespace ISPCore.Models.Triggers.Events.core
{
    public class BruteForce
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(string IP, int DomainID, string method, string host, string uri, string FormData, string keyName, int CountRequest)> OnIsLogin => (s) => IsLogin?.Invoke(null, s);
        public static event EventHandler<ITuple> IsLogin;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(string IP, string host, int DomainID, string Msg, DateTime Expires)> OnBlockedIP => (s) => BlockedIP?.Invoke(null, s);
        public static event EventHandler<ITuple> BlockedIP;
    }
}
