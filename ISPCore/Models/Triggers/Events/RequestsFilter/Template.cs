using System;
using System.Runtime.CompilerServices;

namespace ISPCore.Models.Triggers.Events.RequestsFilter
{
    public class Template
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(int Id, int tmp2)> OnChange => (s) => Change?.Invoke(null, s);
        public static event EventHandler<ITuple> Change;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(int Id, int tmp2)> OnCreate => (s) => Create?.Invoke(null, s);
        public static event EventHandler<ITuple> Create;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(int Id, int tmp2)> OnRemove => (s) => Remove?.Invoke(null, s);
        public static event EventHandler<ITuple> Remove;
    }
}
