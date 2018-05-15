using System;
using System.Runtime.CompilerServices;

namespace ISPCore.Models.Triggers.Events.core
{
    public class BruteForce
    {
        /// <summary>
        /// Авторизация
        /// </summary>
        /// <param name="IP">IPv4/6</param>
        /// <param name="DomainID">Id домена</param>
        /// <param name="method">Метод запроса</param>
        /// <param name="host">Домен</param>
        /// <param name="uri">url запроса</param>
        /// <param name="FormData">Данные POST запроса</param>
        /// <param name="keyName">Minute/Hour/Day</param>
        /// <param name="count">Количество авторизаций</param>
        public static Action<(string IP, int DomainID, string method, string host, string uri, string FormData, string keyName, int count)> OnIsLogin => (s) => IsLogin?.Invoke(null, s);
        public static event EventHandler<ITuple> IsLogin;

        /// <summary>
        /// Блокировка IP
        /// </summary>
        /// <param name="IP">IPv4/6</param>
        /// <param name="host">Домен</param>
        /// <param name="DomainID">Id домена</param>
        /// <param name="Msg">Причина блокировки</param>
        /// <param name="Expires">Время блокировки</param>
        public static Action<(string IP, string host, int DomainID, string Msg, DateTime Expires)> OnBlockedIP => (s) => BlockedIP?.Invoke(null, s);
        public static event EventHandler<ITuple> BlockedIP;
    }
}
