using ISPCore.Models.RequestsFilter.Base.Enums;
using System;
using System.Runtime.CompilerServices;

namespace ISPCore.Models.Triggers.Events.core
{
    public class AntiBot
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(string IP, string host, int DomainID, bool IsValid, string verification)> OnValidCookie => (s) => ValidCookie?.Invoke(null, s);
        public static event EventHandler<ITuple> ValidCookie;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(string IP, string host, string cookie, string verification, int ExpiresToHour)> OnSetValidCookie => (s) => SetValidCookie?.Invoke(null, s);
        public static event EventHandler<ITuple> SetValidCookie;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(bool IsVerify, string IP, string Host, int ExpiresToHour)> OnRecaptchaVerify => (s) => RecaptchaVerify?.Invoke(null, s);
        public static event EventHandler<ITuple> RecaptchaVerify;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(bool IsVerify, string IP, string Host)> OnCheckCookie => (s) => CheckCookie?.Invoke(null, s);
        public static event EventHandler<ITuple> CheckCookie;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(string IP, string host, int DomainID, string PtrHostName, int AddHours)> OnAddToWhitePtr => (s) => AddToWhitePtr?.Invoke(null, s);
        public static event EventHandler<ITuple> AddToWhitePtr;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(string IP, string PtrHostName, string host, int DomainID, string Msg, int AddMinutes)> OnBlockedIP => (s) => BlockedIP?.Invoke(null, s);
        public static event EventHandler<ITuple> BlockedIP;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(string IP, string host, int DomainID, AntiBotType antiBotType)> OnResponseView => (s) => ResponseView?.Invoke(null, s);
        public static event EventHandler<ITuple> ResponseView;
    }
}
