using ISPCore.Models.SyncBackup.Database.Enums;
using System;
using System.Runtime.CompilerServices;

namespace ISPCore.Models.Triggers.Events.SyncBackup
{
    public class Database
    {
        /// <summary>
        /// Задание изменено 
        /// </summary>
        /// <param name="TaskId">Id задания</param>
        public static Action<(int TaskId, int tmp2)> OnChange => (s) => Change?.Invoke(null, s);
        public static event EventHandler<ITuple> Change;

        /// <summary>
        /// Создано задание 
        /// </summary>
        /// <param name="TaskId">Id задания</param>
        public static Action<(int TaskId, int tmp2)> OnCreate => (s) => Create?.Invoke(null, s);
        public static event EventHandler<ITuple> Create;

        /// <summary>
        /// Задание удалено 
        /// </summary>
        /// <param name="TaskId">Id задания</param>
        public static Action<(int TaskId, int tmp2)> OnRemove => (s) => Remove?.Invoke(null, s);
        public static event EventHandler<ITuple> Remove;

        /// <summary>
        /// Задание выполняется
        /// </summary>
        /// <param name="TaskId">Id задания</param>
        /// <param name="typeDb">Экспортируемая база</param>
        public static Action<(int TaskId, TypeDb typeDb)> OnStartJob => (s) => StartJob?.Invoke(null, s);
        public static event EventHandler<ITuple> StartJob;

        /// <summary>
        /// Выполнение задания завершено 
        /// </summary>
        /// <param name="TaskId">Id задания</param>
        /// <param name="typeDb">Экспортируемая база</param>
        /// <param name="IsOk">Задание выполнено без ошибок</param>
        /// <param name="ErrorMsg">Текст ошибки</param>
        public static Action<(int TaskId, TypeDb typeDb, bool IsOk, string ErrorMsg)> OnStopJob => (s) => StopJob?.Invoke(null, s);
        public static event EventHandler<ITuple> StopJob;
    }
}
