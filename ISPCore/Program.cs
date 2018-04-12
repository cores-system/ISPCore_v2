using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;
using ISPCore.Models.Databases.json;
using Microsoft.AspNetCore;
using System.Net;
using ISPCore.Models.Databases;
using ISPCore.Engine;
using ISPCore.Engine.Base;
using ISPCore.Models.Base;
using ISPCore.Engine.Base.SqlAndCache;

namespace ISPCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = new CultureInfo("ru-RU");
            
            // Настройки сервера
            var host = WebHost.CreateDefaultBuilder(args)
                .UseKestrel(op =>
                {
                    #region Unix Socket
                    if (Platform.Get == PlatformOS.Unix || Platform.Get == PlatformOS.Docker)
                    {
                        if (File.Exists("/var/run/ispcore.sock"))
                            File.Delete("/var/run/ispcore.sock");

                        Directory.CreateDirectory("/var/run/");
                        op.ListenUnixSocket("/var/run/ispcore.sock");
                    }
                    #endregion
                    
                    // TCP
                    op.Listen(Platform.Get == PlatformOS.Docker ? IPAddress.Any : IPAddress.Parse("127.0.0.1"), 4538);

                    // ISPCore Panel
                    op.Listen(IPAddress.Any, 8793, listenOptions => listenOptions.UseHttps("cert.pfx", File.ReadAllText($"{Folders.Passwd}/cert.key")));
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

            // Запускаем крон
            Timer cron = new Timer(new TimerCallback(Cron), null, 0, 1000 * 60);
            Timer antiDdos = new Timer(new TimerCallback(AntiDdos), null, 0, 1000);
            Timer writeLogTo = new Timer(new TimerCallback(WriteLogTo.WriteLogToSql), null, 0, 1000);

            // Запускаем ASP.NET 
            host.Run();

            // Что-бы GC.Collect() не сносил таймеры
            antiDdos.Dispose();
            cron.Dispose();
            writeLogTo.Dispose();
        }


        public static void AntiDdos(object ob)
        {
            JsonDB jsonDB = Service.Get<JsonDB>();
            if (!jsonDB.AntiDdos.IsActive || Platform.Get != PlatformOS.Unix)
                return;

            var memoryCache = Service.Get<IMemoryCache>();
            Engine.Cron.AntiDdos.RunSecond(jsonDB, memoryCache);
            Engine.Cron.AntiDdos.RunBlocked(jsonDB, memoryCache); 
        }

        
        public static void Cron(object ob)
        {
            try
            {
                using (CoreDB coreDB = Service.Get<CoreDB>())
                {
                    JsonDB jsonDB = Service.Get<JsonDB>();
                    var memoryCache = Service.Get<IMemoryCache>();

                    Engine.Cron.Project.Run(coreDB, jsonDB, memoryCache);           // Получение новостей и списка изменений
                    Engine.Cron.UpdateAV.Run(coreDB, jsonDB, memoryCache);          // Обновление антивируса
                    
                    Engine.Cron.BlockedIP.Run(coreDB, memoryCache);                 // Удаляем из базы старые IP адреса
                    Engine.Cron.WhitePtrIP.Run(coreDB, memoryCache);                // Удаляем из базы старые IP адреса
                    Engine.RequestsFilter.Access.AccessIP.Clear();                  // Очистка списка IP с разрешенным доступом
                    Engine.Cron.Home.Run(coreDB, memoryCache);                      // Очистка журнала посещений
                    Engine.Cron.Auth.Run(coreDB, memoryCache);                      // Очистка сессий
                    Engine.Cron.AntiDdos.Run(coreDB, jsonDB, memoryCache);          // Сбор статистики && очистка базы и правил IPTables
                    Engine.Cron.AntiDdos.Run(coreDB, jsonDB, memoryCache);          // Сбор статистики && очистка базы и правил IPTables

                    Engine.Cron.Monitoring.Run(coreDB, memoryCache);                // Статистика "/isp/monitoring"
                    Engine.Base.SqlAndCache.WriteLogTo.CloseFiles();                // Закрытие потоков на файлах которые не используются
                    Engine.Base.SqlAndCache.WriteLogTo.ZipFiles();                  // Сжатие логов

                    Engine.Cron.Notifications.Run(coreDB, jsonDB, memoryCache);     // Уведомления
                    Engine.Cron.AntiVirus.Run(coreDB);                              // Проверка доменов антивирусом
                    Engine.Cron.SyncBackup.Run(coreDB, memoryCache);                // SyncBackup
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(Folders.File.SystemErrorLog, ex.ToString() + "\n\n=======================================================================\n\n");
            }
        }
    }
}
