using System;
using System.Runtime.CompilerServices;

namespace ISPCore.Models.Triggers.Events.Base.SqlAndCache
{
    public class WhitePtrIP
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(string IPv4Or6, string PTR, DateTime Expires)> OnAdd => (s) => Add?.Invoke(null, s);
        public static event EventHandler<ITuple> Add;

        // <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(string IPv4Or6, string PTR)> OnRemove => (s) => Remove?.Invoke(null, s);
        public static event EventHandler<ITuple> Remove;
    }
}
