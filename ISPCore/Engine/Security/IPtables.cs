﻿using System;
using Microsoft.Extensions.Caching.Memory;
using System.Text.RegularExpressions;
using ISPCore.Models.Databases;
using ISPCore.Models.RequestsFilter.Domains;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using ISPCore.Models.Base;
using ISPCore.Engine.Base.SqlAndCache;
using ModelIPtables = ISPCore.Models.Security.IPtables;
using ISPCore.Models.Base.WhiteList;
using ISPCore.Engine.Network;
using ISPCore.Engine.Databases;
using System.Collections.Concurrent;

namespace ISPCore.Engine.Security
{
    public static class IPtables
    {
        #region IPtables
        /// <summary>
        /// 
        /// </summary>
        static List<CidrToIPv4> IPv4ToRange = new List<CidrToIPv4>();

        /// <summary>
        /// 
        /// </summary>
        static ConcurrentDictionary<ulong, ModelIPtables> IPv4ToModels = new ConcurrentDictionary<ulong, ModelIPtables>();

        /// <summary>
        /// Белый список IPv6/Regex
        /// </summary>
        static string IPv6ToRegex = "^$";

        /// <summary>
        /// Белый список UserAgent/Regex
        /// </summary>
        static string UserAgentRegex = "^$";

        /// <summary>
        /// 
        /// </summary>
        static DateTime NextTimeClearDbAndCacheToIPv4Or6 = DateTime.Now.AddHours(1);
        #endregion

        #region CheckIP
        /// <summary>
        /// 
        /// </summary>
        /// <param name="RemoteIpAddress"></param>
        /// <param name="memoryCache"></param>
        /// <param name="data"></param>
        /// <param name="BlockedHost"></param>
        public static bool CheckIP(string RemoteIpAddress, IMemoryCache memoryCache, out ModelIPtables data, string BlockedHost = null)
        {
            data = null;

            // Блокировка по домену
            if (BlockedHost != null)
                return memoryCache.TryGetValue(KeyToMemoryCache.IPtables(RemoteIpAddress, BlockedHost), out data);

            // IPv6
            if (RemoteIpAddress.Contains(":"))
            {
                if (IPv6ToRegex != "^$" && Regex.IsMatch(RemoteIpAddress, IPv6ToRegex))
                {
                    data = new ModelIPtables();
                    return true;
                }

                return false;
            }

            // IPv4
            else
            {
                if (IPNetwork.CheckToIPv4(RemoteIpAddress, IPv4ToRange, out ulong FirstUsable))
                {
                    if (!IPv4ToModels.TryGetValue(FirstUsable, out data))
                        data = new ModelIPtables();
                    return true;
                }

                return false;
            }
        }
        #endregion

        #region AddIPv4Or6
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="data"></param>
        /// <param name="expires"></param>
        public static void AddIPv4Or6(string IP, ModelIPtables data, DateTime expires)
        {
            if (IPNetwork.CheckingSupportToIPv4Or6(IP, out var ipnetwork))
            {
                // Крон нужно запустить раньше
                if (NextTimeClearDbAndCacheToIPv4Or6 > expires)
                    NextTimeClearDbAndCacheToIPv4Or6 = expires;

                // IPv6
                if (IP.Contains(":"))
                {
                    if (IPv6ToRegex == "^$")
                    {
                        IPv6ToRegex = $"^({IPNetwork.IPv6ToRegex(ipnetwork.FirstUsable)})";
                        return;
                    }

                    IPv6ToRegex = Regex.Replace(IPv6ToRegex, @"\)$", $"|{IPNetwork.IPv6ToRegex(ipnetwork.FirstUsable)})");
                }

                // IPv4
                else
                {
                    if (IPNetwork.IPv4ToRange(ipnetwork.FirstUsable, ipnetwork.LastUsable) is var item && item.FirstUsable != 0)
                    {
                        #region Находим число которое выше FirstUsable и ставим FirstUsable перед ним
                        int index = IPv4ToRange.FindIndex(0, IPv4ToRange.Count, i => i.FirstUsable > item.FirstUsable);

                        if (index == -1)
                            IPv4ToRange.Add(item);
                        else
                            IPv4ToRange.Insert(index, item);
                        #endregion

                        // Время и причина блокировки
                        IPv4ToModels.AddOrUpdate(item.FirstUsable, data, (s,e) => data);
                    }
                }
            }
        }
        #endregion

