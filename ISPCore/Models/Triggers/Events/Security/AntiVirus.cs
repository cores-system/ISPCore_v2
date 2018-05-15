using System;
using System.Runtime.CompilerServices;

namespace ISPCore.Models.Triggers.Events.Security
{
    public class AntiVirus
    {
        /// <summary>
        /// Изменены настройки антивируса
        /// </summary>
        /// <param name=""></param>
        public static Action<(int tmp1, int tmp2)> OnChange => (s) => Change?.Invoke(null, s);
        public static event EventHandler<ITuple> Change;

        /// <summary>
        /// Началось выполнение задания
        /// </summary>
        /// <param name="progress_id">Id задания</param>
        /// <param name="report">Расположение отчета html</param>
        public static Action<(string progress_id, string report)> OnStart => (s) => Start?.Invoke(null, s);
        public static event EventHandler<ITuple> Start;

        /// <summary>
        /// Задание выполнено
        /// </summary>
        /// <param name="progress_id">Id задания</param>
        /// <param name="report">Расположение отчета html</param>
        public static Action<(string progress_id, string report)> OnStop => (s) => Stop?.Invoke(null, s);
        public static event EventHandler<ITuple> Stop;
    }
}
