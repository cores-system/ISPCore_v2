using System;
using System.Runtime.CompilerServices;

namespace ISPCore.Models.Triggers.Events.core
{
    public class LimitRequest
    {
        /// <summary>
        /// Добавлен IP в системный список
        /// </summary>
        /// <param name="IP">IPv4/6</param>
        /// <param name="host">Домен</param>
        /// <param name="DomainID">Id домена</param>
        /// <param name="PtrHostName">PTR запись</param>
        /// <param name="AddDays">Количество дней</param>
        public static Action<(string IP, string host, int DomainID, string PtrHostName, int AddDays)> OnAddToWhitePtr => (s) => AddToWhitePtr?.Invoke(null, s);
        public static event EventHandler<ITuple> AddToWhitePtr;

        /// <summary>
        /// Заблокирован IP
        /// </summary>
        /// <param name="IP">IPv4/6</param>
        /// <param name="PtrHostName">PTR запись</param>
        /// <param name="host">Домен</param>
        /// <param name="DomainID">Id домена</param>
        /// <param name="Msg">Причина блокировки</param>
        /// <param name="Expires">Время блокировки</param>
        public static Action<(string IP, string PtrHostName, string host, int DomainID, string Msg, DateTime Expires)> OnBlockedIP => (s) => BlockedIP?.Invoke(null, s);
        public static event EventHandler<ITuple> BlockedIP;

        /// <summary>
        /// Авторизация reCAPTCHA
        /// </summary>
        /// <param name="IsVerify">Авторизация успешна</param>
        /// <param name="IP">IPv4/6</param>
        /// <param name="Host">Домен</param>
        /// <param name="ExpiresToMinute">На сколько минут авторизован</param>
        public static Action<(bool IsVerify, string IP, string Host, int ExpiresToMinute)> OnRecaptchaVerify => (s) => RecaptchaVerify?.Invoke(null, s);
        public static event EventHandler<ITuple> RecaptchaVerify;

        /// <summary>
        /// Проверка авторизации
        /// </summary>
        /// <param name="IsVerify">Пользователь авторизован</param>
        /// <param name="IP">IPv4/6</param>
        /// <param name="Host">Домен</param>
        /// <param name="DomainID">Id домена</param>
        /// <param name="countRequest">Количество запросов</param>
        /// <param name="ExpiresToMinute">На сколько минут авторизован</param>
        public static Action<(bool IsVerify, string IP, string Host, int DomainID, int countRequest, int ExpiresToMinute)> OnRecaptchaView => (s) => RecaptchaView?.Invoke(null, s);
        public static event EventHandler<ITuple> RecaptchaView;

        /// <summary>
        /// Выполнен запрос
        /// </summary>
        /// <param name="IP">IPv4/6</param>
        /// <param name="Host">Домен</param>
        /// <param name="DomainID">Id домена</param>
        /// <param name="keyName">Minute/Hour/Day</param>
        /// <param name="countRequest">Количество запросов</param>
        public static Action<(string IP, string host, int DomainID, string keyName, int CountRequest)> OnRequest => (s) => Request?.Invoke(null, s);
        public static event EventHandler<ITuple> Request;
    }
}
