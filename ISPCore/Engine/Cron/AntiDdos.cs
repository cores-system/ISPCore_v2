using System;
using System.Linq;
using ISPCore.Models.Databases;
using ISPCore.Models.Databases.json;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Net;
using Microsoft.EntityFrameworkCore;
using ISPCore.Engine.Databases;
using ISPCore.Engine.Base;
using ISPCore.Models.Security.AntiDdos;
using ISPCore.Models.Base;
using ISPCore.Engine.Base.SqlAndCache;
using Trigger = ISPCore.Models.Triggers.Events.Security.AntiDdos;

namespace ISPCore.Engine.Cron
{
    public class AntiDdos
    {
        private static ConcurrentQueue<string> BlockedIP = new ConcurrentQueue<string>();
        private static NumberOfRequestDay dataHour;

        #region RunSecond
        private static bool IsRunSecond = false;
        public static void RunSecond(JsonDB jsonDB, IMemoryCache memoryCache)
        {
            if (IsRunSecond)
                return;
            IsRunSecond = true;

            try
            {
                Bash bash = new Bash();
                byte second = (byte)DateTime.Now.Second;

                #region Максимальное значение TCP/UPD за текущий час

                int MaxTcpOrUpd = int.Parse(bash.Run("ss -ntu | wc -l"));

                // Записываем данные в кеш
                if (memoryCache.TryGetValue(KeyToMemoryCache.AntiDdosNumberOfRequestDay(DateTime.Now), out dataHour))
                {
                    Trigger.OnCountTcpOrUpd((MaxTcpOrUpd, dataHour.value));

                    if (MaxTcpOrUpd > dataHour.value)
                    {
                        dataHour.value = MaxTcpOrUpd;
                        memoryCache.Set(KeyToMemoryCache.AntiDdosNumberOfRequestDay(DateTime.Now), dataHour);
                    }
                }
                else
                {
                    dataHour = new NumberOfRequestDay()
                    {
                        value = MaxTcpOrUpd,
                        Time = DateTime.Now
                    };

                    memoryCache.Set(KeyToMemoryCache.AntiDdosNumberOfRequestDay(DateTime.Now), dataHour);
                    Trigger.OnCountTcpOrUpd((MaxTcpOrUpd, dataHour.value));
                }
                #endregion

                #region Получаем список IP которые нужно заблокировать
                List<string> MassIP = new List<string>();

                // Количиство активных соеденений
                MassIP.AddRange(bash.Run(@"ss -ntu | awk '{print $6}' | sed 's%\:[0-9]*$%%g' | sed 's%\:\:ffff\:\([0-9]*\.[0-9]*\.[0-9]*\.[0-9]*\)%\1%g' | sort | uniq -ic | awk '{if ($1 >= " + jsonDB.AntiDdos.MaximumBurstSize + ") {print $2}}'").Split('\n'));

                // Максимальное количество запросов за 120 секунд
                if (!memoryCache.TryGetValue("Cron_AntiDdos-tcpdump", out _))
                {
                    // Создаем кеш на 120 секунд
                    memoryCache.Set("Cron_AntiDdos-tcpdump", (byte)0, TimeSpan.FromSeconds(120));

                    // Убиваем tcpdump
                    bash.Run("pkill tcpdump");

                    // Считываем файл
                    MassIP.AddRange(bash.Run(@"awk '{if ($6 > 0) {print $2}}' " + Folders.Log + @"/tcpdump.log | sed 's%\.[0-9]*$%%g' | sed 's%\:\:ffff\:\([0-9]*\.[0-9]*\.[0-9]*\.[0-9]*\)%\1%g' | sort | uniq -ic | awk '{if ($1 >= " + jsonDB.AntiDdos.NumberOfRequestsIn120Second + ") {print $2}}'").Split('\n'));

                    // Запускаем tcpdump
                    bash.Run($"tcpdump -i {jsonDB.AntiDdos.Interface} -nnpqSt dst port {string.Join(" or dst port ", jsonDB.AntiDdos.CheckPorts.Split(','))} > {Folders.Log}/tcpdump.log 2> /dev/null &");
                }
                #endregion

                #region Блокируем IP
                foreach (var IP in MassIP)
                {
                    // IP в белом списке
                    if (string.IsNullOrWhiteSpace(IP) || IsWhiteIP(jsonDB, memoryCache, IP))
                        continue;

                    // Блокируем IP
                    Blocked(memoryCache, Regex.Replace(IP, "[\n\r\t ]+", ""));
                }
                #endregion
            }
            catch { }

            IsRunSecond = false;
        }
        #endregion

