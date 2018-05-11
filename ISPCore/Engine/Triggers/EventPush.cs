using ISPCore.Engine.Base;
using ISPCore.Models.Triggers;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ISPCore.Engine.Triggers
{
    public class EventPush
    {
        #region EventPush
        TriggerConf triggerConf;
        Subscription subs;
        IMemoryCache memoryCache;
        bool IsActual = true;
        
        /// <param name="conf">Данные триггера</param>
        /// <param name="subs">Стартовая подписка</param>
        public EventPush(TriggerConf conf, Subscription subs)
        {
            this.triggerConf = conf;
            this.subs = subs;
            memoryCache = Service.Get<IMemoryCache>();
        }
        #endregion

        #region EventHandler
        /// <summary>
        /// Событие триггера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data">Данные события</param>
        void EventHandler(object sender, ITuple data)
        {
            if (!triggerConf.IsActive || !IsActual)
                return;

            try
            {
                // Обновляем базу если файл изменился
                if (triggerConf.LastUpdateFile != File.GetLastWriteTime(triggerConf.TriggerFile))
                {
                    IsActual = false;
                    RegisteredTriggers.UpdateDB();
                    return;
                }

                // Имена полей для заполнения данных в GenScript
                var TrigerAttributes = Assembly.GetExecutingAssembly().GetType($"ISPCore.Models.Triggers.Events.{subs.TrigerPath}").GetProperty($"On{subs.TrigerName}").GetCustomAttribute<TupleElementNamesAttribute>().TransformNames;

                // Значения и модель
                ITuple tuple = data;
                var model = new GenScript();

                // Заполняем модель
                for (int i = 0; i < tuple.Length; i++)
                {
                    model.SetValue(TrigerAttributes[i], tuple[i]);
                }

                // Выполняем тригер
                if (RunTrigger(triggerConf.Id, triggerConf.LastUpdateFile, model, triggerConf.Trigger, subs.StepId))
                    triggerConf.LastRunToSuccess = DateTime.Now;
            }
            catch (Exception ex) {
                File.AppendAllText(Folders.File.TriggerErrorLog, ex.ToString() + "\n\n=======================================================================\n\n");
            }
        }
        #endregion

        #region RunTrigger
        /// <summary>
        /// Выполнить триггер
        /// </summary>
        /// <param name="Id">Id триггера</param>
        /// <param name="LastUpdateFile">Время обновления триггера</param>
        /// <param name="model">Данные события</param>
        /// <param name="triger">Пользовательское условие (триггер)</param>
        /// <param name="StepId">Следующие условие</param>
        bool RunTrigger(string Id, DateTime LastUpdateFile, GenScript model, Dictionary<string, Trigger> triger, string StepId)
        {
            try
            {
                // Короткое имя
                var tg = triger[StepId];

                #region Компиляция и кеширование
                ScriptRunner<bool> runner = null;
                string memKey = $"Triggers.EventPush:{Id}-{StepId}";

                if (!memoryCache.TryGetValue(memKey, out (ScriptRunner<bool> Runner, DateTime LastUpdateFile) cache) || cache.LastUpdateFile != LastUpdateFile)
                {
                    var scriptOptions = ScriptOptions.Default.AddImports(tg.Namespaces);
                    var script = CSharpScript.Create<bool>(tg.code, options: scriptOptions, globalsType: typeof(GenScript));
                    script.Compile();
                    runner = script.CreateDelegate();
                    memoryCache.Set(memKey, (runner, LastUpdateFile), TimeSpan.FromHours(6));
                }
                else
                {
                    runner = cache.Runner;
                }
                #endregion

                // Условие выполнено
                if (runner(model).Result)
                {
                    switch (tg.returnType)
                    {
                        case ReturnType.NextStep:
                            {
                                // Выполняем следующие условия
                                foreach (var item in tg.NextSteps.Split(','))
                                {
                                    if (!RunTrigger(Id, LastUpdateFile, model, triger, item))
                                        return false;
                                }
                            }
                            break;
                    }
                }

                // Успех
                return true;
            }
            catch (Exception ex)
            {
                File.AppendAllText(Folders.File.TriggerErrorLog, ex.ToString() + "\n\n=======================================================================\n\n");
                return false;
            }
        }
        #endregion
    }
}
