using System;
using Microsoft.AspNetCore.Http;

namespace ISPCore.Engine.Auth
{
    public class IsAuth
    {
        public static bool Auth(IRequestCookieCollection Cookies, string RemoteIpAddress)
        {
            if (Cookies.TryGetValue("auth", out string auth))
            {
                if (auth == PasswdToMD5.Root)
                    return true;

                // Защита от перебора пароля по кукам
                LimitLogin.FailCookieAuthorization("auth", auth, RemoteIpAddress);
            }

            return false;
        }
    }
}