        #region Run
        private static bool IsRun = false;
        public static void Run(CoreDB coreDB, JsonDB jsonDB, IMemoryCache memoryCache)
        {
            if (IsRun || !jsonDB.AntiDdos.IsActive || Platform.Get != PlatformOS.Unix)
                return;
            IsRun = true;

            #region Переносим данные TCP/UPD с кеша в базу (за прошлый час)
            var TimeAntiDdosNumberOfRequestDay = DateTime.Now.AddHours(-1);
            if (memoryCache.TryGetValue(KeyToMemoryCache.AntiDdosNumberOfRequestDay(TimeAntiDdosNumberOfRequestDay), out NumberOfRequestDay dataLastHour))
            {
                // Меняем режим доступа к SQL
                SqlToMode.SetMode(SqlMode.Read);

                // Записываем данные в базу
                coreDB.AntiDdos_NumberOfRequestDays.Add(dataLastHour);

                // Сохраняем базу
                coreDB.SaveChanges();

                // Меняем режим доступа к SQL
                SqlToMode.SetMode(SqlMode.ReadOrWrite);

                // Сносим кеш (статистика за час)
                memoryCache.Remove(KeyToMemoryCache.AntiDdosNumberOfRequestDay(TimeAntiDdosNumberOfRequestDay));
            }
            #endregion

            #region Очистка баз + перенос NumberOfRequestDay в NumberOfRequestMonth
            if (memoryCache.TryGetValue("CronAntiDdosClearDB", out DateTime CronClearDB))
            {
                // Если дата отличается от текущей
                if (CronClearDB.Day != DateTime.Now.Day)
                {
                    // Меняем режим доступа к SQL
                    SqlToMode.SetMode(SqlMode.Read);

                    // Обновляем кеш
                    memoryCache.Set("CronAntiDdosClearDB", DateTime.Now);

                    #region Очищаем NumberOfRequestMonth
                    foreach (var item in coreDB.AntiDdos_NumberOfRequestMonths.AsNoTracking())
                    {
                        // Если записи больше 90 дней
                        if ((DateTime.Now - item.Time).TotalDays > 90)
                            coreDB.Database.ExecuteSqlCommand(ComandToSQL.Delete(nameof(coreDB.AntiDdos_NumberOfRequestMonths), item.Id));
                    }
                    #endregion

                    #region Очищаем Jurnals
                    foreach (var item in coreDB.AntiDdos_Jurnals.AsNoTracking())
                    {
                        // Если записи больше 90 дней
                        if ((DateTime.Now - item.Time).TotalDays > 90)
                            coreDB.Database.ExecuteSqlCommand(ComandToSQL.Delete(nameof(coreDB.AntiDdos_Jurnals), item.Id));
                    }
                    #endregion

                    #region Очищаем NumberOfRequestDay + Переносим NumberOfRequestDay в NumberOfRequestMonth
                    // Хранимм дату и значение
                    var NumberOfRequestMonth = new Dictionary<int, (DateTime time, long value, int CountBlocked)>();

                    // Собираем статистику за прошлые дни
                    foreach (var item in coreDB.AntiDdos_NumberOfRequestDays.AsNoTracking())
                    {
                        // Пропускаем статистику за сегодня
                        if (item.Time.Day == DateTime.Now.Day && item.Time.Month == DateTime.Now.Month)
                            continue;

                        #region Переносим значения в NumberOfRequestMonth
                        if (NumberOfRequestMonth.TryGetValue(item.Time.Day, out (DateTime time, long value, int CountBlocked) it))
                        {
                            NumberOfRequestMonth[item.Time.Day] = (it.time, (item.value > it.value ? item.value : it.value), (item.CountBlocked + it.CountBlocked));
                        }
                        else
                        {
                            NumberOfRequestMonth.Add(item.Time.Day, (item.Time, item.value, item.CountBlocked));
                        }
                        #endregion

                        // Удаляем значения из базы
                        coreDB.Database.ExecuteSqlCommand(ComandToSQL.Delete(nameof(coreDB.AntiDdos_NumberOfRequestDays), item.Id));
                    }

                    // Переносим временные данные с NumberOfRequestMonth в базу
                    foreach (var item in NumberOfRequestMonth)
                    {
                        // Добовляем в базу
                        coreDB.AntiDdos_NumberOfRequestMonths.Add(new NumberOfRequestMonth()
                        {
                            Time = item.Value.time,
                            value = item.Value.value,
                            CountBlocked = item.Value.CountBlocked
                        });
                    }
                    #endregion

                    // Сохраняем базу
                    coreDB.SaveChanges();

                    // Меняем режим доступа к SQL
                    SqlToMode.SetMode(SqlMode.ReadOrWrite);

                    // Раз в сутки
                    GC.Collect(GC.MaxGeneration);
                }
            }
            else
            {
                // Создаем кеш задним числом
                memoryCache.Set("CronAntiDdosClearDB", DateTime.Now.AddDays(-1));
            }
            #endregion

            #region Очистка IPTables/IP6Tables
            if (jsonDB.AntiDdos.BlockToIPtables)
            {
                Bash bash = new Bash();

                foreach (var comandTables in "iptables,ip6tables".Split(','))
                {
                    // Список IP
                    foreach (var line in bash.Run(comandTables + " -L INPUT -v --line-numbers | awk '{print $1,$2,$9,$12}'").Split('\n').Reverse())
                    {
                        // Разбираем строку
                        var gr = new Regex("^([0-9]+) ([^ ]+) [^ ]+ ISPCore_([^\n\r]+)$").Match(line).Groups;
                        if (string.IsNullOrWhiteSpace(gr[1].Value) || !DateTime.TryParse(gr[3].Value, out DateTime time))
                            continue;

                        // Если время блокировки истекло
                        if (DateTime.Now > time)
                        {
                            if (jsonDB.AntiDdos.ActiveLockMode)
                            {
                                if (gr[2].Value == "0")
                                {
                                    bash.Run($"{comandTables} -D INPUT {gr[1].Value}");
                                }
                                else
                                {
                                    bash.Run($"{comandTables} -Z INPUT {gr[1].Value}");
                                }
                            }
                            else
                            {
                                bash.Run($"{comandTables} -D INPUT {gr[1].Value}");
                            }
                        }
                    }
                }
            }
            #endregion

            IsRun = false;
        }
        #endregion

