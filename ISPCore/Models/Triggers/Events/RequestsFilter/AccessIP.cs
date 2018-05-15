using ISPCore.Models.RequestsFilter.Access;
using System;
using System.Runtime.CompilerServices;

namespace ISPCore.Models.Triggers.Events.RequestsFilter
{
    public class AccessIP
    {
        /// <summary>
        /// Добавлен доступ
        /// </summary>
        /// <param name="IP">IPv4/6</param>
        /// <param name="host">Домен</param>
        /// <param name="expires">На сколько открыт доступ</param>
        /// <param name="accessType">Тип доступа</param>
        public static Action<(string IP, string host, DateTime expires, AccessType accessType)> OnAdd => (s) => Add?.Invoke(null, s);
        public static event EventHandler<ITuple> Add;

        /// <summary>
        /// Удален доступ
        /// </summary>
        /// <param name="IP">IPv4/6</param>
        /// <param name="host">Домен</param>
        /// <param name="accessType">Тип доступа</param>
        public static Action<(string IP, string host, AccessType accessType)> OnRemove => (s) => Remove?.Invoke(null, s);
        public static event EventHandler<ITuple> Remove;
    }
}
