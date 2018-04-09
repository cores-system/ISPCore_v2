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
        public DbSet<Security.AntiDdos.Jurnal> AntiDdos_Jurnals { get; set; }
        public DbSet<Security.AntiDdos.NumberOfRequestDay> AntiDdos_NumberOfRequestDays { get; set; }
        public DbSet<Security.AntiDdos.NumberOfRequestMonth> AntiDdos_NumberOfRequestMonths { get; set; }
        #endregion

        #region Фильтрация запросов - Статистика и журнал
        public DbSet<RequestsFilter.Monitoring.Jurnal200> RequestsFilter_Jurnals200 { get; set; }
        public DbSet<RequestsFilter.Monitoring.Jurnal303> RequestsFilter_Jurnals303 { get; set; }
        public DbSet<RequestsFilter.Monitoring.Jurnal403> RequestsFilter_Jurnals403 { get; set; }
        public DbSet<RequestsFilter.Monitoring.Jurnal401> RequestsFilter_Jurnals401 { get; set; }
        public DbSet<RequestsFilter.Monitoring.Jurnal500> RequestsFilter_Jurnals500 { get; set; }
        public DbSet<RequestsFilter.Monitoring.Jurnal2FA> RequestsFilter_Jurnals2FA { get; set; }
        public DbSet<RequestsFilter.Monitoring.NumberOfRequestDay> RequestsFilter_NumberOfRequestDay { get; set; }
        public DbSet<RequestsFilter.Monitoring.NumberOfRequestMonth> RequestsFilter_NumberOfRequestMonth { get; set; }
        #endregion

        #region Фильтрация запросов - Домены/Шаблоны
        public DbSet<RequestsFilter.Templates.Template> RequestsFilter_Templates { get; set; }
        public DbSet<RequestsFilter.Templates.Rules.Rule> RequestsFilter_Template_Rules { get; set; }
        public DbSet<RequestsFilter.Templates.Rules.RuleArg> RequestsFilter_Template_RuleArgs { get; set; }
        public DbSet<RequestsFilter.Templates.Rules.RuleReplace> RequestsFilter_Template_RuleReplaces { get; set; }
        public DbSet<RequestsFilter.Templates.Rules.RuleOverride> RequestsFilter_Template_RuleOverrides { get; set; }

        public DbSet<RequestsFilter.Domains.Domain> RequestsFilter_Domains { get; set; }
        public DbSet<RequestsFilter.Domains.Rules.RuleReplace> RequestsFilter_Domain_RuleReplaces { get; set; }
        public DbSet<RequestsFilter.Domains.Rules.Rule> RequestsFilter_Domain_Rules { get; set; }
        public DbSet<RequestsFilter.Domains.Rules.RuleOverride> RequestsFilter_Domain_RuleOverrides { get; set; }
        public DbSet<RequestsFilter.Domains.Rules.RuleArg> RequestsFilter_Domain_RuleArgs { get; set; }
        public DbSet<RequestsFilter.Domains.Alias> RequestsFilter_Domain_Aliases { get; set; }
        public DbSet<RequestsFilter.Domains.TemplateId> RequestsFilter_Domain_TemplatesId { get; set; }
        public DbSet<RequestsFilter.Domains.Log.IgnoreToLog> RequestsFilter_Domain_IgnoreToLogs { get; set; }
        public DbSet<RequestsFilter.Domains.Log.ConfToLog> RequestsFilter_Domain_ConfToLog { get; set; }
        public DbSet<RequestsFilter.Domains.AntiBot> RequestsFilter_Domain_AntiBot { get; set; }
        public DbSet<RequestsFilter.Domains.LimitRequest> RequestsFilter_Domain_LimitRequest { get; set; }
        public DbSet<RequestsFilter.Domains.AntiVirus> RequestsFilter_Domain_AntiVirus { get; set; }
        #endregion

        #region SyncBackup - Задания
        public DbSet<SyncBackup.Tasks.Task> SyncBackup_Tasks { get; set; }
        public DbSet<SyncBackup.Tasks.FTP> SyncBackup_Task_FTP { get; set; }
        public DbSet<SyncBackup.Tasks.WebDav> SyncBackup_Task_WebDav { get; set; }
        public DbSet<SyncBackup.Tasks.OneDrive> SyncBackup_Task_OneDrive { get; set; }
        public DbSet<SyncBackup.Tasks.IgnoreFileOrFolders> SyncBackup_Task_IgnoreFileOrFolders { get; set; }
        #endregion

        #region SyncBackup - Выполненые операции
        public DbSet<SyncBackup.Operation.Notation> SyncBackup_Notations { get; set; }
        public DbSet<SyncBackup.Operation.More> SyncBackup_Notation_More { get; set; }
        #endregion

        #region Уведомления
        public DbSet<Notification.Notation> Notations { get; set; }
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
        public string BlockedHost { get; set; } //new
    }
}
