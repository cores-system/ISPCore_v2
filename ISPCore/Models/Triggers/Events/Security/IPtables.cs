using System;
using System.Runtime.CompilerServices;

namespace ISPCore.Models.Triggers.Events.Security
{
    public class IPtables
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(string IP, string BlockedHost, string Description, DateTime TimeExpires)> OnAddIPv4Or6 => (s) => AddIPv4Or6?.Invoke(null, s);
        public static event EventHandler<ITuple> AddIPv4Or6;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(string IP, string BlockedHost)> OnRemoveIPv4Or6 => (s) => RemoveIPv4Or6?.Invoke(null, s);
        public static event EventHandler<ITuple> RemoveIPv4Or6;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(string UserAgent, string Description, DateTime TimeExpires)> OnAddUserAgent => (s) => AddUserAgent?.Invoke(null, s);
        public static event EventHandler<ITuple> AddUserAgent;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(string oldUserAgentRegex, string newUserAgentRegex)> OnUpdateCacheToUserAgent => (s) => UpdateCacheToUserAgent?.Invoke(null, s);
        public static event EventHandler<ITuple> UpdateCacheToUserAgent;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BadTo">IP/User-Agent</param>
        public static Action<(string IpOrUserAgent, string BlockedHost, string BadTo)> OnReturn401 => (s) => Return401?.Invoke(null, s);
        public static event EventHandler<ITuple> Return401;
    }
}
