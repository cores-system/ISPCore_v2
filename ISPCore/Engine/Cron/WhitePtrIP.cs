using System;
using ISPCore.Models.Databases;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using ISPCore.Engine.Databases;
using ISPCore.Models.Base;
using ISPCore.Engine.Base.SqlAndCache;
using Trigger = ISPCore.Models.Triggers.Events.Base.SqlAndCache.WhitePtrIP;

namespace ISPCore.Engine.Cron
{
    public class WhitePtrIP
    {
        public static bool IsRun = false;
        public static void Run(CoreDB coreDB, IMemoryCache memoryCache)
        {
            if (IsRun)
                return;
            IsRun = true;

            // Удаляем из базы старые IP адреса
            if (!memoryCache.TryGetValue("Cron-WhitePtrIP_ClearIP", out byte _))
            {
                memoryCache.Set("Cron-WhitePtrIP_ClearIP", (byte)1, TimeSpan.FromHours(1));

                SqlToMode.SetMode(SqlMode.Read);
                foreach (var whiteIP in coreDB.WhitePtrIPs.AsNoTracking())
                {
                    if (DateTime.Now > whiteIP.Expires)
                    {
                        coreDB.Database.ExecuteSqlCommand(ComandToSQL.Delete(nameof(coreDB.WhitePtrIPs), whiteIP.Id));
                        Trigger.OnRemove((whiteIP.IPv4Or6, whiteIP.PTR));
                    }
                }
                SqlToMode.SetMode(SqlMode.ReadOrWrite);

                // Раз в час
                GC.Collect(GC.MaxGeneration);
            }

            IsRun = false;
        }
    }
}
