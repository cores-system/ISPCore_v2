using System;
using System.Runtime.CompilerServices;

namespace ISPCore.Models.Triggers.Events
{
    public class Auth
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(string IP, bool IsSuccess)> OnUnlock => (s) => Unlock?.Invoke(null, s);
        public static event EventHandler<ITuple> Unlock;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(string IP, bool IsSuccess)> OnTwoFacAuth => (s) => TwoFacAuth?.Invoke(null, s);
        public static event EventHandler<ITuple> TwoFacAuth;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(string IP, int tmp)> OnSignOut => (s) => SignOut?.Invoke(null, s);
        public static event EventHandler<ITuple> SignOut;
    }
}
