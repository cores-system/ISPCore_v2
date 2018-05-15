using System;
using System.Runtime.CompilerServices;

namespace ISPCore.Models.Triggers.Events.RequestsFilter
{
    public class Template
    {
        /// <summary>
        /// Изменены настройки шаблона
        /// </summary>
        /// <param name="Id">Id шаблона</param>
        public static Action<(int Id, int tmp2)> OnChange => (s) => Change?.Invoke(null, s);
        public static event EventHandler<ITuple> Change;

        /// <summary>
        /// Создан шаблон
        /// </summary>
        /// <param name="Id">Id шаблона</param>
        public static Action<(int Id, int tmp2)> OnCreate => (s) => Create?.Invoke(null, s);
        public static event EventHandler<ITuple> Create;

        /// <summary>
        /// Удален шаблон
        /// </summary>
        /// <param name="Id">Id шаблона</param>
        public static Action<(int Id, int tmp2)> OnRemove => (s) => Remove?.Invoke(null, s);
        public static event EventHandler<ITuple> Remove;
    }
}
