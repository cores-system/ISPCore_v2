using System;
using System.Runtime.CompilerServices;

namespace ISPCore.Models.Triggers.Events.Settings
{
    public class Base
    {
        /// <summary>
        /// Изменены настройки
        /// </summary>
        /// <param name=""></param>
        public static Action<(int tmp1, int tmp2)> OnChange => (s) => Change?.Invoke(null, s);
        public static event EventHandler<ITuple> Change;

        /// <summary>
        /// Изменена соль
        /// </summary>
        /// <param name="oldSalt">Прошлое значение</param>
        /// <param name="newSalt">Новое значение</param>
        public static Action<(string oldSalt, string newSalt)> OnChangeSalt => (s) => ChangeSalt?.Invoke(null, s);
        public static event EventHandler<ITuple> ChangeSalt;

        /// <summary>
        /// Изменен пароль Root
        /// </summary>
        /// <param name="oldPasswd">Прошлое значение</param>
        /// <param name="newPasswd">Новое значение</param>
        public static Action<(string oldPasswd, string newPasswd)> OnChangePasswdRoot => (s) => ChangePasswdRoot?.Invoke(null, s);
        public static event EventHandler<ITuple> ChangePasswdRoot;

        /// <summary>
        /// Изменен пароль 2FA
        /// </summary>
        /// <param name="oldPasswd">Прошлое значение</param>
        /// <param name="newPasswd">Новое значение</param>
        public static Action<(string oldPasswd, string newPasswd)> OnChangePasswd2FA => (s) => ChangePasswd2FA?.Invoke(null, s);
        public static event EventHandler<ITuple> ChangePasswd2FA;
    }
}
