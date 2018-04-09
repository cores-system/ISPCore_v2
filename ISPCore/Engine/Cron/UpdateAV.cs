using ISPCore.Engine.Base;
using ISPCore.Engine.Base.SqlAndCache;
using ISPCore.Models.Base;
using ISPCore.Models.Databases;
using ISPCore.Models.Databases.json;
using ISPCore.Models.Notification;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace ISPCore.Engine.Cron
{
    public class UpdateAV
    {
        private static bool IsRun = false;
        public static void Run(CoreDB coreDB, JsonDB jsonDB, IMemoryCache memoryCache)
        {
            if (IsRun)
                return;
            IsRun = true;

            if (!memoryCache.TryGetValue("Cron-UpdateAV", out _))
            {
                memoryCache.Set("Cron-UpdateAV", (byte)1, Startup.AbsoluteExpirationToAPI);

                try
                {
                    HttpClient client = new HttpClient();
                    string vers = client.GetStringAsync("http://cdn.core-system.org/isp/av/vers.txt").Result;
                    string old_vers = File.ReadAllText($"{Folders.AV}/vers.txt");
                    if (Regex.IsMatch(vers, "^[0-9]+-[0-9]+-[0-9]+([\n\r]+)?$") && vers != old_vers)
                    {
                        if (Download("ai-bolit.php", "new_ai-bolit.php") && Download("AIBOLIT-WHITELIST.db", "new_AIBOLIT-WHITELIST.db"))
                        {
                            if (File.Exists($"{Folders.AV}/ai-bolit.php"))
                                File.Delete($"{Folders.AV}/ai-bolit.php");

                            if (File.Exists($"{Folders.AV}/AIBOLIT-WHITELIST.db"))
                                File.Delete($"{Folders.AV}/AIBOLIT-WHITELIST.db");

                            File.Move($"{Folders.AV}/new_ai-bolit.php", $"{Folders.AV}/ai-bolit.php");
                            File.Move($"{Folders.AV}/new_AIBOLIT-WHITELIST.db", $"{Folders.AV}/AIBOLIT-WHITELIST.db");
                            File.WriteAllText($"{Folders.AV}/vers.txt", vers);

                            // Добовляем данные в базу
                            SqlToMode.SetMode(SqlMode.Read);
                            coreDB.Notations.Add(new Notation()
                            {
                                Category = "Обновления",
                                Msg = "Обновлен антивирус AI-Bolit",
                                Time = DateTime.Now,
                                More = new List<More>()
                                {
                                    new More("Предыдущая версия", old_vers.Replace('-', '.')),
                                    new More("Текущая версия", vers.Replace('-', '.'))
                                }
                            });

                            // Сохраняем базу
                            coreDB.SaveChanges();
                            SqlToMode.SetMode(SqlMode.ReadOrWrite);

                            // Обновляем CountNotification
                            jsonDB.Base.CountNotification++;
                            jsonDB.Save();
                        }
                    }

                }
                catch { }
            }

            IsRun = false;
        }


        private static bool Download(string RemoteFile, string LocalFile)
        {
            try
            {
                HttpClient client = new HttpClient();
                string LocalFilePatch = $"{Folders.AV}/{LocalFile}";

                using (var RemoteStream = client.GetStreamAsync("http://cdn.core-system.org/isp/av/" + RemoteFile).Result)
                {
                    if (File.Exists(LocalFilePatch))
                        File.Delete(LocalFilePatch);

                    using (var LocalStream = File.OpenWrite(LocalFilePatch))
                    {
                        RemoteStream.CopyTo(LocalStream);
                    }
                }

                return true;
            }
            catch { return false; }
        }
    }
}