        #region RemoveIPv4Or6
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IP"></param>
        public static void RemoveIPv4Or6(string IP)
        {
            if (IPNetwork.CheckingSupportToIPv4Or6(IP, out var ipnetwork))
            {
                // IPv6
                if (IP.Contains(":"))
                {
                    string ipRegex = IPNetwork.IPv6ToRegex(ipnetwork.FirstUsable);
                    IPv6ToRegex = Regex.Replace(IPv6ToRegex, $@"^\^\({ipRegex}\|?", "^(");
                    IPv6ToRegex = Regex.Replace(IPv6ToRegex, $@"\|{ipRegex}", "");
                    if (IPv6ToRegex == "^()")
                        IPv6ToRegex = "^$";
                }

                // IPv4
                else
                {
                    if (IPNetwork.IPv4ToRange(ipnetwork.FirstUsable, ipnetwork.LastUsable) is var item && item.FirstUsable != 0)
                    {
                        IPv4ToRange.RemoveAll(i => i.FirstUsable == item.FirstUsable && i.LastUsable == item.LastUsable);
                        IPv4ToModels.TryRemove(item.FirstUsable, out _);
                    }
                }
            }
        }
        #endregion

        #region ClearDbAndCacheToIPv4Or6
        public static void ClearDbAndCacheToIPv4Or6()
        {
            // Время еще не настало
            if (NextTimeClearDbAndCacheToIPv4Or6 > DateTime.Now)
                return;

            // Блокируем выполнение кода на час
            NextTimeClearDbAndCacheToIPv4Or6 = DateTime.Now.AddHours(1);

            SqlToMode.SetMode(SqlMode.Read);
            using (var coreDB = Service.Get<CoreDB>())
            {
                foreach (var blockedIP in coreDB.BlockedsIP.AsNoTracking())
                {
                    if (DateTime.Now > blockedIP.BlockingTime)
                    {
                        RemoveIPv4Or6(blockedIP.IP);
                        coreDB.Database.ExecuteSqlCommand(ComandToSQL.Delete(nameof(coreDB.BlockedsIP), blockedIP.Id));
                    }
                    else
                    {
                        // Крон нужно запустить раньше
                        if (NextTimeClearDbAndCacheToIPv4Or6 > blockedIP.BlockingTime)
                            NextTimeClearDbAndCacheToIPv4Or6 = blockedIP.BlockingTime;
                    }
                }
            }
            SqlToMode.SetMode(SqlMode.ReadOrWrite);
        }
        #endregion

        #region CheckUserAgent
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userAgent"></param>
        public static bool CheckUserAgent(string userAgent)
        {
            if (UserAgentRegex == "^$")
                return false;

            return Regex.IsMatch(userAgent, UserAgentRegex, RegexOptions.IgnoreCase);
        }
        #endregion

        #region UpdateCacheToUserAgent
        public static void UpdateCacheToUserAgent()
        {
            // Меняем режим доступа к SQL
            SqlToMode.SetMode(SqlMode.Read);

            // Подключаемся к базе
            using (CoreDB coreDB = Service.Get<CoreDB>())
            {
                List<string> mass = new List<string>();
                foreach (var blockedIP in coreDB.BlockedsIP.AsNoTracking())
                {
                    if (blockedIP.BlockingTime > DateTime.Now && blockedIP.typeBlockIP == TypeBlockIP.UserAgent)
                    {
                        mass.Add(blockedIP.IP);
                    }
                }

                // Клеим данные в строчку
                UserAgentRegex = mass.Count == 0 ? "^$" : $"({string.Join("|", mass)})";
            }

            // Меняем режим доступа к SQL
            SqlToMode.SetMode(SqlMode.ReadOrWrite);
        }
        #endregion

        #region BlockedToHtml
        public static string BlockedToHtml(string RemoteIpAddress, string description, DateTime TimeExpires)
        {
            if (description == null && TimeExpires == default(DateTime))
                return BlockedToHtml($"Ваш IP заблокирован<br />{RemoteIpAddress}");

            return BlockedToHtml($"Ваш IP {RemoteIpAddress} заблокирован<br />{description}" + $"<br /><br />Доступ будет открыт {TimeExpires.ToString("dd.MM.yyyy")} в {TimeExpires.AddMinutes(1).ToString("H:mm")}");
        }

        public static string BlockedToHtml(string msg) => @"<!DOCTYPE html>
<html lang='ru-RU'>
<head>
    <title>Доступ запрещен</title>
    <meta http-equiv='Content-Type' content='text/html; charset=utf-8'>
    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
    <meta name='viewport' content='width=device-width, initial-scale=1, user-scalable=no'>
    <script type='text/javascript' src='/statics/jquery.min.js'></script>
    <link rel='stylesheet' href='/statics/style.css'>
</head>
<body>
    <div class='error'>
        <div class='error-block'>

            <div class='code'>401</div>
                <div class='title'>" + $"Доступ запрещен</div><pre>{msg}</pre>" +
            @"
            <div class='copyright'>
                <div>
                    &copy; 2018 <strong>ISPCore</strong>. All rights reserved.
                </div>
                <div>
                    <a href='/'>Главная сайта</a> / <a href='http://core-system.org/' target='_blank'>Core System</a>
                </div>
            </div>

        </div>
    </div>
</body>
</html>
";
        #endregion
    }
}