        #region RunBlocked
        private static bool IsRunBlocked = false;
        public static void RunBlocked(JsonDB jsonDB, IMemoryCache memoryCache)
        {
            if (IsRunBlocked || BlockedIP.IsEmpty)
                return;
            IsRunBlocked = true;

            // 
            bool BlockToIPtables = jsonDB.AntiDdos.BlockToIPtables;

            #region Получаем текущий список заблокированных IP
            string IPv4 = string.Empty;
            string IPv6 = string.Empty;

            if (BlockToIPtables)
            {
                IPv4 = new Bash().Run("iptables -L -n -v | grep \"ISPCore_\" | awk '{print $8}'");
                IPv6 = new Bash().Run("ip6tables -L -n -v | grep \"ISPCore_\" | awk '{print $8}'");
            }
            #endregion

            // Блокируем IP
            Parallel.For(0, BlockedIP.Count, new ParallelOptions { MaxDegreeOfParallelism = jsonDB.Base.CountParallel }, (index, state) =>
            {
                try
                {
                    if (BlockedIP.TryDequeue(out string IP))
                    {
                        // IP уже заблокирован
                        if (IPv4.Contains(IP) || IPv6.Contains(IP))
                            return;

                        #region DNSLookup
                        string HostName = null;
                        try
                        {
                            if (jsonDB.AntiDdos.DNSLookupEnabled)
                            {
                                // Получаем имя хоста по IP
                                var host = Dns.GetHostEntryAsync(IP).Result;

                                // Получаем IP хоста по имени
                                host = Dns.GetHostEntryAsync(host.HostName).Result;

                                // Проверяем имя хоста и IP на совпадение 
                                if (host.AddressList.Where(i => i.ToString() == IP).FirstOrDefault() != null)
                                {
                                    HostName = host.HostName;

                                    // Проверяем имя хоста на белый список DNSLookup
                                    if (Regex.IsMatch(host.HostName, WhiteUserList.PtrRegex, RegexOptions.IgnoreCase))
                                    {
                                        // Добовляем IP в белый список на неделю
                                        WhitePtr.Add(IP, host.HostName, DateTime.Now.AddDays(7));
                                        Trigger.OnAddToWhitePtr((IP, HostName, 7));

                                        // Удаляем временное значение с кеша
                                        memoryCache.Remove($"AntiDdosCheckBlockedIP-{IP}");
                                        return;
                                    }
                                }
                            }
                        }
                        catch { }
                        #endregion

                        // Добовляем IP в IPtables
                        if (BlockToIPtables) {
                            string comandTables = IP.Contains(":") ? "ip6tables" : "iptables";
                            new Bash().Run($"{comandTables} -A INPUT -s {IP} -m comment --comment \"ISPCore_{DateTime.Now.AddMinutes(jsonDB.AntiDdos.BlockingTime).ToString("yyy-MM-ddTHH:mm:00")}\" -j REJECT");
                        }

                        // 
                        Trigger.OnBlockedIP((IP, HostName, jsonDB.AntiDdos.BlockingTime));

                        // Пишем IP в базу
                        if (jsonDB.AntiDdos.Jurnal)
                        {
                            (string Country, string City, string Region) geo = ("Disabled", "Disabled", "Disabled");
                            if (jsonDB.AntiDdos.GeoIP) {
                                geo = GeoIP2.City(IP);
                            }

                            WriteLogTo.SQL(new Jurnal()
                            {
                                City = geo.City,
                                Country = geo.Country,
                                Region = geo.Region,
                                HostName = HostName,
                                IP = IP,
                                Time = DateTime.Now
                            });
                        }

                        // Обновляем кеш
                        int BlockingTime = jsonDB.AntiDdos.BlockingTime > 10 ? 10 : jsonDB.AntiDdos.BlockingTime;
                        memoryCache.Set($"AntiDdosCheckBlockedIP-{IP}", (byte)0, TimeSpan.FromMinutes(BlockingTime));
                    }
                }
                catch { }
            });

            IsRunBlocked = false;
        }
        #endregion

        #region Blocked
        private static void Blocked(IMemoryCache memoryCache, string IP)
        {
            byte bit = 0;
            string key = $"AntiDdosCheckBlockedIP-{IP}";

            if (!memoryCache.TryGetValue(key, out bit))
            {
                // Пишем IP в кеш, что-бы два раза не писать в базу один и тот-же IP
                memoryCache.Set(key, bit, TimeSpan.FromMinutes(30));

                // Добовляем IP в базу блокировок
                BlockedIP.Enqueue(IP);

                // Записываем в кеш
                if (dataHour != null)
                {
                    dataHour.CountBlocked++;
                    memoryCache.Set(KeyToMemoryCache.AntiDdosNumberOfRequestDay(DateTime.Now), dataHour);
                }
            }
        }
        #endregion

        #region IsWhiteIP
        private static bool IsWhiteIP(JsonDB jsonDB, IMemoryCache memoryCache, string IP)
        {
            // Глобальный список белых IP
            if (WhitePtr.IsWhiteIP(IP))
                return true;

            // Результат
            return WhiteUserList.IsWhiteIP(IP);
        }
        #endregion  
    }
}
