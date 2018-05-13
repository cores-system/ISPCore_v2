using ISPCore.Models.RequestsFilter.Access;
using System;
using System.Runtime.CompilerServices;

namespace ISPCore.Models.Triggers.Events.RequestsFilter
{
    public class AccessIP
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(string IP, string host, DateTime expires, AccessType accessType)> OnAdd => (s) => Add?.Invoke(null, s);
        public static event EventHandler<ITuple> Add;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public static Action<(string IP, string host, AccessType accessType)> OnRemove => (s) => Remove?.Invoke(null, s);
        public static event EventHandler<ITuple> Remove;
    }
}
