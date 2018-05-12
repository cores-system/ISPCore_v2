using ISPCore.Models.RequestsFilter.Monitoring;
using System;
using System.Runtime.CompilerServices;

namespace ISPCore.Models.Triggers.Events.core
{
    public class CheckRequest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(string IP, string UserAgent, string Referer, int DomainID, string method, string host, string uri, string password, bool IsSuccess)> OnUnlock2FA => (s) => Unlock2FA?.Invoke(null, s);
        public static event EventHandler<ITuple> Unlock2FA;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(string IP, string UserAgent, string Referer, int DomainID, string method, string host, string uri, string FormData)> OnRequest => (s) => Request?.Invoke(null, s);
        public static event EventHandler<ITuple> Request;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(string IP, TypeRequest type, ulong CountRequest, string host, int DomainID)> OnRequestToMinute => (s) => RequestToMinute?.Invoke(null, s);
        public static event EventHandler<ITuple> RequestToMinute;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(string IP, string host, int DomainID)> OnIpToAccessHost => (s) => IpToAccessHost?.Invoke(null, s);
        public static event EventHandler<ITuple> IpToAccessHost;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(string IP, string host, int DomainID, string BadTo)> OnReturn401 => (s) => Return401?.Invoke(null, s);
        public static event EventHandler<ITuple> Return401;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(string IP, string UserAgent, string Referer, int DomainID, string method, string host, string uri, string FormData, int StatusCode, bool IsCache)> OnResponseView => (s) => ResponseView?.Invoke(null, s);
        public static event EventHandler<ITuple> ResponseView;
    }
}
