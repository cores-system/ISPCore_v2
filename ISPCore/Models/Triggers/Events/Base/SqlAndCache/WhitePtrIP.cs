using System;
using System.Runtime.CompilerServices;

namespace ISPCore.Models.Triggers.Events.Base.SqlAndCache
{
    public class WhitePtrIP
    {
        /// <summary>
        /// Добавлен IP
        /// </summary>
        /// <param name="IPv4Or6">IPv4/6</param>
        /// <param name="PTR">PTR запись</param>
        /// <param name="Expires">На какое время</param>
        public static Action<(string IPv4Or6, string PTR, DateTime Expires)> OnAdd => (s) => Add?.Invoke(null, s);
        public static event EventHandler<ITuple> Add;

        /// <summary>
        /// Удален IP
        /// </summary>
        /// <param name="IPv4Or6">IPv4/6</param>
        /// <param name="PTR">PTR запись</param>
        public static Action<(string IPv4Or6, string PTR)> OnRemove => (s) => Remove?.Invoke(null, s);
        public static event EventHandler<ITuple> Remove;
    }
}
