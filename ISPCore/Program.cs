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
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ISPCore.Models.Command_Line;

namespace ISPCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            cmd cmd = new cmd();
            LogLevel logLevel = LogLevel.Error;
            CultureInfo.CurrentCulture = new CultureInfo("ru-RU");

            #region Command Line
            foreach (var line in args)
            {
                var g = new Regex("--([^=]+)(='?([^\n\r']+)'?)?").Match(line).Groups;
                string comand = g[1].Value.ToLower();
                string value = g[3].Value;

                switch (comand)
                {
                    #region platform
                    case "platform":
                        {
                            switch (value.ToLower())
                            {
                                case "docker":
                                    Platform.Set(IsDocker: true);
                                    break;
                                case "demo":
                                    Platform.Set(IsDemo: true);
                                    break;
                                case "debug":
                                    Platform.Set(IsDebug: true);
                                    break;
                            }
                            break;
                        }
                    #endregion

                    #region loglevel
                    case "loglevel":
                        {
                            if (Enum.TryParse(typeof(LogLevel), value, out object res))
                                logLevel = (LogLevel)res;
                            break;
                        }
                    #endregion

                    #region conf
                    case "conf":
                        {
                            cmd = Newtonsoft.Json.JsonConvert.DeserializeObject<cmd>(value);
                            break;
                        }
                    #endregion

                    #region timeout
                    case "timeout:core":
                        {
                            if (int.TryParse(value.Replace("s", ""), out int res))
                                cmd.Timeout.core = res;
                            break;
                        }
                    #endregion

                    #region statuscode
                    case "statuscode":
                        {
                            foreach (var item in value.ToLower().Split(','))
                            {
                                switch (item)
                                {
                                    case "iptables":
                                        cmd.StatusCode.IPtables = true;
                                        break;
                                    case "checklink":
                                        cmd.StatusCode.Checklink = true;
                                        break;
                                }
                            }
                            break;
                        }
                    #endregion

                    #region cache
                    case "cache:checklink":
                        {
                            if (int.TryParse(value.Replace("ms", ""), out int res))
                                cmd.Cache.Checklink = res;
                            break;
                        }

                    case "cache:antibot":
                        {
                            if (int.TryParse(value.Replace("ms", ""), out int res))
                                cmd.Cache.AntiBot = res;
                            break;
                        }
                    #endregion
                }
            }
            
            Startup.cmd = cmd;
            #endregion

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
                .ConfigureLogging(logging => logging.SetMinimumLevel(logLevel))
                .Build();

            // Запускаем крон
            Timer cronMinutes = new Timer(new TimerCallback(CronMinutes), null, 0, 1000 * 60);
            Timer cronSeconds = new Timer(new TimerCallback(CronSeconds), null, 0, 1000);
            Timer writeLogTo = new Timer(new TimerCallback(WriteLogTo.WriteLogToSql), null, 0, 1000);

            // Запускаем ASP.NET 
            host.Run();

            // Что-бы GC.Collect() не сносил таймеры
            cronSeconds.Dispose();
            cronMinutes.Dispose();
            writeLogTo.Dispose();
        }


        #region CronSeconds
        public static void CronSeconds(object ob)
        {
            try
            {
                // Удаляем из базы старые IP адреса
                Engine.Security.IPtables.ClearDbAndCacheToIPv4Or6();

                #region AntiDdos
                JsonDB jsonDB = Service.Get<JsonDB>();
                if (!jsonDB.AntiDdos.IsActive || Platform.Get != PlatformOS.Unix)
                    return;

                var memoryCache = Service.Get<IMemoryCache>();
                Engine.Cron.AntiDdos.RunSecond(jsonDB, memoryCache);
                Engine.Cron.AntiDdos.RunBlocked(jsonDB, memoryCache);
                #endregion
            }
            catch (Exception ex)
            {
                File.AppendAllText(Folders.File.SystemErrorLog, ex.ToString() + "\n\n=======================================================================\n\n");
            }
        }
        #endregion

        #region CronMinutes
        public static void CronMinutes(object ob)
        {
            try
            {
                using (CoreDB coreDB = Service.Get<CoreDB>())
                {
                    JsonDB jsonDB = Service.Get<JsonDB>();
                    var memoryCache = Service.Get<IMemoryCache>();

                    Engine.Cron.Project.Run(coreDB, jsonDB, memoryCache);           // Получение новостей и списка изменений
                    Engine.Cron.UpdateAV.Run(coreDB, jsonDB, memoryCache);          // Обновление антивируса
                    
                    Engine.Cron.WhitePtrIP.Run(coreDB, memoryCache);                // Удаляем из базы старые IP адреса
                    Engine.RequestsFilter.Access.AccessIP.Clear();                  // Очистка списка IP с разрешенным доступом
                    Engine.Cron.Home.Run(coreDB, memoryCache);                      // Очистка журнала посещений
                    Engine.Cron.Auth.Run(coreDB, jsonDB, memoryCache);              // Очистка сессий
                    Engine.Cron.AntiDdos.Run(coreDB, jsonDB, memoryCache);          // Сбор статистики && очистка базы и правил IPTables

                    Engine.Cron.Monitoring.Run(coreDB, memoryCache);                // Статистика "/isp/monitoring"
                    Engine.Base.SqlAndCache.WriteLogTo.CloseFiles();                // Закрытие потоков на файлах которые не используются
                    Engine.Base.SqlAndCache.WriteLogTo.ZipFiles();                  // Сжатие логов

                    Engine.Cron.Notifications.Run(coreDB, jsonDB, memoryCache);     // Уведомления
                    Engine.Cron.AntiVirus.Run(coreDB);                              // Проверка доменов антивирусом

                    Engine.Cron.SyncBackup.Database.Run(coreDB, memoryCache);       // SyncBackup - DB
                    Engine.Cron.SyncBackup.Files.Run(coreDB, memoryCache);          // SyncBackup - IO
                    Engine.Cron.SyncBackup.Files.ClearingTemp(memoryCache);         // Очистка временных файлов
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(Folders.File.SystemErrorLog, ex.ToString() + "\n\n=======================================================================\n\n");
            }
        }
        #endregion
    }
}
