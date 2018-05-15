using ISPCore.Engine.Network;
using ISPCore.Models.Base.WhiteList;
using ISPCore.Models.Databases.json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ISPCore.Engine.Base.SqlAndCache
{
    public static class WhiteUserList
    {
        #region WhiteUserList
        /// <summary>
        /// 
        /// </summary>
        static List<CidrToIPv4> IPv4ToRange = null;

        /// <summary>
        /// Белый список IPv6/Regex
        /// </summary>
        static string IPv6ToRegex = "^$";

        /// <summary>
        /// Белый список PTR/Regex
        /// </summary>
        public static string PtrRegex { get; private set; } = "^$";

        /// <summary>
        /// Белый список UserAgent/Regex
        /// </summary>
        static string UserAgentRegex = "^$";

        /// <summary>
        /// Время обновления настроек
        /// </summary>
        public static DateTime LastUpdateCache { get; private set; }
        #endregion

        #region UpdateCache
        /// <summary>
        /// Кеш настроек WhiteList
        /// </summary>
        public static void UpdateCache()
        {
            // Оригинальные настройки WhiteList
            var conf = Service.Get<JsonDB>().WhiteList;

            #region Локальный метод - "JoinMass"
            string JoinMass(List<string> mass, bool IsUserAgent = false, bool IsIPv6 = false)
            {
                if (mass == null || mass.Count == 0)
                    return "^$";

                if (IsUserAgent || IsIPv6)
                    return $"({string.Join("|", mass)})";

                return $"^({string.Join("|", mass)})$";
            }
            #endregion

            #region Обновляем список IPv4/6
            // Базовый список IPv6
            var IPv6ToMass = new List<string>() { "::1" };

            // Базовый список IPv4
            var IPv4ToMass = new List<CidrToIPv4>();
            IPv4ToMass.Add(IPNetwork.IPv4ToRange("127.0.0.1"));
            IPv4ToMass.Add(IPNetwork.IPv4ToRange("8.8.4.4"));
            IPv4ToMass.Add(IPNetwork.IPv4ToRange("8.8.8.8"));
            IPv4ToMass.Add(IPNetwork.IPv4ToRange("192.168.0.1", "192.168.0.254"));

            // Пользовательский список IPv4/6
            foreach (string IP in conf.Where(i => i.Type == WhiteListType.IPv4Or6).Select(i => i.Value))
            {
                if (IP.Contains(":"))
                {
                    // IPv6
                    if (IPNetwork.CheckingSupportToIPv4Or6(IP, out var ipnetwork))
                        IPv6ToMass.Add(IPNetwork.IPv6ToRegex(ipnetwork.FirstUsable));
                }
                else
                {
                    // IPv4
                    if (IPNetwork.CheckingSupportToIPv4Or6(IP, out var ipnetwork))
                    {
                        if (IPNetwork.IPv4ToRange(ipnetwork.FirstUsable, ipnetwork.LastUsable) is var item && item.FirstUsable != 0)
                            IPv4ToMass.Add(item);
                    }
                }
            }

            // Обновляем базу
            IPv4ToRange = IPv4ToMass.OrderBy(i => i.FirstUsable).ToList();
            IPv6ToRegex = JoinMass(IPv6ToMass, IsIPv6: true);
            #endregion

            // Базовый список PTR
            List<string> PTRs = new List<string>(conf.Where(i => i.Type == WhiteListType.PTR).Select(i => i.Value).ToArray());
            PTRs.Add(@".*\.(yandex.(ru|net|com)|googlebot.com|google.com|mail.ru|search.msn.com)");

            // Создаем кеш
            PtrRegex = JoinMass(PTRs);
            UserAgentRegex = JoinMass(conf.Where(i => i.Type == WhiteListType.UserAgent).Select(i => i.Value).ToList(), IsUserAgent: true);
            LastUpdateCache = DateTime.Now;
        }
        #endregion

        #region IsWhiteIP
        /// <summary>
        /// Проверка IP в белом списке
        /// </summary>
        /// <param name="IP">IPv4/6</param>
        public static bool IsWhiteIP(string IP)
        {
            // IPv6
            if (IP.Contains(":"))
            {
                if (IPv6ToRegex != "^$" && Regex.IsMatch(IP, IPv6ToRegex))
                    return true;

                return false;
            }

            // IPv4
            return IPNetwork.CheckToIPv4(IP, IPv4ToRange, out _);
        }
        #endregion

        #region IsWhiteUserAgent
        /// <summary>
        /// Проверка User-Agent в белом списке
        /// </summary>
        /// <param name="userAgent">User-Agent</param>
        public static bool IsWhiteUserAgent(string userAgent)
        {
            if (UserAgentRegex == "^$")
                return false;

            return Regex.IsMatch(userAgent, UserAgentRegex, RegexOptions.IgnoreCase);
        }
        #endregion
    }
}
