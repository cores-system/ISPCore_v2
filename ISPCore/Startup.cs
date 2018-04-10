using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Caching.Memory;
using ISPCore.Models.api;
using ISPCore.Models.Databases;
using ISPCore.Engine.Middleware;
using ISPCore.Engine.Base;
using ISPCore.Models.RequestsFilter.Domains;
using ISPCore.Engine;
using ISPCore.Models.Security;
using ISPCore.Models.Base;
using ISPCore.Hubs;
using ISPCore.Engine.RequestsFilter.Access;
using ISPCore.Models.RequestsFilter.Access;
using System.Net;
using ISPCore.Engine.Databases;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Threading.Tasks;
using ISPCore.Engine.Base.SqlAndCache;

namespace ISPCore
{
    public class Startup
    {
        #region Startup
        public Startup(IHostingEnvironment env)
        {
            // Указываем полный путь к софту
            Folders.SetRootPath(env.ContentRootPath);

            // Создаем папки
            Folders.CreateDirectory();

            // Миграция SQL
            MigrateSQL.ISPCore();
        }
        #endregion

        #region version
        public static LatestVersion version => new LatestVersion()
        {
            Version = 2.1,
            Patch = 5
        };

        public static LatestVersion vSql => new LatestVersion()
        {
            Version = 0.1,
            Patch = 2
        };
        #endregion

