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
        public static void Run(CoreDB coreDB, IMemoryCache memoryCache)
        {
            if (IsRun)
                return;
            IsRun = true;

            // Очистка журнала посещений
            if (!memoryCache.TryGetValue("Cron-Auth_Session", out _))
            {
                memoryCache.Set("Cron-Auth_Session", (byte)1, TimeSpan.FromHours(3));

                SqlToMode.SetMode(SqlMode.Read);
                foreach (var session in coreDB.Auth_Sessions.AsNoTracking())
                {
                    // Удаляем старые записи
                    if (DateTime.Now > session.Expires)
                        coreDB.Database.ExecuteSqlCommand(ComandToSQL.Delete(nameof(coreDB.Auth_Sessions), session.Id));
                }
                SqlToMode.SetMode(SqlMode.ReadOrWrite);
            }

            IsRun = false;
        }
    }
}
