using ISPCore.Models.api;
using ISPCore.Engine.Base;
using ISPCore.Models.Base;
using ISPCore.Models.Databases;
using ISPCore.Models.Databases.json;
using ISPCore.Models.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.IO;

namespace ISPCore.Engine.Cron
{
    public class Project
    {
        private static bool IsRun = false;
        public static void Run(CoreDB coreDB, JsonDB jsonDB, IMemoryCache memoryCache)
        {
            if (IsRun)
                return;
            IsRun = true;

            if (!memoryCache.TryGetValue("Cron-ProjectInfo", out _))
            {
                memoryCache.Set("Cron-ProjectInfo", (byte)0, Startup.AbsoluteExpirationToAPI);

                #region Получаем новости проекта
                try
                {
                    HttpClient client = new HttpClient();
                    var newsAPI = JsonConvert.DeserializeObject<List<ProjectNews>>(client.GetStringAsync("http://api.core-system.org/isp/news").Result);
                    if (newsAPI.Count > 0)
                    {
                        jsonDB.ProjectNews = newsAPI;
                        jsonDB.Save();
                    }
                }
                catch { }
                #endregion

                #region Получаем список изменений проекта
                try
                {
                    HttpClient client = new HttpClient();
                    var ProjectChange = JsonConvert.DeserializeObject<List<ProjectChange>>(client.GetStringAsync("http://api.core-system.org/isp/change").Result);
                    if (ProjectChange.Count > 0)
                    {
                        jsonDB.ProjectChange = ProjectChange;
                        jsonDB.Save();
                    }

                }
                catch { }
                #endregion

                #region Сравниваем версию ISPCore
                try
                {
                    HttpClient client = new HttpClient();
                    var result = JsonConvert.DeserializeObject<LatestVersion>(client.GetStringAsync("http://api.core-system.org/isp/LatestVersion").Result);

                    // Сверяем версии ISPCore
                    if (result.Version > 0 && (result.Version > Startup.version.Version || (result.Version == Startup.version.Version && result.Patch > Startup.version.Patch)))
                    {
                        #region Автоматическое обновление ISPCore
                        if (jsonDB.Base.AutoUpdate && (Platform.Get == PlatformOS.Docker || Platform.Get == PlatformOS.Unix))
                        {
                            // Если метод auto-update.sh еще не вызывался для этой версии ISPCore
                            if (!File.Exists($"{Folders.AutoUpdate}/{Startup.version.ToString()}.ok"))
                            {
                                // Проверяем можно ли текущею версию обновлять
                                if (client.GetStringAsync($"http://api.core-system.org/isp/UpdateSupport?Version={Startup.version.Version}&Patch={Startup.version.Patch}&os={PlatformOS.Unix.ToString()}").Result == "ok")
                                {
                                    // Записываем версию ISPCore с которой был вызван auto-update.sh
                                    File.WriteAllText($"{Folders.AutoUpdate}/{Startup.version.ToString()}.ok", string.Empty);

                                    // Обновляем
                                    Bash bash = new Bash();
                                    string os = Platform.Get == PlatformOS.Docker ? "docker" : "linux";
                                    bash.Run($"curl -fsSL http://cdn.core-system.org/isp/{os}/auto-update.sh | sh");
                                    return;
                                }
                            }
                        }
                        #endregion

                        // Уведомление
                        var note = new Notation()
                        {
                            Category = "Система",
                            Msg = jsonDB.Base.AutoUpdate ? "Доступна новая версия ISPCore, требуется ручное обновление" : "Доступна новая версия ISPCore",
                            Time = DateTime.Now,
                            More = new List<More>()
                            {
                                new More("Текущая версия", Startup.version.ToString()),
                                new More("Доступна версия", result.ToString())
                            }
                        };

                        // HashData
                        note.HashData = Notation.CreateHashData(note);

                        // Если в базе нету HashData
                        if (coreDB.Notations.AsNoTracking().FirstOrDefault(it => it.HashData == note.HashData) == null)
                        {
                            // Добовляем в базу
                            coreDB.Notations.Add(note);

                            // Сохраняем базу
                            coreDB.SaveChanges();

                            // Обновляем CountNotification
                            jsonDB.Base.CountNotification++;
                            jsonDB.Save();
                        }
                    }
                }
                catch { }
                #endregion
            }

            IsRun = false;
        }
    }
}