        /// <summary>
        /// Через какое время можно повторно запросить данные с API на сервере
        /// </summary>
        public static TimeSpan AbsoluteExpirationToAPI => TimeSpan.FromMinutes(60);


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CoreDB>(options => options.UseSqlite($"DataSource={Folders.File.ISPCoreDB}"));
            services.AddSignalR();
            services.AddMvc();
        } 
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime applicationLifetime, IMemoryCache memoryCache)
        {
            #region Системные настройки
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Страница для тестов
                app.UseMvc(routes => routes.MapRoute("TestPage", "test", new { controller = "Test", action = "Index" }));
            }
            #endregion

            #region Добовляем порт в белый список
            if (Platform.Get == PlatformOS.Unix)
            {
                if (string.IsNullOrWhiteSpace(new Bash().Run("iptables -L INPUT -v -n 2>/dev/null | grep 8793")))
                    new Bash().Run("iptables -I INPUT -p tcp --dport 8793 -j ACCEPT");
            }
            #endregion

            #region Unix Socket
            try
            {
                applicationLifetime.ApplicationStarted.Register(() =>
                {
                    if (Platform.Get == PlatformOS.Unix || Platform.Get == PlatformOS.Docker)
                    {
                        ThreadPool.QueueUserWorkItem((s) =>
                        {
                            while (true)
                            {
                                // Ждем 3 секунды
                                Thread.Sleep(1000 * 3);

                                // Меняем права доступа
                                if (File.Exists("/var/run/ispcore.sock")) {
                                    new Bash().Run("chmod 666 /var/run/ispcore.sock");
                                    return;
                                }
                            }
                        });
                    }
                });
            }
            catch { }
            #endregion

            // Создаем сервис
            Service.Create(memoryCache);

            #region Загружаем список BlockedIP в кеш
            using (CoreDB coreDB = Service.Get<CoreDB>())
            {
                // IP который нужно удалить
                string unlockip = string.Empty;
                if (File.Exists($"{Folders.Tmp}/unlockip.root"))
                    unlockip = File.ReadAllText($"{Folders.Tmp}/unlockip.root").Trim();

                // Загружаем IP адреса
                foreach (var blockedIP in coreDB.BlockedsIP.AsNoTracking())
                {
                    if (blockedIP.BlockingTime > DateTime.Now && blockedIP.typeBlockIP != TypeBlockIP.UserAgent)
                    {
                        // IP адрес
                        string RemoteIpAddress = blockedIP.IP.Replace(".*", "").Replace(":*", "");

                        // Белый IP
                        if (RemoteIpAddress.Contains(unlockip))
                            continue;

                        // Где именно блокировать IP
                        string keyIPtables = blockedIP.typeBlockIP == TypeBlockIP.domain ? KeyToMemoryCache.IPtables(RemoteIpAddress, blockedIP.BlockedHost) : KeyToMemoryCache.IPtables(RemoteIpAddress);

                        // Записываем IP в кеш IPtables
                        memoryCache.Set(keyIPtables, new IPtables(blockedIP.Description, blockedIP.BlockingTime), blockedIP.BlockingTime);
                    }
                }
            }
            #endregion

            #region Загружаем список WhitePtrIP в кеш
            using (CoreDB coreDB = Service.Get<CoreDB>())
            {
                // Загружаем IP адреса
                foreach (var item in coreDB.WhitePtrIPs.AsNoTracking())
                {
                    // Добовляем IP в кеш
                    if (item.Expires > DateTime.Now)
                        memoryCache.Set(KeyToMemoryCache.WhitePtrIP(item.IPv4Or6), (byte)0, item.Expires);
                }
            }
            #endregion

            #region Загружаем список "Разрешенные доступы" в кеш
            foreach (var item in AccessIP.List())
            {
                // IP для кеша
                string ipCache = item.IP.Replace(".*", "").Replace(":*", "");

                switch (item.accessType)
                {
                    case AccessType.all:
                        memoryCache.Set(KeyToMemoryCache.CheckLinkWhitelistToAll(item.host, ipCache), (byte)1, item.Expires);
                        break;
                    case AccessType.Is2FA:
                        memoryCache.Set(KeyToMemoryCache.CheckLinkWhitelistTo2FA(item.host, ipCache), (byte)1, item.Expires);
                        break;
                    case AccessType.allDomain:
                        memoryCache.Set(KeyToMemoryCache.CheckLinkWhitelistToAllDomain(ipCache), (byte)1, item.Expires);
                        break;
                }
            }
            #endregion

            // Статичиские файлы
            app.UseStaticFiles();

            #region IP-адрес клиента
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
                KnownProxies = { IPAddress.Parse("172.17.42.1"), IPAddress.Parse("127.0.0.1") },
            });
            #endregion

            // Страницы ошибок
            app.UseMvc(routes => {
                routes.MapRoute("ErrorPage-404", "404", new { controller = "Error", action = "_404" });
            });

            // Блокировка IP
            app.UseIPtablesMiddleware();

            #region Core API
            // Доступ запрещен с порта панели 8793
            app.UseCoreMiddleware();

            // Core CheckRequest
            app.Map("/core/check/request", ap => ap.Run(context => 
            {
                // Состояние потока
                bool RanToCompletion = false;

                // Завершаем подключение через 10 секунд
                var token = new CancellationTokenSource(1000 * 10).Token;
                token.Register(() =>
                {
                    if (!RanToCompletion)
                        context.Abort();
                });

                // Проверка запроса
                var task = Engine.core.Check.Request.Check(context);
                RanToCompletion = task.Status == TaskStatus.RanToCompletion;
                
                // Успех
                return task;
            }));

            // Core API
            app.UseMvc(routes => {
                routes.MapRoute(null, "core/unlock/2fa", new { controller = "CoreUnlock2FA", action = "Index" });
                routes.MapRoute(null, "core/check/cookie", new { controller = "CoreCheckCookie", action = "Index" });
                routes.MapRoute(null, "core/check/recaptcha", new { controller = "CoreCheckRecaptcha", action = "Base" });
                routes.MapRoute(null, "core/check/recaptcha/limitrequest", new { controller = "CoreCheckRecaptcha", action = "LimitRequest" });
                routes.MapRoute(null, "core/base/myip", new { controller = "CoreBase", action = "MyIP" });
            });

            // AntiBotHub
            app.UseSignalR(routes => routes.MapHub<AntiBotHub>("core/AntiBotHub"));

            // Генерация скриптов
            app.UseMvc(routes => {
                routes.MapRoute(null, "core/gen/antibot.js", new { controller = "CoreGenToAntiBot", action = "Index" });
            });

            // Заглушка для "Core API"
            app.Map("/core", ap => ap.Run(context => context.Response.WriteAsync("404  Not Found")));
            #endregion

            #region Открытый API
            {
                // Проверка авторизации
                app.UseAuthApiMiddleware();

                #region API - List
                app.UseMvc(routes =>
                {
                    routes.MapRoute(null, "api/list/home/jurnal", new { controller = "ApiListHome", action = "Jurnal" });
                    routes.MapRoute(null, "api/list/notifications", new { controller = "ApiListNotifications", action = "Jurnal" });
                    routes.MapRoute(null, "api/list/jsondb", new { controller = "ApiListJsonDB", action = "Get" });

                    routes.MapRoute(null, "api/list/security/anti-ddos/stats/day", new { controller = "ApiListAntiDdos", action = "StatsDay" });
                    routes.MapRoute(null, "api/list/security/anti-ddos/stats/month", new { controller = "ApiListAntiDdos", action = "StatsMonth" });
                    routes.MapRoute(null, "api/list/security/anti-ddos/jurnal", new { controller = "ApiListAntiDdos", action = "Jurnal" });

                    routes.MapRoute(null, "api/list/security/iptables/blockedip", new { controller = "ApiListIptables", action = "BlockedsIP" });
                    routes.MapRoute(null, "api/list/security/av/report", new { controller = "ApiListAntiVirus", action = "Report" });

                    routes.MapRoute(null, "api/list/requests-filter/domain", new { controller = "ApiListDomain", action = "Domain" });
                    routes.MapRoute(null, "api/list/requests-filter/domains", new { controller = "ApiListDomain", action = "Domains" });
                    routes.MapRoute(null, "api/list/requests-filter/template", new { controller = "ApiListTemplate", action = "Template" });
                    routes.MapRoute(null, "api/list/requests-filter/templates", new { controller = "ApiListTemplate", action = "Templates" });
                    routes.MapRoute(null, "api/list/requests-filter/access", new { controller = "ApiListAccess", action = "Get" });

                    routes.MapRoute(null, "api/list/requests-filter/monitoring/stats/day", new { controller = "ApiListMonitoring", action = "StatsDay" });
                    routes.MapRoute(null, "api/list/requests-filter/monitoring/stats/month", new { controller = "ApiListMonitoring", action = "StatsMonth" });
                    routes.MapRoute(null, "api/list/requests-filter/monitoring/jurnal", new { controller = "ApiListMonitoring", action = "Jurnal" });

                    routes.MapRoute(null, "api/list/backup/task", new { controller = "ApiListBackup", action = "Task" });
                    routes.MapRoute(null, "api/list/backup/tasks", new { controller = "ApiListBackup", action = "Tasks" });
                    routes.MapRoute(null, "api/list/backup/operations", new { controller = "ApiListBackup", action = "Operation" });
                });
                #endregion

                #region API - Add
                app.UseMvc(routes =>
                {
                    routes.MapRoute(null, "api/add/whitelist", new { controller = "ApiAddWhiteList", action = "Base" });
                    routes.MapRoute(null, "api/add/security/iptables", new { controller = "ApiAddIptables", action = "Base" });

                    routes.MapRoute(null, "api/add/backup/task", new { controller = "ApiAddBackup", action = "Task" });
                    routes.MapRoute(null, "api/add/backup/ignore", new { controller = "ApiAddBackup", action = "Ignore" });

                    routes.MapRoute(null, "api/add/requests-filter/template", new { controller = "ApiAddTemplate", action = "Base" });
                    routes.MapRoute(null, "api/add/requests-filter/template/rule", new { controller = "ApiAddRule", action = "RuleTemplate" });

                    routes.MapRoute(null, "api/add/requests-filter/domain", new { controller = "ApiAddDomain", action = "Domain" });
                    routes.MapRoute(null, "api/add/requests-filter/aliases", new { controller = "ApiAddDomain", action = "Aliases" });
                    routes.MapRoute(null, "api/add/requests-filter/domain/rule", new { controller = "ApiAddRule", action = "RuleDomain" });
                    routes.MapRoute(null, "api/add/requests-filter/domain/template", new { controller = "ApiAddDomain", action = "TemplatesId" });
                    routes.MapRoute(null, "api/add/requests-filter/domain/ignore", new { controller = "ApiAddDomain", action = "IgnoreLogs" });
                    routes.MapRoute(null, "api/add/requests-filter/access", new { controller = "ApiAddAccess", action = "Base" });
                });
                #endregion

                #region API - Remove
                app.UseMvc(routes =>
                {
                    routes.MapRoute(null, "api/remove/whitelist", new { controller = "ApiRemoveWhiteList", action = "Base" });
                    routes.MapRoute(null, "api/remove/security/iptables", new { controller = "ApiRemoveIptables", action = "BlockedsIP" });
                    routes.MapRoute(null, "api/remove/security/antivirus", new { controller = "ApiRemoveAntivirus", action = "Base" });

                    routes.MapRoute(null, "api/remove/backup/task", new { controller = "ApiRemoveBackup", action = "Task" });
                    routes.MapRoute(null, "api/remove/backup/ignore", new { controller = "ApiRemoveBackup", action = "Ignore" });

                    routes.MapRoute(null, "api/remove/requests-filter/rules/base", new { controller = "ApiRemoveRules", action = "Rule" });
                    routes.MapRoute(null, "api/remove/requests-filter/rules/replace", new { controller = "ApiRemoveRules", action = "RuleReplace" });
                    routes.MapRoute(null, "api/remove/requests-filter/rules/override", new { controller = "ApiRemoveRules", action = "RuleOverride" });
                    routes.MapRoute(null, "api/remove/requests-filter/rules/arg", new { controller = "ApiRemoveRules", action = "RuleArg" });
                    routes.MapRoute(null, "api/remove/requests-filter/alias", new { controller = "ApiRemoveDomain", action = "Alias" });
                    routes.MapRoute(null, "api/remove/requests-filter/domain", new { controller = "ApiRemoveDomain", action = "Base" });
                    routes.MapRoute(null, "api/remove/requests-filter/domain/template", new { controller = "ApiRemoveDomain", action = "Template" });
                    routes.MapRoute(null, "api/remove/requests-filter/domain/ignore", new { controller = "ApiRemoveDomain", action = "Ignore" });
                    routes.MapRoute(null, "api/remove/requests-filter/template", new { controller = "ApiRemoveTemplate", action = "Base" });
                    routes.MapRoute(null, "api/remove/requests-filter/access", new { controller = "ApiRemoveAccess", action = "Base" });
                });
                #endregion

                #region API - Common
                app.UseMvc(routes =>
                {
                    routes.MapRoute(null, "api/common/av/start", new { controller = "ApiCommonAV", action = "Start" });
                    routes.MapRoute(null, "api/common/av/stop", new { controller = "ApiCommonAV", action = "Stop" });

                    routes.MapRoute(null, "api/common/template/export", new { controller = "ApiCommonTemplate", action = "Export" });
                    routes.MapRoute(null, "api/common/template/import", new { controller = "ApiCommonTemplate", action = "Import" });

                    routes.MapRoute(null, "api/common/backup/clearing/cache", new { controller = "ApiCommonBackup", action = "ClearingCache" });
                    routes.MapRoute(null, "api/common/backup/recovery", new { controller = "ApiCommonBackup", action = "Recovery" });
                });
                #endregion

                #region API - Edit
                app.UseMvc(routes =>
                {
                    routes.MapRoute(null, "api/edit/settings/base", new { controller = "ApiEditSettings", action = "Base" });
                    routes.MapRoute(null, "api/edit/settings/api", new { controller = "ApiEditSettings", action = "API" });
                    routes.MapRoute(null, "api/edit/settings/security", new { controller = "ApiEditSettings", action = "Security" });
                    routes.MapRoute(null, "api/edit/settings/passwd", new { controller = "ApiEditSettings", action = "Passwd" });
                    routes.MapRoute(null, "api/edit/settings/brute-force", new { controller = "ApiEditSettings", action = "BruteForce" });
                    routes.MapRoute(null, "api/edit/settings/telegrambot", new { controller = "ApiEditSettings", action = "TelegramBot" });

                    routes.MapRoute(null, "api/edit/settings/anti-ddos", new { controller = "ApiEditSettings", action = "AntiDdos" });
                    routes.MapRoute(null, "api/edit/settings/antivirus", new { controller = "ApiEditSettings", action = "AntiVirus" });

                    routes.MapRoute(null, "api/edit/antibot/base", new { controller = "ApiEditAntiBot", action = "Base" });
                    routes.MapRoute(null, "api/edit/antibot/limit", new { controller = "ApiEditAntiBot", action = "Limit" });

                    routes.MapRoute(null, "api/edit/domain/base", new { controller = "ApiEditDomain", action = "Base" });
                    routes.MapRoute(null, "api/edit/domain/log", new { controller = "ApiEditDomain", action = "LogSettings" });
                    routes.MapRoute(null, "api/edit/domain/av", new { controller = "ApiEditDomain", action = "AntiVirus" });
                    routes.MapRoute(null, "api/edit/domain/antibot", new { controller = "ApiEditDomain", action = "AntiBot" });
                    routes.MapRoute(null, "api/edit/domain/limit/request", new { controller = "ApiEditDomain", action = "LimitRequest" });

                    routes.MapRoute(null, "api/edit/template", new { controller = "ApiEditTemplate", action = "Base" });

                    routes.MapRoute(null, "api/edit/backup/task", new { controller = "ApiEditBackup", action = "Task" });
                    routes.MapRoute(null, "api/edit/backup/ftp", new { controller = "ApiEditBackup", action = "FTP" });
                    routes.MapRoute(null, "api/edit/backup/webdav", new { controller = "ApiEditBackup", action = "WebDav" });
                    routes.MapRoute(null, "api/edit/backup/onedrive", new { controller = "ApiEditBackup", action = "OneDrive" });
                });
                #endregion
            }
            #endregion
            
            // Страница авторизации
            app.UseMvc(routes => {
                routes.MapRoute(null, "auth", new { controller = "Auth", action = "Index" });
                routes.MapRoute(null, "auth/unlock", new { controller = "Auth", action = "Unlock" });
            });

            // Проверка авторизации
            app.UseAuthMiddleware();

            // Главная страница
            app.UseMvc(routes => {
                routes.MapRoute(null, "", new { controller = "Home", action = "Index" });
            });

            #region API
            // FAQ
            app.UseMvc(routes => routes.MapRoute(null, "api/faq", new { controller = "ApiFaq", action = "Index" }));

            // Заглушка для "API"
            app.Map("/api", ap => ap.Run(context => context.Response.WriteAsync("404  Not Found")));
            #endregion

            #region Настройки
            app.UseMvc(routes => {
                routes.MapRoute(null, "settings", new { controller = "Settings", action = "Index" });
                routes.MapRoute(null, "settings/save/base", new { controller = "Settings", action = "Save" });
                routes.MapRoute(null, "settings/remove/whiteList", new { controller = "Settings", action = "RemoveWhiteList" });
            });
            #endregion

            #region Безопастность системы
            // Брандмауэр
            app.UseMvc(routes => {
                routes.MapRoute(null, "security/iptables", new { controller = "SecurityToIPtables", action = "Index" });
                routes.MapRoute(null, "security/iptables/remove", new { controller = "SecurityToIPtables", action = "Remove" });
                routes.MapRoute(null, "security/iptables/add", new { controller = "SecurityToIPtables", action = "Add" });
            });

            // Антивирус
            app.UseMvc(routes => {
                routes.MapRoute(null, "security/antivirus", new { controller = "SecurityToAntiVirus", action = "Index" });
                routes.MapRoute(null, "security/antivirus/save", new { controller = "SecurityToAntiVirus", action = "Save" });
                routes.MapRoute(null, "security/antivirus/remove", new { controller = "SecurityToAntiVirus", action = "Remove" });
                routes.MapRoute(null, "security/antivirus/start", new { controller = "SecurityToAntiVirus", action = "Start" });
                routes.MapRoute(null, "security/antivirus/stop", new { controller = "SecurityToAntiVirus", action = "Stop" });
            });

            // Антидосс
            if (Platform.Get == PlatformOS.Unix)
            {
                app.UseMvc(routes =>
                {
                    routes.MapRoute(null, "security/anti-ddos", new { controller = "SecurityToAntiDdos", action = "Index" });
                    routes.MapRoute(null, "security/anti-ddos/save", new { controller = "SecurityToAntiDdos", action = "Save" });
                });
            }

            // Anti-Bot
            app.UseMvc(routes =>
            {
                routes.MapRoute(null, "security/antibot", new { controller = "SecurityToAntiBot", action = "Index" });
                routes.MapRoute(null, "security/antibot/save", new { controller = "SecurityToAntiBot", action = "Save" });
            });
            #endregion

            #region Фильтрация запросов
            // Views
            app.UseMvc(routes => {
                routes.MapRoute(null, "requests-filter/domains", new { controller = "RequestsFilterToDomains", action = "Index" });
                routes.MapRoute(null, "requests-filter/templates", new { controller = "RequestsFilterToTemplates", action = "Index" });
                routes.MapRoute(null, "requests-filter/monitoring", new { controller = "RequestsFilterToMonitoring", action = "Index" });
            });

            // Common
            app.UseMvc(routes => {
                routes.MapRoute(null, "requests-filter/common/remove/rule", new { controller = "RequestsFilterToCommon", action = "RemoveToRule" });
                routes.MapRoute(null, "requests-filter/common/remove/rulereplace", new { controller = "RequestsFilterToCommon", action = "RemoveToRuleReplace" });
                routes.MapRoute(null, "requests-filter/common/remove/ruleoverride", new { controller = "RequestsFilterToCommon", action = "RemoveToRuleOverride" });
                routes.MapRoute(null, "requests-filter/common/remove/rulearg", new { controller = "RequestsFilterToCommon", action = "RemoveToRuleArg" });
                routes.MapRoute(null, "requests-filter/common/remove/alias", new { controller = "RequestsFilterToCommon", action = "RemoveToAlias" });
            });

            // Шаблон
            app.UseMvc(routes => {
                routes.MapRoute(null, "requests-filter/template", new { controller = "RequestsFilterToTemplate", action = "Index" });
                routes.MapRoute(null, "requests-filter/template/save", new { controller = "RequestsFilterToTemplate", action = "Save" });
                routes.MapRoute(null, "requests-filter/template/remove", new { controller = "RequestsFilterToTemplate", action = "Remove" });
                routes.MapRoute(null, "requests-filter/template/import", new { controller = "RequestsFilterToTemplate", action = "Import" });
                routes.MapRoute(null, "requests-filter/template/export", new { controller = "RequestsFilterToTemplate", action = "Export" });
            });

            // Разрешенные доступы
            app.UseMvc(routes => {
                routes.MapRoute(null, "requests-filter/access", new { controller = "RequestsFilterToAccess", action = "Index" });
                routes.MapRoute(null, "requests-filter/access/open", new { controller = "RequestsFilterToAccess", action = "Open" });
                routes.MapRoute(null, "requests-filter/access/remove", new { controller = "RequestsFilterToAccess", action = "Remove" });
            });
            #endregion

            #region Фильтрация запросов - Домен
            // Домен - Главная/FAQ
            app.UseMvc(routes => {
                routes.MapRoute(null, "requests-filter/domain/base", new { controller = "RequestsFilterToDomainBase", action = "Index" });
                routes.MapRoute(null, "requests-filter/domain/faq", new { controller = "RequestsFilterToDomainBase", action = "Faq" });
                routes.MapRoute(null, "requests-filter/domain/remove", new { controller = "RequestsFilterToDomainBase", action = "Remove" });
                routes.MapRoute(null, "requests-filter/domain/save/base", new { controller = "RequestsFilterToDomainBase", action = "Save" });
                routes.MapRoute(null, "requests-filter/domain/import", new { controller = "RequestsFilterToDomainBase", action = "Import" });
                routes.MapRoute(null, "requests-filter/domain/export", new { controller = "RequestsFilterToDomainBase", action = "Export" });
            });

            // Домен - Алиасы
            app.UseMvc(routes => {
                routes.MapRoute(null, "requests-filter/domain/aliases", new { controller = "RequestsFilterToDomainAliases", action = "Index" });
                routes.MapRoute(null, "requests-filter/domain/save/aliases", new { controller = "RequestsFilterToDomainAliases", action = "Save" });
            });

            // Домен - Правила
            app.UseMvc(routes => {
                routes.MapRoute(null, "requests-filter/domain/rules", new { controller = "RequestsFilterToDomainRules", action = "Index" });
                routes.MapRoute(null, "requests-filter/domain/save/rules", new { controller = "RequestsFilterToDomainRules", action = "Save" });
            });

            // Домен - Настройки журнала
            app.UseMvc(routes => {
                routes.MapRoute(null, "requests-filter/domain/logsettings", new { controller = "RequestsFilterToDomainLogSettings", action = "Index" });
                routes.MapRoute(null, "requests-filter/domain/save/logsettings", new { controller = "RequestsFilterToDomainLogSettings", action = "Save" });
            });

            // Домен - Антивирус
            app.UseMvc(routes => {
                routes.MapRoute(null, "requests-filter/domain/av", new { controller = "RequestsFilterToDomainAv", action = "Index" });
                routes.MapRoute(null, "requests-filter/domain/save/av", new { controller = "RequestsFilterToDomainAv", action = "Save" });
            });

            // Домен - AntiBot
            app.UseMvc(routes => {
                routes.MapRoute(null, "requests-filter/domain/antibot", new { controller = "RequestsFilterToDomainAntiBot", action = "Index" });
                routes.MapRoute(null, "requests-filter/domain/save/antibot", new { controller = "RequestsFilterToDomainAntiBot", action = "Save" });
            });

            // Домен - Лимит запросов
            app.UseMvc(routes => {
                routes.MapRoute(null, "requests-filter/domain/limitrequest", new { controller = "RequestsFilterToDomainLimitRequest", action = "Index" });
                routes.MapRoute(null, "requests-filter/domain/save/limitrequest", new { controller = "RequestsFilterToDomainLimitRequest", action = "Save" });
            });
            #endregion

            #region SyncBackup
            // Views
            app.UseMvc(routes =>
            {
                routes.MapRoute(null, "backup/tasks", new { controller = "SyncBackupToTasks", action = "Index" });
                routes.MapRoute(null, "backup/operation", new { controller = "SyncBackupToOperation", action = "Index" });
            });

            // Задание
            app.UseMvc(routes =>
            {
                routes.MapRoute(null, "backup/task", new { controller = "SyncBackupToTask", action = "Index" });
                routes.MapRoute(null, "backup/task/remove", new { controller = "SyncBackupToTask", action = "Remove" });
                routes.MapRoute(null, "backup/task/save", new { controller = "SyncBackupToTask", action = "Save" });
                routes.MapRoute(null, "backup/task/clearing-cache", new { controller = "SyncBackupToTask", action = "ClearingCache" });
            });

            // Улиты
            app.UseMvc(routes =>
            {
                routes.MapRoute(null, "backup/tools", new { controller = "SyncBackupToTools", action = "Index" });
                routes.MapRoute(null, "backup/tools/recover", new { controller = "SyncBackupToTools", action = "Recovery" });
            });

            // Получение токенов
            app.UseMvc(routes =>
            {
                routes.MapRoute(null, "backup/authorize/onedrive", new { controller = "SyncBackupToAuthorize", action = "OneDrive" });
            });
            #endregion

            // Уведомления
            app.UseMvc(routes => {
                routes.MapRoute(null, "notifications", new { controller = "Notifications", action = "Index" });
            });

            // Ошибка 404
            app.Run(async (context) =>
            {
                await RewriteTo.Local(context, "404" + (context.Request.QueryString.Value.Contains("ajax=true") ? "?ajax=true" : ""));
            });
        }
    }
}
