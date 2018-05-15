using System;
using System.Runtime.CompilerServices;

namespace ISPCore.Models.Triggers.Events
{
    public class Auth
    {
        /// <summary>
        /// Авторизация
        /// </summary>
        /// <param name="IP">IPv4/6</param>
        /// <param name="IsSuccess">Успешная авторизация</param>
        public static Action<(string IP, bool IsSuccess)> OnUnlock => (s) => Unlock?.Invoke(null, s);
        public static event EventHandler<ITuple> Unlock;

        /// <summary>
        /// Авторизация 2FA
        /// </summary>
        /// <param name="IP">IPv4/6</param>
        /// <param name="IsSuccess">Успешная авторизация</param>
        public static Action<(string IP, bool IsSuccess)> OnTwoFacAuth => (s) => TwoFacAuth?.Invoke(null, s);
        public static event EventHandler<ITuple> TwoFacAuth;

        /// <summary>
        /// Выход
        /// </summary>
        /// <param name="IP">IPv4/6</param>
        public static Action<(string IP, int tmp)> OnSignOut => (s) => SignOut?.Invoke(null, s);
        public static event EventHandler<ITuple> SignOut;
    }
}
