using System;
using System.Runtime.CompilerServices;

namespace ISPCore.Models.Triggers.Events.core
{
    public class LimitRequest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(string IP, string host, int DomainID, string PtrHostName, int AddDays)> OnAddToWhitePtr => (s) => AddToWhitePtr?.Invoke(null, s);
        public static event EventHandler<ITuple> AddToWhitePtr;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(string IP, string PtrHostName, string host, int DomainID, string Msg, DateTime Expires)> OnBlockedIP => (s) => BlockedIP?.Invoke(null, s);
        public static event EventHandler<ITuple> BlockedIP;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(bool IsVerify, string IP, string Host, int ExpiresToMinute)> OnRecaptchaVerify => (s) => RecaptchaVerify?.Invoke(null, s);
        public static event EventHandler<ITuple> RecaptchaVerify;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(bool IsVerify, string IP, string Host, int DomainID, int countRequest, int ExpiresToMinute)> OnRecaptchaView => (s) => RecaptchaView?.Invoke(null, s);
        public static event EventHandler<ITuple> RecaptchaView;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(string IP, string host, int DomainID, string keyName, int CountRequest)> OnRequest => (s) => Request?.Invoke(null, s);
        public static event EventHandler<ITuple> Request;
    }
}
