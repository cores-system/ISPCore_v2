using ISPCore.Models.RequestsFilter.Base.Enums;
using System;
using System.Runtime.CompilerServices;

namespace ISPCore.Models.Triggers.Events.core
{
    public class AntiBot
    {
        /// <summary>
        /// Проверка Cookie
        /// </summary>
        /// <param name="IP">IPv4/6</param>
        /// <param name="host">Домен</param>
        /// <param name="DomainID">Id домена</param>
        /// <param name="IsValid">Cookie валидны</param>
        /// <param name="verification">Проверка пройдена в "reCAPTCHA/SignalR/js"</param>
        public static Action<(string IP, string host, int DomainID, bool IsValid, string verification)> OnValidCookie => (s) => ValidCookie?.Invoke(null, s);
        public static event EventHandler<ITuple> ValidCookie;

        /// <summary>
        /// Переданы валидные Cookie
        /// </summary>
        /// <param name="IP">IPv4/6</param>
        /// <param name="host">Домен</param>
        /// <param name="cookie">Валидные Cookie</param>
        /// <param name="verification">reCAPTCHA/SignalR/js</param>
        /// <param name="ExpiresToHour">Сколько часов валидны Cookie</param>
        public static Action<(string IP, string host, string cookie, string verification, int ExpiresToHour)> OnSetValidCookie => (s) => SetValidCookie?.Invoke(null, s);
        public static event EventHandler<ITuple> SetValidCookie;

        /// <summary>
        /// Проверка reCAPTCHA
        /// </summary>
        /// <param name="IsVerify">Проверка пройдена</param>
        /// <param name="IP">IPv4/6</param>
        /// <param name="host">Домен</param>
        public static Action<(bool IsVerify, string IP, string Host)> OnRecaptchaVerify => (s) => RecaptchaVerify?.Invoke(null, s);
        public static event EventHandler<ITuple> RecaptchaVerify;

        /// <summary>
        /// Проверка Cookie
        /// </summary>
        /// <param name="IsVerify">Проверка пройдена</param>
        /// <param name="IP">IPv4/6</param>
        /// <param name="host">Домен</param>
        public static Action<(bool IsVerify, string IP, string Host)> OnCheckCookie => (s) => CheckCookie?.Invoke(null, s);
        public static event EventHandler<ITuple> CheckCookie;

        /// <summary>
        /// Добавлен IP в системный список
        /// </summary>
        /// <param name="IP">IPv4/6</param>
        /// <param name="host">Домен</param>
        /// <param name="DomainID">Id домена</param>
        /// <param name="PtrHostName">PTR запись</param>
        /// <param name="AddHours">Количество часов</param>
        public static Action<(string IP, string host, int DomainID, string PtrHostName, int AddHours)> OnAddToWhitePtr => (s) => AddToWhitePtr?.Invoke(null, s);
        public static event EventHandler<ITuple> AddToWhitePtr;

        /// <summary>
        /// Заблокирован IP
        /// </summary>
        /// <param name="IP">IPv4/6</param>
        /// <param name="PtrHostName">PTR запись</param>
        /// <param name="host">Домен</param>
        /// <param name="DomainID">Id домена</param>
        /// <param name="Msg">Причина блокировки</param>
        /// <param name="AddMinutes">На сколько минут заблокирован</param>
        public static Action<(string IP, string PtrHostName, string host, int DomainID, string Msg, int AddMinutes)> OnBlockedIP => (s) => BlockedIP?.Invoke(null, s);
        public static event EventHandler<ITuple> BlockedIP;

        /// <summary>
        /// Ответ AntiBot
        /// </summary>
        /// <param name="IP">IPv4/6</param>
        /// <param name="host">Домен</param>
        /// <param name="DomainID">Id домена</param>
        /// <param name="antiBotType">Шаблон проверки</param>
        public static Action<(string IP, string host, int DomainID, AntiBotType antiBotType)> OnResponseView => (s) => ResponseView?.Invoke(null, s);
        public static event EventHandler<ITuple> ResponseView;
    }
}
