﻿using System;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using ISPCore.Models.Databases;
using ISPCore.Models.Base;
using ISPCore.Engine.Base.SqlAndCache;
using ISPCore.Models.SyncBackup.Database;
using ISPCore.Engine.Base;
using System.Text.RegularExpressions;
using ISPCore.Models.SyncBackup.Database.Enums;
using System.IO;
using Trigger = ISPCore.Models.Triggers.Events.SyncBackup.Database;

namespace ISPCore.Engine.Cron.SyncBackup
{
    public class Database
    {
        #region Run
        static bool IsRun = false;
        public static void Run(CoreDB coreDB, IMemoryCache memoryCache)
        {
            if (IsRun)
                return;
            IsRun = true;

            #region Очистка базы - "Отчеты"
            if (memoryCache.TryGetValue("CronSyncBackupDB:ClearDB", out DateTime CronSyncBackupClearDB))
            {
                // Если дата отличается от текущей
                if (CronSyncBackupClearDB.Day != DateTime.Now.Day)
                {
                    // Меняем режим доступа к SQL
                    SqlToMode.SetMode(SqlMode.Read);

                    // Обновляем кеш
                    memoryCache.Set("CronSyncBackupDB:ClearDB", DateTime.Now);

                    // Чистим базу
                    foreach (var note in coreDB.SyncBackup_db_Reports.AsNoTracking())
                    {
                        // Если записи больше 90 дней
                        if ((DateTime.Now - note.Time).TotalDays > 90)
                        {
                            // Удаляем отчет
                            coreDB.SyncBackup_db_Reports.RemoveAttach(coreDB, note.Id);
                        }
                    }

                    // Меняем режим доступа к SQL
                    SqlToMode.SetMode(SqlMode.ReadOrWrite);

                    // Раз в сутки
                    GC.Collect(GC.MaxGeneration);
                }
            }
            else
            {
                // Создаем кеш задним числом
                memoryCache.Set("CronSyncBackupDB:ClearDB", DateTime.Now.AddDays(-1));
            }
            #endregion

            // Меняем режим доступа к SQL
            SqlToMode.SetMode(SqlMode.Read);

            // Получаем весь список заданий
            var Alltasks = coreDB.SyncBackup_db_Tasks.Include(i => i.DumpConf).Include(i => i.ConnectionConf).ToList();

            // Меняем режим доступа к SQL
            SqlToMode.SetMode(SqlMode.ReadOrWrite);

            // Проходим задания
            foreach (Task task in Alltasks)
            {
                // Пропускаем задания которые не требуют выполнения
                if (task.JobStatus != JobStatus.on || task.LastSync > DateTime.Now.AddMinutes(-task.SuncTime))
                    continue;

                // 
                Trigger.OnStartJob((task.Id, task.TypeDb));

                // Выполняем задание
                Dump(task, out bool IsOk, out string ErrorMsg);

                // Меняем режим доступа к SQL
                SqlToMode.SetMode(SqlMode.Read);

                // Добовляем задание в список завершеных операций
                coreDB.SyncBackup_db_Reports.Add(new Report()
                {
                    TaskId = task.Id,
                    Category = $"{task.TypeDb.ToString()}",
                    Msg = $"Задание: {task.Description}",
                    Time = DateTime.Now,
                    Status = IsOk ? "Задание выполнено без ошибок" : "Задание выполнено с ошибками",
                    ErrorMsg = ErrorMsg,
                });

                // Завершаем задание
                task.LastSync = DateTime.Now;
                coreDB.SaveChanges();

                // Меняем режим доступа к SQL
                SqlToMode.SetMode(SqlMode.ReadOrWrite);

                // 
                Trigger.OnStopJob((task.Id, task.TypeDb, IsOk, ErrorMsg));
            }
            
            IsRun = false;
        }
        #endregion

        #region Dump
        /// <summary>
        /// Экспорт базы
        /// </summary>
        /// <param name="task">Задание</param>
        /// <param name="IsOk"></param>
        /// <param name="ErrorMsg"></param>
        private static void Dump(Task task, out bool IsOk, out string ErrorMsg)
        {
            IsOk = true;
            ErrorMsg = null;

            // Файл логов
            string fileLog = $"{Folders.Temp.SyncBackup}/{task.TypeDb.ToString()}.dump-{DateTime.Now.ToBinary()}.log";

            #region Список баз
            string dbs = string.Empty;
            switch (task.TypeDb)
            {
                case TypeDb.MySQL:
                    dbs = new Bash().Run($"mysql -P{task.ConnectionConf.Port} -h{task.ConnectionConf.Host} -u{task.ConnectionConf.User} -p{task.ConnectionConf.Password} -N -e 'show databases' 2>{fileLog}" + " | awk '{print $1}'");
                    break;
                case TypeDb.PostgreSQL:
                    dbs = new Bash().Run($@"PGPASSWORD={task.ConnectionConf.Password} psql -p {task.ConnectionConf.Port} -h {task.ConnectionConf.Host} -U {task.ConnectionConf.User} -qAntc '\l' | cut -d\| -f1 " + " | grep -v \"=\"");
                    break;
            }
            #endregion

            // Проходим каждую базу отдельно
            foreach (string dbName in dbs.Split('\n'))
            {
                // Пустая линия
                if (string.IsNullOrWhiteSpace(dbName))
                    continue;

                // Список игнорируемых баз 
                if (task.DumpConf.IgnoreDatabases != null && task.DumpConf.IgnoreDatabases.Contains(dbName))
                    continue;

                // Список экспортируемых баз 
                if (!string.IsNullOrWhiteSpace(task.DumpConf.DumpDatabases) && !task.DumpConf.DumpDatabases.Contains(dbName))
                    continue;

                // Файл SQL
                string dumpTime = task.DumpConf.AddBackupTime ? $"_{DateTime.Now.ToString("dd.MM.yyy_HH-mm")}" : "";
                string dumpCompression = task.DumpConf.Compression == CompressionType.GZip ? ".gz" : "";
                string outSQL = $"{Regex.Replace(task.DumpConf.Whence, "/$", "")}/{dbName}{dumpTime}.sql{dumpCompression}";

                #region Dump SQL
                string bashCompression = task.DumpConf.Compression == CompressionType.GZip ? "| gzip" : "";
                switch (task.TypeDb)
                {
                    case TypeDb.MySQL:
                        new Bash().Run($"mysqldump --port={task.ConnectionConf.Port} --host={task.ConnectionConf.Host} --user={task.ConnectionConf.User} --password={task.ConnectionConf.Password} --ignore-table=mysql.event {dbName} 2>>{fileLog} {bashCompression} > {outSQL}");
                        break;
                    case TypeDb.PostgreSQL:
                        new Bash().Run($"PGPASSWORD={task.ConnectionConf.Password} pg_dump {dbName} -p {task.ConnectionConf.Port} -h {task.ConnectionConf.Host} -U {task.ConnectionConf.User} 2>>{fileLog} {bashCompression} > {outSQL}");
                        break;
                }
                #endregion
            }

            // Ошибки
            if (File.Exists(fileLog))
            {
                string error = File.ReadAllText(fileLog);
                if (!string.IsNullOrWhiteSpace(error)) {
                    IsOk = false;
                    ErrorMsg = error;
                }

                File.Delete(fileLog);
            }
        }
        #endregion
    }
}
