using System;
using System.Runtime.CompilerServices;

namespace ISPCore.Models.Triggers.Events.RequestsFilter
{
    public class Domain
    {
        /// <summary>
        /// Изменены настройки домена
        /// </summary>
        /// <param name="DomainId">Id домена</param>
        /// <param name="cat">Base/Aliases/LogSettings/Rules/av/AntiBot/LimitRequest</param>
        public static Action<(int DomainId, string cat)> OnChange => (s) => Change?.Invoke(null, s);
        public static event EventHandler<ITuple> Change;

        /// <summary>
        /// Создан домен
        /// </summary>
        /// <param name="DomainId">Id домена</param>
        public static Action<(int DomainId, int tmp2)> OnCreate => (s) => Create?.Invoke(null, s);
        public static event EventHandler<ITuple> Create;

        /// <summary>
        /// Удален домен
        /// </summary>
        /// <param name="DomainId">Id домена</param>
        public static Action<(int DomainId, int tmp2)> OnRemove => (s) => Remove?.Invoke(null, s);
        public static event EventHandler<ITuple> Remove;
    }
}
