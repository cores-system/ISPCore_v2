using System;
using System.Runtime.CompilerServices;

namespace ISPCore.Models.Triggers.Events.Security
{
    public class AntiDdos
    {
        /// <summary>
        /// Изменены настройки Anti-Ddos
        /// </summary>
        /// <param name=""></param>
        public static Action<(int tmp1, int tmp2)> OnChange => (s) => Change?.Invoke(null, s);
        public static event EventHandler<ITuple> Change;

        /// <summary>
        /// Значение TCP/UPD
        /// </summary>
        /// <param name="count">Текущее значение TCP/UPD</param>
        /// <param name="MaxTcpOrUpd">Максимальное значение TCP/UPD</param>
        public static Action<(int count, long MaxTcpOrUpd)> OnCountTcpOrUpd => (s) => CountTcpOrUpd?.Invoke(null, s);
        public static event EventHandler<ITuple> CountTcpOrUpd;

        /// <summary>
        /// IP добавлен в системный список
        /// </summary>
        /// <param name="IP">IPv4/6</param>
        /// <param name="PtrHostName">PTR запись</param>
        /// <param name="AddDays">Количество дней</param>
        public static Action<(string IP, string PtrHostName, int AddDays)> OnAddToWhitePtr => (s) => AddToWhitePtr?.Invoke(null, s);
        public static event EventHandler<ITuple> AddToWhitePtr;

        /// <summary>
        /// Заблокирован IP
        /// </summary>
        /// <param name="IP">IPv4/6</param>
        /// <param name="PtrHostName">PTR запись</param>
        /// <param name="BlockingMinute">Количество минут</param>
        public static Action<(string IP, string PtrHostName, int BlockingMinute)> OnBlockedIP => (s) => BlockedIP?.Invoke(null, s);
        public static event EventHandler<ITuple> BlockedIP;
    }
}
