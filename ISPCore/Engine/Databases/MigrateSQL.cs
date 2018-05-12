using ISPCore.Models.api;
using ISPCore.Models.Databases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ISPCore.Engine.Databases
{
    public static class MigrateSQL
    {
        public static void ISPCore()
        {
            using (CoreDB coreDB = Service.Get<CoreDB>())
            {
                // Версия базы
                if (coreDB.Version.AsNoTracking().LastOrDefault() is LatestVersion vSql)
                {
                    // Версия базы совпадает
                    if (vSql.ToString() == Startup.vSql.ToString())
                        return;


                    #region Миграция SQL
                    switch (vSql.Version)
                    {
                        #region Первая версия базы
                        case 1.9:
                            {
                                coreDB.Database.ExecuteSqlCommand("ALTER TABLE [RequestsFilter_Domain_Aliases] ADD [Folder] TEXT NULL ;");
                                vSql.Patch = 1;
                                goto case 0.0;
                            }
                        #endregion

                        #region case 0.0
                        case 0.0:
                            {
                                switch (vSql.Patch)
                                {
                                    case 1:
                                        {
                                            coreDB.Database.ExecuteSqlCommand("ALTER TABLE [RequestsFilter_Domain_ConfToLog] ADD [Jurn200] BIGINT DEFAULT 0 NOT NULL ;");
                                            goto case 2;
                                        }

                                    #region case 2
                                    case 2:
                                        {
                                            #region Журнал 200
                                            coreDB.Database.ExecuteSqlCommand(@"CREATE TABLE [RequestsFilter_Jurnals200] (
  [Id] INTEGER  NOT NULL
, [City] text NULL
, [Country] text NULL
, [FormData] text NULL
, [Host] text NULL
, [IP] text NULL
, [Method] text NULL
, [Referer] text NULL
, [Region] text NULL
, [Time] text NOT NULL
, [Uri] text NULL
, [UserAgent] text NULL
, [typeJurn] bigint  NOT NULL
, CONSTRAINT [sqlite_master_PK_RequestsFilter_Jurnals200] PRIMARY KEY ([Id])
);");
                                            #endregion

                                            // Удаляем старые базы
                                            coreDB.Database.ExecuteSqlCommand("DROP TABLE [RequestsFilter_NumberOfRequestDay];");
                                            coreDB.Database.ExecuteSqlCommand("DROP TABLE [RequestsFilter_NumberOfRequestMonth];");

                                            #region Статистика запросов - сутки
                                            coreDB.Database.ExecuteSqlCommand(@"CREATE TABLE [RequestsFilter_NumberOfRequestDay] (
  [Id] INTEGER  NOT NULL
, [Count200] bigint  NOT NULL
, [Count2FA] bigint  NOT NULL
, [Count303] bigint  NOT NULL
, [Count401] bigint  NOT NULL
, [Count403] bigint  NOT NULL
, [Count500] bigint  NOT NULL
, [Host] text NULL
, [Time] text NOT NULL
, CONSTRAINT [sqlite_master_PK_RequestsFilter_NumberOfRequestDay] PRIMARY KEY ([Id])
);");
                                            #endregion

                                            #region Статистика запросов - месяц
                                            coreDB.Database.ExecuteSqlCommand(@"CREATE TABLE [RequestsFilter_NumberOfRequestMonth] (
  [Id] INTEGER  NOT NULL
, [Count200] bigint  NOT NULL
, [Count2FA] bigint  NOT NULL
, [Count303] bigint  NOT NULL
, [Count401] bigint  NOT NULL
, [Count403] bigint  NOT NULL
, [Count500] bigint  NOT NULL
, [Time] text NOT NULL
, [allRequests] bigint  NOT NULL
, CONSTRAINT [sqlite_master_PK_RequestsFilter_NumberOfRequestMonth] PRIMARY KEY ([Id])
);");
                                            #endregion

                                            // Успех
                                            goto case 3;
                                        }
                                    #endregion

                                    #region case 3
                                    case 3:
                                        {
                                            coreDB.Database.ExecuteSqlCommand(@"CREATE TABLE [WhitePtrIPs] (
  [Id] INTEGER  NOT NULL
, [Expires] text NOT NULL
, [IPv4Or6] text NULL
, CONSTRAINT [sqlite_master_PK_WhitePtrIPs] PRIMARY KEY ([Id])
);");
                                            goto case 4;
                                        }
                                    #endregion

                                    case 4:
                                        {
                                            coreDB.Database.ExecuteSqlCommand("ALTER TABLE [RequestsFilter_Domain_LimitRequest] ADD [UseGlobalConf] BIGINT DEFAULT 0 NOT NULL ;");
                                            goto case 5;
                                        }

                                    case 5:
                                        {
                                            coreDB.Database.ExecuteSqlCommand("ALTER TABLE [RequestsFilter_Domains] ADD [Auth2faToAccess] BIGINT DEFAULT 0 NOT NULL ;");
                                            goto case 6;
                                        }

                                    #region case 6
                                    case 6:
                                        {
                                            #region Новая таблица AntiBot
                                            coreDB.Database.ExecuteSqlCommand(@"CREATE TABLE [RequestsFilter_Domain_AntiBot] (
                                                      [Id] INTEGER  NOT NULL
                                                    , [AddCodeToHtml] text NULL
                                                    , [DomainId] bigint  NOT NULL
                                                    , [UseGlobalConf] bigint  NOT NULL
                                                    , [FirstSkipToBot] bigint  NOT NULL
                                                    , [HourCacheToBot] bigint  NOT NULL
                                                    , [HourCacheToUser] bigint  NOT NULL
                                                    , [RewriteToOriginalDomain] bigint  NOT NULL
                                                    , [WaitUser] bigint  NOT NULL
                                                    , [type] bigint  NOT NULL
                                                    , CONSTRAINT [sqlite_master_PK_RequestsFilter_Domain_AntiBot] PRIMARY KEY ([Id])
                                                    , FOREIGN KEY ([DomainId]) REFERENCES [RequestsFilter_Domains] ([Id]) ON DELETE CASCADE ON UPDATE NO ACTION
                                                    );
                                                    CREATE UNIQUE INDEX [IX_RequestsFilter_Domain_AntiBot_DomainId] ON [RequestsFilter_Domain_AntiBot] ([DomainId] ASC);
                                                ");
                                            #endregion

                                            #region Удаляем поле AntiBot в таблице RequestsFilter_Domain
                                            coreDB.Database.ExecuteSqlCommand(@"CREATE TABLE [table_temp_05032018] (
                                                      [Id] INTEGER  NOT NULL
                                                    , [Protect] bigint  NOT NULL
                                                    , [StopBruteForce] bigint  NOT NULL
                                                    , [host] text NULL
                                                    , [typeBlockIP] bigint  NOT NULL
                                                    , [Auth2faToAccess] bigint DEFAULT 0  NOT NULL
                                                    , CONSTRAINT [sqlite_master_PK_RequestsFilter_Domains] PRIMARY KEY ([Id])
                                                    );

                                                    INSERT INTO [table_temp_05032018] SELECT [id], [Protect], [StopBruteForce], [host], [typeBlockIP], [Auth2faToAccess] FROM [RequestsFilter_Domains];
                                                    ALTER TABLE [RequestsFilter_Domains] RENAME TO [migrate_05032018];
                                                    ALTER TABLE [table_temp_05032018] RENAME TO [RequestsFilter_Domains];
                                                ");
                                            #endregion

                                            #region Делаем привязку к таблице AntiBot
                                            int ColumnId = 1;
                                            foreach (var domain in coreDB.RequestsFilter_Domains.AsNoTracking())
                                            {
                                                coreDB.Database.ExecuteSqlCommand($@"INSERT INTO [RequestsFilter_Domain_AntiBot]
                                                        ([Id]
                                                        ,[AddCodeToHtml]
                                                        ,[DomainId]
                                                        ,[UseGlobalConf]
                                                        ,[FirstSkipToBot]
                                                        ,[HourCacheToBot]
                                                        ,[HourCacheToUser]
                                                        ,[RewriteToOriginalDomain]
                                                        ,[WaitUser]
                                                        ,[type]) 
                                                        VALUES ({ColumnId},null,{domain.Id},0,1,216,12,1,2800,0);
                                                    ");

                                                ColumnId++;
                                            }
                                            #endregion

                                            goto case 7;
                                        }
                                    #endregion

                                    #region case 7
                                    case 7:
                                        {
                                            coreDB.Database.ExecuteSqlCommand("UPDATE SQLITE_SEQUENCE SET SEQ=200 WHERE NAME='RequestsFilter_Domains';");
                                            coreDB.Database.ExecuteSqlCommand(@"CREATE TABLE [RequestsFilter_Domain_RuleReplaces] (
                                                      [Id] INTEGER  NOT NULL
                                                    , [ContentType] text NULL
                                                    , [DomainId] bigint  NOT NULL
                                                    , [GetArgs] text NULL
                                                    , [IsActive] bigint  NOT NULL
                                                    , [PostArgs] text NULL
                                                    , [RegexWhite] text NULL
                                                    , [ResponceUri] text NULL
                                                    , [TypeResponse] bigint  NOT NULL
                                                    , [kode] text NULL
                                                    , [uri] text NULL
                                                    , CONSTRAINT [sqlite_master_PK_RequestsFilter_Domain_RuleReplaces] PRIMARY KEY ([Id])
                                                    , FOREIGN KEY ([DomainId]) REFERENCES [RequestsFilter_Domains] ([Id]) ON DELETE CASCADE ON UPDATE NO ACTION
                                                    );
                                                    CREATE INDEX [IX_RequestsFilter_Domain_RuleReplaces_DomainId] ON [RequestsFilter_Domain_RuleReplaces] ([DomainId] ASC);
                                                ");
                                            goto case 8;
                                        }
                                    #endregion

                                    #region case 8
                                    case 8:
                                        {
                                            coreDB.Database.ExecuteSqlCommand(@"CREATE TABLE [RequestsFilter_Template_RuleReplaces] (
                                                  [Id] INTEGER  NOT NULL
                                                , [ContentType] text NULL
                                                , [GetArgs] text NULL
                                                , [IsActive] bigint  NOT NULL
                                                , [PostArgs] text NULL
                                                , [RegexWhite] text NULL
                                                , [ResponceUri] text NULL
                                                , [TemplateId] bigint  NOT NULL
                                                , [TypeResponse] bigint  NOT NULL
                                                , [kode] text NULL
                                                , [uri] text NULL
                                                , CONSTRAINT [sqlite_master_PK_RequestsFilter_Template_RuleReplaces] PRIMARY KEY ([Id])
                                                , FOREIGN KEY ([TemplateId]) REFERENCES [RequestsFilter_Templates] ([Id]) ON DELETE CASCADE ON UPDATE NO ACTION
                                                );
                                                CREATE INDEX [IX_RequestsFilter_Template_RuleReplaces_TemplateId] ON [RequestsFilter_Template_RuleReplaces] ([TemplateId] ASC);
                                            ");

                                            coreDB.Database.ExecuteSqlCommand(@"CREATE TABLE [RequestsFilter_Template_RuleOverrides] (
                                                  [Id] INTEGER  NOT NULL
                                                , [IsActive] bigint  NOT NULL
                                                , [Method] bigint  NOT NULL
                                                , [TemplateId] bigint  NOT NULL
                                                , [order] bigint  NOT NULL
                                                , [rule] text NULL
                                                , CONSTRAINT [sqlite_master_PK_RequestsFilter_Template_RuleOverrides] PRIMARY KEY ([Id])
                                                , FOREIGN KEY ([TemplateId]) REFERENCES [RequestsFilter_Templates] ([Id]) ON DELETE CASCADE ON UPDATE NO ACTION
                                                );
                                                CREATE INDEX [IX_RequestsFilter_Template_RuleOverrides_TemplateId] ON [RequestsFilter_Template_RuleOverrides] ([TemplateId] ASC);
                                            ");

                                            goto case 9;
                                        }
                                    #endregion

                                    case 9:
                                        {
                                            coreDB.Database.ExecuteSqlCommand("ALTER TABLE [RequestsFilter_Domain_AntiBot] ADD [BackgroundCheck] BIGINT DEFAULT 0 NOT NULL ;");
                                            coreDB.Database.ExecuteSqlCommand("ALTER TABLE [RequestsFilter_Domain_AntiBot] ADD [CountBackgroundRequest] BIGINT DEFAULT 2 NOT NULL ;");
                                            coreDB.Database.ExecuteSqlCommand("ALTER TABLE [RequestsFilter_Domain_AntiBot] ADD [BackgroundCheckToAddExtensions] TEXT NULL ;");
                                            break;
                                        }
                                }

                                // Миграция на 0.1.*
                                vSql.Patch = 0;
                                goto case 0.1;
                            }
                        #endregion

                        case 0.1:
                            {
                                switch (vSql.Patch)
                                {
                                    case 0:
                                        {
                                            coreDB.Database.ExecuteSqlCommand("ALTER TABLE [RequestsFilter_Domain_AntiBot] ADD [BackgroundHourCacheToIP] BIGINT DEFAULT 0 NOT NULL ;");
                                            goto case 1;
                                        }

                                    case 1:
                                        {
                                            coreDB.Database.ExecuteSqlCommand("ALTER TABLE [RequestsFilter_Domain_LimitRequest] ADD [BlockType] BIGINT DEFAULT 0 NOT NULL ;");
                                            coreDB.Database.ExecuteSqlCommand("ALTER TABLE [RequestsFilter_Domain_LimitRequest] ADD [MaxRequestToAgainСheckingreCAPTCHA] BIGINT DEFAULT 300 NOT NULL ;");
                                            coreDB.Database.ExecuteSqlCommand("ALTER TABLE [RequestsFilter_Domains] ADD [Auth2faToPasswd] TEXT NULL ;");
                                            goto case 2;
                                        }

                                    case 2:
                                        {
                                            coreDB.Database.ExecuteSqlCommand(@"CREATE TABLE [Auth_Sessions] (
                                                  [Id] INTEGER  NOT NULL
                                                , [Confirm2FA] bigint  NOT NULL
                                                , [Expires] text NOT NULL
                                                , [HashPasswdToRoot] text NULL
                                                , [IP] text NULL
                                                , [Session] text NULL
                                                , CONSTRAINT [sqlite_master_PK_Auth_Sessions] PRIMARY KEY ([Id])
                                                );
                                            ");
                                            goto case 3;
                                        }

                                    case 3:
                                        {
                                            coreDB.Database.ExecuteSqlCommand("ALTER TABLE [Auth_Sessions] ADD [CreateTime] TEXT DEFAULT [2018-04-24 10:57:30.9735464] NOT NULL ;");
                                            goto case 4;
                                        }

                                    #region case 4
                                    case 4:
                                        {
                                            coreDB.Database.ExecuteSqlCommand(@"CREATE TABLE [SyncBackup_db_Tasks] (
                                                  [Id] INTEGER  NOT NULL
                                                , [Description] text NULL
                                                , [JobStatus] bigint  NOT NULL
                                                , [LastSync] text NOT NULL
                                                , [SuncTime] bigint  NOT NULL
                                                , [TypeDb] bigint  NOT NULL
                                                , CONSTRAINT [sqlite_master_PK_SyncBackup_db_Tasks] PRIMARY KEY ([Id])
                                                );
                                            ");

                                            coreDB.Database.ExecuteSqlCommand(@"CREATE TABLE [SyncBackup_db_Task_Conf] (
                                                  [Id] INTEGER  NOT NULL
                                                , [AddBackupTime] bigint  NOT NULL
                                                , [Compression] bigint  NOT NULL
                                                , [DumpDatabases] text NULL
                                                , [IgnoreDatabases] text NULL
                                                , [TaskId] bigint  NOT NULL
                                                , [Whence] text NULL
                                                , CONSTRAINT [sqlite_master_PK_SyncBackup_db_Task_Conf] PRIMARY KEY ([Id])
                                                , FOREIGN KEY ([TaskId]) REFERENCES [SyncBackup_db_Tasks] ([Id]) ON DELETE CASCADE ON UPDATE NO ACTION
                                                );
                                                CREATE UNIQUE INDEX [IX_SyncBackup_db_Task_Conf_TaskId] ON [SyncBackup_db_Task_Conf] ([TaskId] ASC);
                                            ");

                                            coreDB.Database.ExecuteSqlCommand(@"CREATE TABLE [SyncBackup_db_Task_MySQL] (
                                                  [Id] INTEGER  NOT NULL
                                                , [Host] text NULL
                                                , [Password] text NULL
                                                , [Port] bigint  NOT NULL
                                                , [TaskId] bigint  NOT NULL
                                                , [User] text NULL
                                                , CONSTRAINT [sqlite_master_PK_SyncBackup_db_Task_MySQL] PRIMARY KEY ([Id])
                                                , FOREIGN KEY ([TaskId]) REFERENCES [SyncBackup_db_Tasks] ([Id]) ON DELETE CASCADE ON UPDATE NO ACTION
                                                );
                                                CREATE UNIQUE INDEX [IX_SyncBackup_db_Task_MySQL_TaskId] ON [SyncBackup_db_Task_MySQL] ([TaskId] ASC);
                                            ");

                                            coreDB.Database.ExecuteSqlCommand(@"CREATE TABLE [SyncBackup_db_Reports] (
                                                  [Id] INTEGER  NOT NULL
                                                , [Category] text NULL
                                                , [ErrorMsg] text NULL
                                                , [Msg] text NULL
                                                , [Status] text NULL
                                                , [TaskId] bigint  NOT NULL
                                                , [Time] text NOT NULL
                                                , CONSTRAINT [sqlite_master_PK_SyncBackup_db_Reports] PRIMARY KEY ([Id])
                                                );
                                            ");

                                            goto case 5;
                                        }
                                    #endregion

                                    case 5:
                                        {
                                            coreDB.Database.ExecuteSqlCommand("ALTER TABLE SyncBackup_db_Task_MySQL RENAME TO SyncBackup_db_Task_ConnectionConf;");
                                            coreDB.Database.ExecuteSqlCommand("ALTER TABLE SyncBackup_db_Task_Conf RENAME TO SyncBackup_db_Task_DumpConf;");
                                            goto case 6;
                                        }

                                    case 6:
                                        {
                                            coreDB.Database.ExecuteSqlCommand("ALTER TABLE [WhitePtrIPs] ADD [PTR] TEXT NULL ;");
                                            goto case 7;
                                        }

                                    case 7:
                                        {
                                            coreDB.Database.ExecuteSqlCommand("ALTER TABLE [RequestsFilter_NumberOfRequestDay] ADD [CountIPtables] BIGINT DEFAULT 0 NOT NULL ;");
                                            coreDB.Database.ExecuteSqlCommand("ALTER TABLE [RequestsFilter_NumberOfRequestMonth] ADD [CountIPtables] BIGINT DEFAULT 0 NOT NULL ;");
                                            coreDB.Database.ExecuteSqlCommand("ALTER TABLE [RequestsFilter_Domain_AntiBot] ADD [HashKey] TEXT NULL ;");
                                            goto case 8;
                                        }

                                    case 8:
                                        {
                                            // Миграция на 9
                                            //goto case 9;
                                            break;
                                        }
                                }
                                break;
                            }
                    }
                    #endregion

                    // Сохраняем версию базы
                    coreDB.Version.Add(Startup.vSql);
                    coreDB.SaveChanges();
                }
            }
        }
    }
}
