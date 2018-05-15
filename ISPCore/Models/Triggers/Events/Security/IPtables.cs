using System;
using System.Runtime.CompilerServices;

namespace ISPCore.Models.Triggers.Events.Security
{
    public class IPtables
    {
        /// <summary>
        /// Заблокирован IP
        /// </summary>
        /// <param name="IP">IPv4/6</param>
        /// <param name="BlockedHost">Домен - (если не указан, значит блокировка глобальная)</param>
        /// <param name="Description">Причина блокировки</param>
        /// <param name="TimeExpires">Время блокировки</param>
        public static Action<(string IP, string BlockedHost, string Description, DateTime TimeExpires)> OnAddIPv4Or6 => (s) => AddIPv4Or6?.Invoke(null, s);
        public static event EventHandler<ITuple> AddIPv4Or6;

        /// <summary>
        /// Разблокирован IP
        /// </summary>
        /// <param name="IP">IPv4/6</param>
        /// <param name="BlockedHost">Домен - (если не указан, значит IP удален из глобального списка)</param>
        public static Action<(string IP, string BlockedHost)> OnRemoveIPv4Or6 => (s) => RemoveIPv4Or6?.Invoke(null, s);
        public static event EventHandler<ITuple> RemoveIPv4Or6;

        /// <summary>
        /// Заблокирован User-Agent
        /// </summary>
        /// <param name="UserAgent">User-Agent</param>
        /// <param name="Description">Причина блокировки</param>
        /// <param name="TimeExpires">Время блокировки</param>
        public static Action<(string UserAgent, string Description, DateTime TimeExpires)> OnAddUserAgent => (s) => AddUserAgent?.Invoke(null, s);
        public static event EventHandler<ITuple> AddUserAgent;

        /// <summary>
        /// Обновлен кеш блокировки по User-Agent
        /// </summary>
        /// <param name="oldUserAgentRegex">Прошлое значение</param>
        /// <param name="newUserAgentRegex">Новое значение</param>
        public static Action<(string oldUserAgentRegex, string newUserAgentRegex)> OnUpdateCacheToUserAgent => (s) => UpdateCacheToUserAgent?.Invoke(null, s);
        public static event EventHandler<ITuple> UpdateCacheToUserAgent;

        /// <summary>
        /// Запрос от заблокированного IP/User-Agent
        /// </summary>
        /// <param name="IpOrUserAgent">IP/User-Agent</param>
        /// <param name="BlockedHost">Домен</param>
        /// <param name="BadTo">IP/User-Agent</param>
        public static Action<(string IpOrUserAgent, string BlockedHost, string BadTo)> OnReturn401 => (s) => Return401?.Invoke(null, s);
        public static event EventHandler<ITuple> Return401;
    }
}
