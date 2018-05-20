using System;
using System.Collections.Generic;
using System.Linq;
using ISPCore.Engine.Base;
using ISPCore.Engine.Triggers;
using ISPCore.Models.Databases;
using ISPCore.Models.Response;
using ISPCore.Models.Triggers;
using ISPCore.Models.Triggers.Blueprint;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ISPCore.Controllers
{
    public class ToolsToTriggerNodes : ControllerToDB
    {
        #region Index
        public IActionResult Index(bool ajax, int Id = 0)
        {
            ViewData["Id"] = Id;
            ViewData["ajax"] = ajax;

            if (RegisteredTriggers.List().Where(i => i.Id == Id).FirstOrDefault() is TriggerConf triggerConf)
            {
                // Модель ceron.pw
                Dictionary<string, BlueprintModel> blueprintModel = new Dictionary<string, BlueprintModel>();

                #region Конверт "Subscriptions" в "BlueprintModel"
                foreach (var subs in triggerConf.Subscriptions)
                {
                    blueprintModel.Add(subs.Key, new BlueprintModel()
                    {
                        uid = subs.Key,
                        worker = "event",
                        position = subs.Value.position,
                        varsData = new VarsData()
                        {
                            input = new Input()
                            {
                                name = subs.Value.TrigerName,
                                path = subs.Value.TrigerPath,
                            }
                        }
                    });
                }
                #endregion

                #region Конверт "TriggerNodes" в "BlueprintModel"
                foreach (var node in triggerConf.Trigger)
                {
                    blueprintModel.Add(node.Key, new BlueprintModel()
                    {
                        uid = node.Key,
                        worker = "action",
                        position = node.Value.position,
                        varsData = new VarsData()
                        {
                            output = new Output()
                            {
                                output = node.Value.Name
                            },

                            input = new Input()
                            {
                                code = node.Value.code,
                                references = string.Join('\n', node.Value.References),
                                namespaces = string.Join('\n', node.Value.Namespaces),
                            }
                        }
                    });
                }
                #endregion

                #region Ставим ссылки NextSteps
                foreach (var subs in triggerConf.Subscriptions)
                {
                    if (subs.Value.StepIds == null)
                        continue;

                    foreach (string StepId in subs.Value.StepIds.Split(','))
                    {
                        if (string.IsNullOrWhiteSpace(StepId))
                            continue;

                        if (blueprintModel.TryGetValue(StepId, out BlueprintModel item))
                        {
                            item.parents.Add(new Parent()
                            {
                                uid = subs.Key,
                                input = "input",
                                output = "output"
                            });
                        }
                    }
                }

                foreach (var node in triggerConf.Trigger)
                {
                    if (node.Value.NextSteps == null)
                        continue;

                    foreach (string StepId in node.Value.NextSteps.Split(','))
                    {
                        if (string.IsNullOrWhiteSpace(StepId))
                            continue;

                        if (blueprintModel.TryGetValue(StepId, out BlueprintModel item))
                        {
                            item.parents.Add(new Parent()
                            {
                                uid = node.Key,
                                input = "input",
                                output = "output"
                            });
                        }
                    }
                }
                #endregion

                // Успех
                return View("~/Views/Tools/Trigger/Nodes.cshtml", JsonConvert.SerializeObject(blueprintModel));
            }

            // Пустой ответ
            return View("~/Views/Tools/Trigger/Nodes.cshtml", "{}");
        }
        #endregion

        #region Save
        [HttpPost]
        public JsonResult Save(int Id, string nodes, bool IsAPI = false)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            try
            {
                // Поиск триггера
                var FindTrigger = RegisteredTriggers.List().Where(i => i.Id == Id).FirstOrDefault();
                if (FindTrigger == null)
                    return Json(new Text("Триггер не найден"));

                // Модель ceron.pw
                var blueprintModel = JsonConvert.DeserializeObject<IDictionary<string, BlueprintModel>>(nodes);

                // Подписки
                Dictionary<string, Subscription> Subscriptions = new Dictionary<string, Subscription>();

                // Условия триггера
                Dictionary<string, Trigger> TriggerNodes = new Dictionary<string, Trigger>();

                #region Конверт "BlueprintModel" в "Subscriptions/TriggerNodes"
                foreach (var blueprint in blueprintModel)
                {
                    #region Локальный метод - "StringToList"
                    List<string> StringToList(string data)
                    {
                        List<string> mass = new List<string>();
                        if (string.IsNullOrWhiteSpace(data))
                            return mass;

                        foreach (string line in data.Replace("\r", "").Split('\n'))
                        {
                            if (string.IsNullOrWhiteSpace(data))
                                continue;

                            mass.Add(line);
                        }

                        return mass;
                    }
                    #endregion

                    switch (blueprint.Value.worker)
                    {
                        case "event":
                            {
                                var val = blueprint.Value;
                                Subscriptions.Add(blueprint.Key, new Subscription()
                                {
                                    position = val.position,
                                    TrigerName = val.varsData.input.name,
                                    TrigerPath = val.varsData.input.path,
                                });
                                break;
                            }
                        case "action":
                            {
                                var val = blueprint.Value;
                                TriggerNodes.Add(blueprint.Key, new Trigger()
                                {
                                    position = val.position,
                                    Name = val.varsData.output.output,
                                    code = val.varsData.input.code,
                                    Namespaces = StringToList(val.varsData.input.namespaces),
                                    References = StringToList(val.varsData.input.references),
                                });
                                break;
                            }
                    }
                }
                #endregion

                #region Ставим ссылки NextSteps
                foreach (var blueprint in blueprintModel)
                {
                    if (blueprint.Value.worker == "action")
                    {
                        foreach (var parent in blueprint.Value.parents)
                        {
                            if (Subscriptions.TryGetValue(parent.uid, out Subscription sub)) {
                                sub.StepIds += string.IsNullOrWhiteSpace(sub.StepIds) ? blueprint.Key : "," + blueprint.Key;
                            }

                            if (TriggerNodes.TryGetValue(parent.uid, out Trigger tgr)) {
                                tgr.NextSteps += string.IsNullOrWhiteSpace(tgr.NextSteps) ? blueprint.Key : "," + blueprint.Key;
                            }
                        }
                    }
                }
                #endregion

                // Обновляем параметры
                FindTrigger.Trigger = TriggerNodes;
                FindTrigger.Subscriptions = Subscriptions;

                // Сохраняем файл
                System.IO.File.WriteAllText(FindTrigger.TriggerFile, JsonConvert.SerializeObject(FindTrigger, Formatting.Indented));

                // Обновляем базу
                RegisteredTriggers.UpdateDB();

                // Отдаем сообщение
                return Json(new Text("Настройки успешно сохранены"));
            }
            catch (Exception ex)
            {
                return Json(new Text(ex.Message));
            }
        }
        #endregion
    }
}
