using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ISPCore.Engine.Base;
using ISPCore.Models.Databases;
using ISPCore.Models.Base;
using System.Text;
using System.IO;
using ISPCore.Engine.Base.SqlAndCache;
using Trigger = ISPCore.Models.Triggers.Events.Security.AntiVirus;

namespace ISPCore.Engine.Cron
{
    public class AntiVirus
    {
        static bool IsRun = false;
        public static void Run(CoreDB coreDB)
        {
            if (IsRun)
                return;
            IsRun = true;
            
            // Меняем режим доступа к SQL
            SqlToMode.SetMode(SqlMode.Read);

            // Получаем весь список заданий
            var Alltasks = coreDB.RequestsFilter_Domains.Where(i => i.av.JobStatus == JobStatus.on && DateTime.Now.AddMinutes(-i.av.CheckEveryToMinute) > i.av.LastRun).Include(i => i.av).Include(i => i.Aliases).ToList();

            // Меняем режим доступа к SQL
            SqlToMode.SetMode(SqlMode.ReadOrWrite);

            // Проходим задания
            foreach (var task in Alltasks)
            {
                // Нету PHP
                if (!File.Exists(task.av.php))
                    continue;

                #region Локальный метод - "RunAV"
                void RunAV(string progress_id)
                {
                    #region Создаем команду
                    StringBuilder comand = new StringBuilder();
                    comand.Append($"--path={task.av.path} ");

                    if (!string.IsNullOrWhiteSpace(task.av.skip))
                        comand.Append($"--skip={task.av.skip} ");

                    if (!string.IsNullOrWhiteSpace(task.av.scan))
                        comand.Append($"--scan={task.av.scan} ");

                    comand.Append($"--mode={task.av.mode} ");
                    comand.Append($"--memory={task.av.memory}M ");
                    comand.Append($"--size={task.av.size}K ");
                    comand.Append($"--delay={task.av.delay} ");
                    #endregion

                    // Имя отчета
                    string report = $"{Models.Security.AntiVirus.name}_{Models.Security.AntiVirus.vers}_{DateTime.Now.ToString("HH-mm_dd-MM-yyy")}{task.av.path.Replace("/", "_-_")}";

                    // 
                    Trigger.OnStart((progress_id, report));

                    // Запускаем процесс bash
                    Bash bash = new Bash();
                    bash.Run($"{task.av.php} {Folders.AV}/ai-bolit.php {comand.ToString()} --progress={Folders.AV}/progress_id-{progress_id}.json --report={Folders.ReportsAV}/{report}.html >/dev/null 2>/dev/null");

                    // 
                    Trigger.OnStop((progress_id, report));
                }
                #endregion

                // Проверяем папки алиасов
                foreach (var alias in task.Aliases)
                {
                    if (string.IsNullOrWhiteSpace(alias.Folder))
                        continue;

                    RunAV($"{task.Id}.{alias.Id}");
                }

                // Задание самого домена
                RunAV(task.Id.ToString());

                // Обновляем LastRun
                SqlToMode.SetMode(SqlMode.Read);
                task.av.LastRun = DateTime.Now;
                coreDB.SaveChanges();
                SqlToMode.SetMode(SqlMode.ReadOrWrite);
            }
            
            IsRun = false;
        }
    }
}
