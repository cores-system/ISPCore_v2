using System;
using System.Runtime.CompilerServices;

namespace ISPCore.Models.Triggers.Events.Settings
{
    public class Service
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(int tmp1, int tmp2)> OnChange => (s) => Change?.Invoke(null, s);
        public static event EventHandler<ITuple> Change;
    }
}
