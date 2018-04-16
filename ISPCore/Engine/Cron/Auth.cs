using System;
using Microsoft.Extensions.Caching.Memory;
using ISPCore.Models.Databases;
using ISPCore.Models.Databases.json;
using Microsoft.EntityFrameworkCore;
using ISPCore.Engine.Databases;
using ISPCore.Models.Base;
using System.Linq;
using ISPCore.Engine.Base.SqlAndCache;

namespace ISPCore.Engine.Cron
{
    public class Auth
    {
        private static bool IsRun = false;
        public static void Run(CoreDB coreDB, JsonDB jsonDB, IMemoryCache memoryCache)
        {
            if (IsRun)
                return;
            IsRun = true;

            // Очистка сессий
            if (!memoryCache.TryGetValue("Cron-Auth_Session", out _))
            {
                memoryCache.Set("Cron-Auth_Session", (byte)1, TimeSpan.FromMinutes(30));

                SqlToMode.SetMode(SqlMode.Read);
                foreach (var session in coreDB.Auth_Sessions.AsNoTracking())
                {
                    // Удаляем старые записи
                    // Если включена авторизация 2FA и сессии больше 20 минут 
                    if (DateTime.Now > session.Expires || (jsonDB.Base.EnableTo2FA && !session.Confirm2FA && DateTime.Now.AddMinutes(-20) > session.CreateTime))
                        coreDB.Database.ExecuteSqlCommand(ComandToSQL.Delete(nameof(coreDB.Auth_Sessions), session.Id));
                }
                SqlToMode.SetMode(SqlMode.ReadOrWrite);
            }

            IsRun = false;
        }
    }
}
