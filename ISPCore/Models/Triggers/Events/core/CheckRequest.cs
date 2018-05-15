using ISPCore.Models.RequestsFilter.Monitoring;
using System;
using System.Runtime.CompilerServices;

namespace ISPCore.Models.Triggers.Events.core
{
    public class CheckRequest
    {
        /// <summary>
        /// Авторизация 2FA
        /// </summary>
        /// <param name="IP">IPv4/6</param>
        /// <param name="UserAgent">User-Agent</param>
        /// <param name="Referer">Referer</param>
        /// <param name="DomainID">Id домена</param>
        /// <param name="method">Метод запроса</param>
        /// <param name="host">Домен</param>
        /// <param name="uri">Url запроса</param>
        /// <param name="password">Переданный пароль</param>
        /// <param name="IsSuccess">Авторизация успешна</param>
        public static Action<(string IP, string UserAgent, string Referer, int DomainID, string method, string host, string uri, string password, bool IsSuccess)> OnUnlock2FA => (s) => Unlock2FA?.Invoke(null, s);
        public static event EventHandler<ITuple> Unlock2FA;

        /// <summary>
        /// Кеш домена
        /// </summary>
        /// <param name="DomainID">Id домена</param>
        /// <param name="IsCreate">Кеш создан</param>
        /// <param name="IsRemove">Кеш удален</param>
        public static Action<(int DomainID, bool IsCreate, bool IsRemove)> OnDomainCache => (s) => DomainCache?.Invoke(null, s);
        public static event EventHandler<ITuple> DomainCache;

        /// <summary>
        /// Выполнен запрос
        /// </summary>
        /// <param name="IP">IPv4/6</param>
        /// <param name="UserAgent">User-Agent</param>
        /// <param name="Referer">Referer</param>
        /// <param name="DomainID">Id домена</param>
        /// <param name="method">Метод запроса</param>
        /// <param name="host">Домен</param>
        /// <param name="uri">Url запроса</param>
        /// <param name="FormData">Данные POST запроса</param>
        public static Action<(string IP, string UserAgent, string Referer, int DomainID, string method, string host, string uri, string FormData)> OnRequest => (s) => Request?.Invoke(null, s);
        public static event EventHandler<ITuple> Request;

        /// <summary>
        /// Количество запросов за текущею минуту
        /// </summary>
        /// <param name="IP">IPv4/6</param>
        /// <param name="">All / _303</param>
        /// <param name="CountRequest">Количество запросов за текущею минуту</param>
        /// <param name="host">Домен</param>
        /// <param name="DomainID">Id домена</param>
        public static Action<(string IP, TypeRequest type, ulong CountRequest, string host, int DomainID)> OnRequestToMinute => (s) => RequestToMinute?.Invoke(null, s);
        public static event EventHandler<ITuple> RequestToMinute;

        /// <summary>
        /// Запрос с IP которым разрешен доступ к доменам в обход ISPCore
        /// </summary>
        /// <param name="IP">IPv4/6</param>
        /// <param name="host">Домен</param>
        /// <param name="DomainID">Id домена</param>
        public static Action<(string IP, string host, int DomainID)> OnIpToAccessHost => (s) => IpToAccessHost?.Invoke(null, s);
        public static event EventHandler<ITuple> IpToAccessHost;

        /// <summary>
        /// Запрос с заблокированного IP/UserAgent
        /// </summary>
        /// <param name="IP">IPv4/6</param>
        /// <param name="host">Домен</param>
        /// <param name="DomainID">Id домена</param>
        /// <param name="BadTo">IP/User-Agent</param>
        public static Action<(string IP, string host, int DomainID, string BadTo)> OnReturn401 => (s) => Return401?.Invoke(null, s);
        public static event EventHandler<ITuple> Return401;

        /// <summary>
        /// Выполнен ответ
        /// </summary>
        /// <param name="IP">IPv4/6</param>
        /// <param name="UserAgent">User-Agent</param>
        /// <param name="Referer">Referer</param>
        /// <param name="DomainID">Id домена</param>
        /// <param name="method">Метод запроса</param>
        /// <param name="host">Домен</param>
        /// <param name="uri">Url запроса</param>
        /// <param name="FormData">Данные POST запроса</param>
        /// <param name="StatusCode">Код ответа</param>
        /// <param name="IsCache">Ответ взят с кеша</param>
        public static Action<(string IP, string UserAgent, string Referer, int DomainID, string method, string host, string uri, string FormData, int StatusCode, bool IsCache)> OnResponseView => (s) => ResponseView?.Invoke(null, s);
        public static event EventHandler<ITuple> ResponseView;
    }
}
