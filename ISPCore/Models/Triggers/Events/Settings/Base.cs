using System;
using System.Runtime.CompilerServices;

namespace ISPCore.Models.Triggers.Events.Settings
{
    public class Base
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
        public static Action<(string oldSalt, string newSalt)> OnChangeSalt => (s) => ChangeSalt?.Invoke(null, s);
        public static event EventHandler<ITuple> ChangeSalt;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(string oldPasswd, string newPasswd)> OnChangePasswdRoot => (s) => ChangePasswdRoot?.Invoke(null, s);
        public static event EventHandler<ITuple> ChangePasswdRoot;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(string oldPasswd, string newPasswd)> OnChangePasswd2FA => (s) => ChangePasswd2FA?.Invoke(null, s);
        public static event EventHandler<ITuple> ChangePasswd2FA;
    }
}
