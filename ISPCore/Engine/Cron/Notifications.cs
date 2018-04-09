using System;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using ISPCore.Models.Databases;
using Microsoft.EntityFrameworkCore;
using ISPCore.Models.Databases.json;
using ISPCore.Models.Base;
using ISPCore.Engine.Base.SqlAndCache;

namespace ISPCore.Engine.Cron
{
    public class Notifications
    {
        public static void Run(CoreDB coreDB, JsonDB jsonDB, IMemoryCache memoryCache)
        {
            // Очистка базы от старых уведомлений
            if (memoryCache.TryGetValue("CronNoteClearDB", out DateTime CronDate))
            {
                // Если дата отличается от текущей
                if (CronDate.Day != DateTime.Now.Day)
                {
                    // Меняем режим доступа к SQL
                    SqlToMode.SetMode(SqlMode.Read);

                    // Создаем кеш
                    memoryCache.Set("CronNoteClearDB", DateTime.Now);

                    // Удаляем  записи старше 180 дней
                    foreach (var item in coreDB.Notations.AsNoTracking())
                    {
                        if (DateTime.Now > item.Time.AddDays(180))
                        {
                            // Удаляем заметку
                            coreDB.Notations.RemoveAttach(coreDB, item.Id);
                        }
                    }

                    // Меняем режим доступа к SQL
                    SqlToMode.SetMode(SqlMode.ReadOrWrite);

                    // Раз в день
                    GC.Collect(GC.MaxGeneration);
                }
            }
            else
            {
                // Создаем кеш задним числом
                memoryCache.Set("CronNoteClearDB", DateTime.Now.AddDays(-1));
            }
        }
    }
}
