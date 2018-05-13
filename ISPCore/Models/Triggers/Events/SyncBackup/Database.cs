using ISPCore.Models.SyncBackup.Database.Enums;
using System;
using System.Runtime.CompilerServices;

namespace ISPCore.Models.Triggers.Events.SyncBackup
{
    public class Database
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(int TaskId, int tmp2)> OnChange => (s) => Change?.Invoke(null, s);
        public static event EventHandler<ITuple> Change;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(int TaskId, int tmp2)> OnCreate => (s) => Create?.Invoke(null, s);
        public static event EventHandler<ITuple> Create;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(int TaskId, int tmp2)> OnRemove => (s) => Remove?.Invoke(null, s);
        public static event EventHandler<ITuple> Remove;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(int TaskId, TypeDb typeDb)> OnStartJob => (s) => StartJob?.Invoke(null, s);
        public static event EventHandler<ITuple> StartJob;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(int TaskId, TypeDb typeDb, bool IsOk, string ErrorMsg)> OnStopJob => (s) => StopJob?.Invoke(null, s);
        public static event EventHandler<ITuple> StopJob;
    }
}
