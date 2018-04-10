using System;

namespace ISPCore.Engine.Base.SqlAndCache
{
    public static class KeyToMemoryCache
    {
        public static string CheckLinkWhitelistTo2FA(string host, string IP) => $"CheckLinkWhitelistTo2FA-{IP}_{host}";
        public static string CheckLinkWhitelistToAll(string host, string IP) => $"CheckLinkWhitelistToAll-{IP}_{host}";
        public static string CheckLinkWhitelistToAllDomain(string IP) => $"CheckLinkWhitelistToAllDomain-{IP}";

        public static string LimitRequestToreCAPTCHA(string IP) => $"KeyToMem:LimitRequestToreCAPTCHA-{IP}";

        public static string IPtables(string IP) => $"IPtables-{IP}";
        public static string IPtables(string IP, string host) => $"IPtables-{IP}_{host}";

        public static string LimitLogin(string IP) => $"LimitLogin-{IP}";
        public static string LimitLoginCookie(string act, string IP) => $"LimitLoginCookie{act}-{IP}";
        public static string IspNumberOfRequestToHour(DateTime time) => $"isp_NumberOfRequestToHour-{time.Hour}";

        public static string AntiDdosNumberOfRequestDay(DateTime time) => $"AntiDdosNumberOfRequestDay-{time.Hour}";
        
        public static string LimitRequestToWhiteIP(string IP, string host) => $"LimitRequestToWhiteIP-{IP}_{host}";
        public static string WhitePtrIP(string IP) => $"WhitePtrIP-{IP}";
    }
}
