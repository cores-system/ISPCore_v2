using System;
using System.Runtime.CompilerServices;

namespace ISPCore.Models.Triggers.Events
{
    public class System
    {
        /// <summary>
        /// Ежесекундный крон
        /// </summary>
        /// <param name=""></param>
        public static Action<(int tmp1, int tmp2)> OnCronSeconds => (s) => CronSeconds?.Invoke(null, s);
        public static event EventHandler<ITuple> CronSeconds;

        /// <summary>
        /// Триггеры загружены
        /// </summary>
        /// <param name=""></param>
        public static Action<(int tmp1, int tmp2)> OnTriggersInitialize => (s) => TriggersInitialize?.Invoke(null, s);
        public static event EventHandler<ITuple> TriggersInitialize;
    }
}
