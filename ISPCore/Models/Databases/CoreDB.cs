using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using ISPCore.Engine.Base;
using System.IO;
using ISPCore.Models.api;

namespace ISPCore.Models.Databases
{
    public class CoreDB : DbContext
    {
        /// <summary>
        /// Показывает текущие операции в SyncBackup
        /// </summary>
        public static BlockingCollection<SyncBackup.Operation.Notation> SyncBackupWorkNote = new BlockingCollection<SyncBackup.Operation.Notation>();

        /// <summary>
        /// Версия базы
        /// </summary>
        public DbSet<LatestVersion> Version { get; set; }

        /// <summary>
        /// Сессии авторизованных пользователей
        /// </summary>
        public DbSet<Auth.AuthSession> Auth_Sessions { get; set; }

        /// <summary>
        /// Журнал авторизаций в панель
        /// </summary>
        public DbSet<Home.Jurnal> Home_Jurnals { get; set; }

        /// <summary>
        /// Список заблокированных IP
        /// </summary>
        public DbSet<BlockedIP> BlockedsIP { get; set; }

        /// <summary>
        /// Список белых IP-адресов
        /// </summary>
        public DbSet<Models.Base.WhitePtrIP> WhitePtrIPs { get; set; }

        #region Безопасность системы - AntiDdos
        /// <summary>
        /// Журнал блокировок
        /// </summary>
        public DbSet<Security.AntiDdos.Jurnal> AntiDdos_Jurnals { get; set; }

        /// <summary>
        /// Статистика за сутки
        /// </summary>
        public DbSet<Security.AntiDdos.NumberOfRequestDay> AntiDdos_NumberOfRequestDays { get; set; }

        /// <summary>
        /// Статистика за месяц
        /// </summary>
        public DbSet<Security.AntiDdos.NumberOfRequestMonth> AntiDdos_NumberOfRequestMonths { get; set; }
        #endregion

        #region Фильтрация запросов - Статистика и журнал
        /// <summary>
        /// Журнал 200
        /// </summary>
        public DbSet<RequestsFilter.Monitoring.Jurnal200> RequestsFilter_Jurnals200 { get; set; }

        /// <summary>
        /// Журнал 303
        /// </summary>
        public DbSet<RequestsFilter.Monitoring.Jurnal303> RequestsFilter_Jurnals303 { get; set; }

        /// <summary>
        /// Журнал 403
        /// </summary>
        public DbSet<RequestsFilter.Monitoring.Jurnal403> RequestsFilter_Jurnals403 { get; set; }

        /// <summary>
        /// Журнал блокировок
        /// </summary>
        public DbSet<RequestsFilter.Monitoring.Jurnal401> RequestsFilter_Jurnals401 { get; set; }

        /// <summary>
        /// Журнал ошибок
        /// </summary>
        public DbSet<RequestsFilter.Monitoring.Jurnal500> RequestsFilter_Jurnals500 { get; set; }

        /// <summary>
        /// Журнал авторизаций
        /// </summary>
        public DbSet<RequestsFilter.Monitoring.Jurnal2FA> RequestsFilter_Jurnals2FA { get; set; }

        /// <summary>
        /// Статистика за сутки
        /// </summary>
        public DbSet<RequestsFilter.Monitoring.NumberOfRequestDay> RequestsFilter_NumberOfRequestDay { get; set; }

        /// <summary>
        /// Статистика за месяц
        /// </summary>
        public DbSet<RequestsFilter.Monitoring.NumberOfRequestMonth> RequestsFilter_NumberOfRequestMonth { get; set; }
        #endregion

        #region Фильтрация запросов - Шаблоны
        /// <summary>
        /// Шаблоны
        /// </summary>
        public DbSet<RequestsFilter.Templates.Template> RequestsFilter_Templates { get; set; }

        /// <summary>
        /// Правила шаблона
        /// </summary>
        public DbSet<RequestsFilter.Templates.Rules.Rule> RequestsFilter_Template_Rules { get; set; }

        /// <summary>
        /// Правила шаблона
        /// </summary>
        public DbSet<RequestsFilter.Templates.Rules.RuleArg> RequestsFilter_Template_RuleArgs { get; set; }

        /// <summary>
        /// Правила шаблона
        /// </summary>
        public DbSet<RequestsFilter.Templates.Rules.RuleReplace> RequestsFilter_Template_RuleReplaces { get; set; }

        /// <summary>
        /// Правила шаблона
        /// </summary>
        public DbSet<RequestsFilter.Templates.Rules.RuleOverride> RequestsFilter_Template_RuleOverrides { get; set; }
        #endregion

        #region Фильтрация запросов - Домены
        /// <summary>
        /// Домены
        /// </summary>
        public DbSet<RequestsFilter.Domains.Domain> RequestsFilter_Domains { get; set; }

        /// <summary>
        /// Правила домена
        /// </summary>
        public DbSet<RequestsFilter.Domains.Rules.RuleReplace> RequestsFilter_Domain_RuleReplaces { get; set; }

        /// <summary>
        /// Правила домена
        /// </summary>
        public DbSet<RequestsFilter.Domains.Rules.Rule> RequestsFilter_Domain_Rules { get; set; }

