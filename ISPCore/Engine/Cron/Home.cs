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
    public class Home
    {
        private static bool IsRun = false;
        public static void Run(CoreDB coreDB, JsonDB jsonDB, IMemoryCache memoryCache)
        {
            if (IsRun)
                return;
            IsRun = true;

            // Очистка журнала посещений
            if (!memoryCache.TryGetValue("Cron-Home_Jurnals", out _))
            {
                memoryCache.Set("Cron-Home_Jurnals", (byte)1, TimeSpan.FromHours(12));

                SqlToMode.SetMode(SqlMode.Read);
                var expires = DateTime.Now.AddDays(-30);

                // Пропускаем последние 60 записей
                foreach (var jurn in coreDB.Home_Jurnals.AsNoTracking().AsEnumerable().Reverse().Skip(60))
                {
                    // Удаляем старые записи
                    if (expires > jurn.Time)
                        coreDB.Database.ExecuteSqlCommand(ComandToSQL.Delete(nameof(coreDB.Home_Jurnals), jurn.Id));
                }
                SqlToMode.SetMode(SqlMode.ReadOrWrite);

                // Раз в 12 часов
                GC.Collect(GC.MaxGeneration);
            }

            IsRun = false;
        }
    }
}
