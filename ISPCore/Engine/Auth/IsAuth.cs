using System;
using ISPCore.Engine.Base.SqlAndCache;
using ISPCore.Engine.Hash;
using ISPCore.Models.Auth;
using ISPCore.Models.Base;
using ISPCore.Models.Databases;
using Microsoft.AspNetCore.Http;

namespace ISPCore.Engine.Auth
{
    public class IsAuth
    {
        public static bool Auth(IRequestCookieCollection Cookies, string RemoteIpAddress, out bool IsConfirm2FA)
        {
            IsConfirm2FA = false;
            if (Cookies.TryGetValue("authSession", out string authSession))
            {
                SqlToMode.SetMode(SqlMode.Read);
                using (var coreDB = Service.Get<CoreDB>())
                {
                    // Поиск сессии
                    if (coreDB.Auth_Sessions.FindItem(i => i.Session == authSession) is AuthSession item)
                    {
                        // Сессия не истекла
                        // IP-адрес совпадает 
                        if (item.Expires > DateTime.Now && item.IP == RemoteIpAddress)
                        {
                            // Хеш пароля совпадает с текущем 
                            if (item.HashPasswdToRoot == SHA256.Text(PasswdTo.Root + PasswdTo.salt))
                            {
                                IsConfirm2FA = item.Confirm2FA;
                                return true;
                            }
                        }
                    }
                }
                SqlToMode.SetMode(SqlMode.ReadOrWrite);

                // Защита от перебора пароля по кукам
                LimitLogin.FailCookieAuthorization("authSession", authSession, RemoteIpAddress);
            }

            return false;
        }
    }
}
