using ISPCore.Models.Base.WhiteList;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace ISPCore.Engine.Network
{
    public static class IPNetwork
    {
        #region CheckingSupportToIPv4Or6
        /// <summary>
        /// Проверка правильности CIDR
        /// </summary>
        /// <param name="IPv4Or6">IP-адрес</param>
        /// <param name="ipnetwork"></param>
        public static bool CheckingSupportToIPv4Or6(string IPv4Or6, out (string FirstUsable, string LastUsable) ipnetwork)
        {
            ipnetwork = (null, null);

            try
            {
                #region Просто IP без "/24"
                if (!IPv4Or6.Contains("/"))
                {
                    if (!IPAddress.TryParse(IPv4Or6, out _))
                        return false;

                    ipnetwork = (IPv4Or6, IPv4Or6);
                    return true;
                }
                #endregion

                // Парсим IPv4Or6
                var res = System.Net.IPNetwork.Parse(IPv4Or6);
                string FirstUsable = res.FirstUsable.ToString();
                string LastUsable = res.LastUsable.ToString();

                // IPv6
                if (IPv4Or6.Contains(":"))
                {
                    if (!FirstUsable.Contains(Regex.Replace(LastUsable, ":[f]{3,4}", "")))
                        return false;
                }

                // Успех
                ipnetwork = (FirstUsable, LastUsable);
                return true;
            }
            catch { }

            // Ошибка
            return false;
        }
        #endregion

        #region IPv4ToRange
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FirstUsable"></param>
        /// <param name="LastUsable"></param>
        public static CidrToIPv4 IPv4ToRange(string FirstUsable, string LastUsable = null)
        {
            if (LastUsable == null)
                LastUsable = FirstUsable;

            if (ConvertIPv4(FirstUsable, out ulong firstU) && ConvertIPv4(LastUsable, out ulong lastU))
            {
                if (firstU == lastU)
                    return new CidrToIPv4(lastU, lastU);

                if (firstU > lastU)
                    return new CidrToIPv4(lastU - 10, lastU + 10);

                return new CidrToIPv4(firstU - 10, lastU + 10);
            }

            // Ошибка
            return new CidrToIPv4(0, 0);
        }
        #endregion

        #region IPv6ToRegex
        /// <summary>
        /// 
        /// </summary>
        /// <param name="LastUsable"></param>
        public static string IPv6ToRegex(string FirstUsable)
        {
            return Regex.Replace(FirstUsable, "::$", ":");
        }
        #endregion

        #region CheckToIPv4
        /// <summary>
        /// Проверка IPv4
        /// </summary>
        /// <param name="IP">IPv4/6</param>
        /// <param name="mass"></param>
        /// <param name="FirstUsable"></param>
        public static bool CheckToIPv4(string IP, List<CidrToIPv4> mass, out ulong FirstUsable)
        {
            if (ConvertIPv4(IP, out ulong val))
                return BinarySearch(val, mass, out FirstUsable);

            FirstUsable = 0;
            return false;
        }
        #endregion

        #region ConvertIPv4
        /// <summary>
        /// Конверт IPv4 в ulong
        /// </summary>
        /// <param name="ip">IPv4</param>
        /// <param name="val">ulong</param>
        public static bool ConvertIPv4(string ip, out ulong val)
        {
            StringBuilder res = new StringBuilder();
            foreach (var item in ip.Split('.'))
            {
                switch (item.Length)
                {
                    case 1:
                        res.Append($"00{item}0");
                        break;

                    case 2:
                        res.Append($"0{item}0");
                        break;

                    case 3:
                        res.Append($"{item}0");
                        break;
                }
            }

            return ulong.TryParse(res.ToString(), out val);
        }
        #endregion

        #region BinarySearch
        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <param name="arr"></param>
        /// <param name="FirstUsable"></param>
        private static bool BinarySearch(ulong val, List<CidrToIPv4> arr, out ulong FirstUsable)
        {
            FirstUsable = 0;
            if (arr == null || arr.Count == 0)
                return false;

            if (1 == arr.Count)
            {
                var item = arr[0];
                return val >= item.FirstUsable && item.LastUsable >= val;
            }

            int right = arr.Count - 1;
            int left = 0;

            if (arr[left].FirstUsable > val)
                return false;

            if (arr[right].LastUsable < val)
                return false;

            while (left < right)
            {
                int mid = left + (right - left) / 2;
                var item = arr[mid];

                FirstUsable = item.FirstUsable;
                ulong LastUsable = item.LastUsable;

                if (val >= FirstUsable && LastUsable >= val)
                    return true;

                if (val < FirstUsable) {
                    right = mid;
                }
                else if (val > FirstUsable)
                {
                    left = mid + 1;
                    if (left >= right)
                    {
                        item = arr[arr.Count - 1];
                        return val >= item.FirstUsable && item.LastUsable >= val;
                    }
                }
                else {
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}
