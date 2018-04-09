using System;
using ISPCore.Models.Databases;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using ISPCore.Engine.Databases;
using ISPCore.Models.Base;
using ISPCore.Engine.Base.SqlAndCache;

namespace ISPCore.Engine.Cron
{
    public class BlockedIP
    {
        public static bool IsRun = false;
        public static void Run(CoreDB coreDB, IMemoryCache memoryCache)
        {
            if (IsRun)
                return;
            IsRun = true;

            // Удаляем из базы старые IP адреса
            if (!memoryCache.TryGetValue("Cron-BlockedIP_ClearIP", out byte _))
            {
                memoryCache.Set("Cron-BlockedIP_ClearIP", (byte)1, TimeSpan.FromHours(1));

                SqlToMode.SetMode(SqlMode.Read);
                foreach (var blockedIP in coreDB.BlockedsIP.AsNoTracking())
                {
                    if (DateTime.Now > blockedIP.BlockingTime)
                        coreDB.Database.ExecuteSqlCommand(ComandToSQL.Delete(nameof(coreDB.BlockedsIP), blockedIP.Id));
                }
                SqlToMode.SetMode(SqlMode.ReadOrWrite);

                // Раз в час
                GC.Collect(GC.MaxGeneration);
            }

            IsRun = false;
        }
    }
}
