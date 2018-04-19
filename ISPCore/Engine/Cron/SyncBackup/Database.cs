using System;
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
                        // Если записи больше 7 дней
                        if ((DateTime.Now - note.Time).TotalDays > 7)
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
            var Alltasks = coreDB.SyncBackup_db_Tasks.Include(i => i.Conf).Include(i => i.MySQL).ToList();

            // Меняем режим доступа к SQL
            SqlToMode.SetMode(SqlMode.ReadOrWrite);

            // Проходим задания
            foreach (Task task in Alltasks)
            {
                // Пропускаем задания которые не требуют выполнения
                if (task.JobStatus != JobStatus.on || task.LastSync > DateTime.Now.AddMinutes(-task.SuncTime))
                    continue;

                // Выполняем задание
                Dump(task, out bool IsOk, out string ErrorMsg);

                // Меняем режим доступа к SQL
                SqlToMode.SetMode(SqlMode.Read);

                // Добовляем задание в список завершеных операций
                coreDB.SyncBackup_db_Reports.Add(new Report()
                {
                    TaskId = task.Id,
                    Category = $"{task.TypeDb.ToString()} Dump",
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
            string fileLog = $"{Folders.Temp.SyncBackup}/MysqlDump-{DateTime.Now.ToBinary()}.log";

            // Список баз
            string dbs = new Bash().Run($"mysql -P{task.MySQL.Port} -h{task.MySQL.Host} -u{task.MySQL.User} -p{task.MySQL.Password} -N -e 'show databases' 2>{fileLog}" + " | awk '{print $1}'");

            // Проходим каждую базу отдельно
            foreach (string dbName in dbs.Split('\n'))
            {
                // Пустая линия
                if (string.IsNullOrWhiteSpace(dbName))
                    continue;

                // Список игнорируемых баз 
                if (task.Conf.IgnoreDatabases != null && task.Conf.IgnoreDatabases.Contains(dbName))
                    continue;

                // Список экспортируемых баз 
                if (!string.IsNullOrWhiteSpace(task.Conf.DumpDatabases) && !task.Conf.DumpDatabases.Contains(dbName))
                    continue;

                // Файл SQL
                string dumpTime = task.Conf.AddBackupTime ? $"_{DateTime.Now.ToString("dd.MM.yyy_HH-mm")}" : "";
                string dumpCompression = task.Conf.Compression == CompressionType.GZip ? ".gz" : "";
                string outSQL = $"{Regex.Replace(task.Conf.Whence, "/$", "")}/{dbName}{dumpTime}.sql{dumpCompression}";

                // Dump SQL
                string bashCompression = task.Conf.Compression == CompressionType.GZip ? "| gzip" : "";
                new Bash().Run($"mysqldump --port={task.MySQL.Port} --host={task.MySQL.Host} --user={task.MySQL.User} --password={task.MySQL.Password} {dbName} 2>>{fileLog} {bashCompression} > {outSQL}");
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