        /// <summary>
        /// Правила домена
        /// </summary>
        public DbSet<RequestsFilter.Domains.Rules.RuleOverride> RequestsFilter_Domain_RuleOverrides { get; set; }

        /// <summary>
        /// Правила домена
        /// </summary>
        public DbSet<RequestsFilter.Domains.Rules.RuleArg> RequestsFilter_Domain_RuleArgs { get; set; }

        /// <summary>
        /// Алиасы домена
        /// </summary>
        public DbSet<RequestsFilter.Domains.Alias> RequestsFilter_Domain_Aliases { get; set; }

        /// <summary>
        /// Настройки - "Журнал запросов"
        /// </summary>
        public DbSet<RequestsFilter.Domains.TemplateId> RequestsFilter_Domain_TemplatesId { get; set; }

        /// <summary>
        /// Настройки - "Журнал запросов"
        /// </summary>
        public DbSet<RequestsFilter.Domains.Log.IgnoreToLog> RequestsFilter_Domain_IgnoreToLogs { get; set; }

        /// <summary>
        /// Настройки - "AntiBot"
        /// </summary>
        public DbSet<RequestsFilter.Domains.Log.ConfToLog> RequestsFilter_Domain_ConfToLog { get; set; }

        /// <summary>
        /// Настройки - "AntiBot"
        /// </summary>
        public DbSet<RequestsFilter.Domains.AntiBot> RequestsFilter_Domain_AntiBot { get; set; }

        /// <summary>
        /// Настройки - "Лимит запросов"
        /// </summary>
        public DbSet<RequestsFilter.Domains.LimitRequest> RequestsFilter_Domain_LimitRequest { get; set; }

        /// <summary>
        /// Настройки - "АВ"
        /// </summary>
        public DbSet<RequestsFilter.Domains.AntiVirus> RequestsFilter_Domain_AntiVirus { get; set; }
        #endregion

        #region SyncBackup - IO
        /// <summary>
        /// Задания
        /// </summary>
        public DbSet<SyncBackup.Tasks.Task> SyncBackup_Tasks { get; set; }

        /// <summary>
        /// Настройки подключения к FTP
        /// </summary>
        public DbSet<SyncBackup.Tasks.FTP> SyncBackup_Task_FTP { get; set; }

        /// <summary>
        /// Настройки подключения к WebDav
        /// </summary>
        public DbSet<SyncBackup.Tasks.WebDav> SyncBackup_Task_WebDav { get; set; }

        /// <summary>
        /// Настройки подключения к OneDrive
        /// </summary>
        public DbSet<SyncBackup.Tasks.OneDrive> SyncBackup_Task_OneDrive { get; set; }

        /// <summary>
        /// Список файлов/папок которые не нужно бекапить
        /// </summary>
        public DbSet<SyncBackup.Tasks.IgnoreFileOrFolders> SyncBackup_Task_IgnoreFileOrFolders { get; set; }

        /// <summary>
        /// Отчеты
        /// </summary>
        public DbSet<SyncBackup.Operation.Notation> SyncBackup_Notations { get; set; }

        /// <summary>
        /// Дополнительная информация
        /// </summary>
        public DbSet<SyncBackup.Operation.More> SyncBackup_Notation_More { get; set; }
        #endregion

        #region SyncBackup - Database
        /// <summary>
        /// Задания
        /// </summary>
        public DbSet<SyncBackup.Database.Task> SyncBackup_db_Tasks { get; set; }

        /// <summary>
        /// Настройки экспорта
        /// </summary>
        public DbSet<SyncBackup.Database.DumpConf> SyncBackup_db_Task_DumpConf { get; set; }

        /// <summary>
        /// Настройки подключений
        /// </summary>
        public DbSet<SyncBackup.Database.ConnectionConf> SyncBackup_db_Task_ConnectionConf { get; set; }

        /// <summary>
        /// Отчеты
        /// </summary>
        public DbSet<SyncBackup.Database.Report> SyncBackup_db_Reports { get; set; }
        #endregion

        #region Уведомления
        /// <summary>
        /// Журнал уведомлений
        /// </summary>
        public DbSet<Notification.Notation> Notations { get; set; }

        /// <summary>
        /// Дополнительная информация
        /// </summary>
        public DbSet<Notification.More> Notation_More { get; set; }
        #endregion

        static bool IsFirstRun = true;
        public CoreDB(DbContextOptions<CoreDB> options) : base(options)
        {
            if (IsFirstRun && !File.Exists(Folders.File.ISPCoreDB)) {
                Database.EnsureCreated();
                Version.Add(Startup.vSql);
                this.SaveChanges();
            }

            IsFirstRun = false;
        }
    }


    /// <summary>
    /// Независимая таблица
    /// Список заблокированных IP
    /// </summary>
    public class BlockedIP
    {
        public int Id { get; set; }

        /// <summary>
        /// IP пользователя
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        ///  Причина блокировки
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// До какого времени заблокирован
        /// </summary>
        public DateTime BlockingTime { get; set; }

        /// <summary>
        /// IP заблокирован глобально или только для домена
        /// </summary>
        public RequestsFilter.Domains.TypeBlockIP typeBlockIP { get; set; }

        /// <summary>
        /// Заблокированный домен
        /// </summary>
        public string BlockedHost { get; set; }
    }
}
