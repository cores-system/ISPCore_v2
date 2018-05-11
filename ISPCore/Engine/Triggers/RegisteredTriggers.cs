﻿using ISPCore.Engine.Base;
using ISPCore.Models.Triggers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;

namespace ISPCore.Engine.Triggers
{
    public static class RegisteredTriggers
    {
        static ConcurrentDictionary<string, TriggerConf> dbTriggers = new ConcurrentDictionary<string, TriggerConf>();
        static ConcurrentDictionary<string, (EventInfo, Object, Delegate)> dbEvents = new ConcurrentDictionary<string, (EventInfo, Object, Delegate)>();

        #region UpdateDB
        static bool IsRunUpdateDB = false;
        public static void UpdateDB()
        {
            if (IsRunUpdateDB)
                return;
            IsRunUpdateDB = true;

            foreach (var file in Directory.GetFiles(Folders.Triggers, "*.conf"))
            {
                try
                {
                    // Имя файла
                    string FileName = Path.GetFileName(file);

                    // Кеш
                    if (!dbTriggers.TryGetValue(FileName, out var cache) || cache.LastUpdateFile != File.GetLastWriteTime(file))
                    {
                        // Получаем триггер
                        var triggerConf = JsonConvert.DeserializeObject<TriggerConf>(File.ReadAllText(file));
                        triggerConf.LastUpdateFile = File.GetLastWriteTime(file);
                        triggerConf.TriggerFile = file;

                        // Обновляем базу
                        dbTriggers.AddOrUpdate(FileName, triggerConf, (s, e) => triggerConf);

                        if (cache != null)
                        {
                            // Отписываем текущий триггер
                            (EventInfo eventInfo, Object target, Delegate handler) data = dbEvents[FileName];
                            data.eventInfo.RemoveEventHandler(data.target, data.handler);

                            // Подписываем новый триггер 
                            RegTriggerToEvent(triggerConf);
                        }
                    }
                }
                catch (Exception ex) {
                    File.AppendAllText(Folders.File.TriggerErrorLog, ex.ToString() + "\n\n=======================================================================\n\n");
                }
            }

            IsRunUpdateDB = false;
        }
        #endregion

        #region RegTriggerToEvent
        /// <summary>
        /// Регистрация триггера
        /// </summary>
        static void RegTriggerToEvent(TriggerConf triggerConf)
        {
            try
            {
                // Обрабатываем подписки
                foreach (var subs in triggerConf.Subscriptions)
                {
                    // Ищем нужный event
                    EventInfo eventInfo = Assembly.GetExecutingAssembly().GetType($"ISPCore.Models.Triggers.Events.{subs.TrigerPath}").GetEvent(subs.TrigerName);

                    // Создаем делегат EventPush
                    MethodInfo EventPushMethod = typeof(EventPush).GetMethod("EventHandler", BindingFlags.NonPublic | BindingFlags.Instance);
                    Delegate handler = Delegate.CreateDelegate(eventInfo.EventHandlerType, new EventPush(triggerConf, subs), EventPushMethod);

                    // Подписываемся на события
                    object target = Activator.CreateInstance(Assembly.GetExecutingAssembly().GetType($"ISPCore.Models.Triggers.Events.{subs.TrigerPath}"));
                    eventInfo.AddEventHandler(target, handler);

                    // Сохраняем Event для отписки
                    var data = (eventInfo, target, handler);
                    dbEvents.AddOrUpdate(Path.GetFileName(triggerConf.TriggerFile), data, (s, e) => data);

                }
            }
            catch (Exception ex) {
                File.AppendAllText(Folders.File.TriggerErrorLog, ex.ToString() + "\n\n=======================================================================\n\n");
            }
        }
        #endregion

        #region Initialize
        /// <summary>
        /// Регистрация триггеров
        /// </summary>
        public static void Initialize()
        {
            foreach (var triggerConf in List())
            {
                RegTriggerToEvent(triggerConf);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Список триггеров
        /// </summary>
        public static IEnumerable<TriggerConf> List()
        {
            UpdateDB();
            foreach (var item in dbTriggers)
            {
                yield return item.Value;
            }
        }
        #endregion
    }
}