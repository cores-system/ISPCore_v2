using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ISPCore.Models.Databases.Interface;
using ISPCore.Engine.Databases;
using ISPCore.Models.RequestsFilter.Domains;
using ISPCore.Models.RequestsFilter.Domains.Log;
using ISPCore.Models.Databases;
using ISPCore.Models.SyncBackup.Tasks;
using ISPCore.Models.RequestsFilter.Templates;
using ISPCore.Models.Notification;
using ISPCore.Models.Base.WhiteList;
using ISPCore.Models.RequestsFilter.Base.Rules;
using ISPCore.Models.Databases.Enums;
using ISPCore.Models.RequestsFilter.Domains.Types;

namespace ISPCore
{
    public static class Expansion
    {
        public static string GetTemplateName(this DbSet<Template> db, int TemplateId, string DefaultDescription = null)
        {
            string name = db.Find(TemplateId)?.Name;
            if (name == null)
                return DefaultDescription;

            return name;
        }

        public static string ToSql(this string item) => item.Replace("'", "\"");

        #region RemoveAttach
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="Id"></param>
        public static bool RemoveAttach<T>(this DbSet<T> db, CoreDB coreDB, int Id) where T: class, IId, new()
        {
            try
            {
                // Получаем данные
                var item = new T() { Id = Id };
                db.Attach(item);

                // Удаляем данные
                db.Remove(item);

                // Сохраняем базу
                coreDB.SaveChanges();

                // Отдаем результат
                return true;
            }
            catch { return false; }
        }
        #endregion

        #region RemoveAll
        /// <summary>
        /// Удалить элементы из колекции
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db">Колекция</param>
        /// <param name="predicate">Функция поиска</param>
        public static void RemoveAll<T>(this DbSet<T> db, Func<T, bool> predicate) where T : class
        {
            foreach (var item in db.Where(predicate))
                db.Remove(item);
        }
        #endregion

        #region FindItem
        /// <summary>
        /// Получить элемент
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db">Колекция</param>
        /// <param name="predicate">Функция поиска</param>
        /// <param name="type">Тип поиска по колекции</param>
        public static T FindItem<T>(this DbSet<T> db, Func<T, bool> predicate, TrackingType type = TrackingType.NoTracking) where T : class
        {
            if (type == TrackingType.NoTracking)
                return db.AsNoTracking().AsEnumerable().Where(predicate).FirstOrDefault();

            return db.Where(predicate).FirstOrDefault();
        }
        #endregion

        #region FindAll
        /// <summary>
        /// Получить элементы
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db">Колекция</param>
        /// <param name="predicate">Функция поиска</param>
        /// <param name="type">Тип поиска по колекции</param>
        public static IEnumerable<T> FindAll<T>(this DbSet<T> db, Func<T, bool> predicate, TrackingType type = TrackingType.NoTracking) where T : class
        {
            if (type == TrackingType.NoTracking)
                return db.AsNoTracking().AsEnumerable().Where(predicate);

            return db.Where(predicate);
        }
        #endregion

        #region FindAndInclude
        public static T FindAndInclude<T>(this DbSet<T> _db, int Id, bool AsNoTracking = false) where T : class
        {
            var db = AsNoTracking ? _db.AsNoTracking() : _db;
            switch (db)
            {
                case IQueryable<Template> Templates:
                    return (dynamic)Templates.Where(i => i.Id == Id).Include(r => r.Rules).Include(r => r.RuleReplaces).Include(r => r.RuleOverrides).Include(r => r.RuleArgs).FirstOrDefault();

                case IQueryable<Domain> Domains:
                    return (dynamic)Domains.Where(i => i.Id == Id).Include(a => a.av).Include(a => a.AntiBot).Include(l => l.limitRequest).Include(i => i.IgnoreToLogs).Include(c => c.confToLog).Include(a => a.Aliases).Include(r => r.Rules).Include(r => r.RuleReplaces).Include(r => r.RuleOverrides).Include(r => r.RuleArgs).Include(t => t.Templates).FirstOrDefault();

                case IQueryable<Notation> Notations:
                    return (dynamic)Notations.Where(i => i.Id == Id).Include(n => n.More).FirstOrDefault();

                case IQueryable<Task> Tasks:
                    return (dynamic)Tasks.Where(i => i.Id == Id).Include(f => f.FTP).Include(w => w.WebDav).Include(o => o.OneDrive).Include(i => i.IgnoreFileOrFolders).FirstOrDefault();

                case IQueryable<Models.SyncBackup.Database.Task> Tasks:
                    return (dynamic)Tasks.Where(i => i.Id == Id).Include(i => i.Conf).Include(i => i.MySQL).FirstOrDefault();
            }

            return null;
        }
        #endregion

        #region UpdateOrAddRange
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="data"></param>
        /// <param name="NewIds"></param>
        public static void UpdateOrAddRange<T>(this IList<T> collection, IDictionary<string, T> data, out IDictionary<string, IId> NewIds) where T : class, IId
        {
            NewIds = new Dictionary<string, IId>();
            if (data == null)
                return;

            foreach (var item in data)
            {
                #region Проверяем данные
                switch ((dynamic)item.Value)
                {
                    #region WhiteList
                    case WhiteListModel whiteList:
                        {
                            if (string.IsNullOrWhiteSpace(whiteList?.Value))
                                continue;

                            // Убириаем лишнее с правила
                            whiteList.Value = Regex.Replace(whiteList.Value, "(^\\^|\\$$)", "");
                            break;
                        }
                    #endregion

                    #region IRule
                    case IRule rule:
                        {
                            if (string.IsNullOrWhiteSpace(rule?.rule))
                                continue;
                            break;
                        }
                    #endregion

                    #region RuleReplace
                    case RuleReplace ruleReplace:
                        {
                            // URL для замены ответа
                            if (string.IsNullOrWhiteSpace(ruleReplace?.uri))
                                continue;

                            // Если не указано какие аргументы заменять
                            if (string.IsNullOrWhiteSpace(ruleReplace?.RegexWhite) || (string.IsNullOrWhiteSpace(ruleReplace?.GetArgs) && string.IsNullOrWhiteSpace(ruleReplace?.PostArgs)))
                            {
                                // Если не указан url куда  отправить пользователя или код ответа
                                if ((ruleReplace.TypeResponse == TypeResponseRule._302 && string.IsNullOrWhiteSpace(ruleReplace?.ResponceUri)) || 
                                    (ruleReplace.TypeResponse == TypeResponseRule.kode && string.IsNullOrWhiteSpace(ruleReplace?.kode)))
                                    continue;
                            }

                            break;
                        }
                    #endregion

                    #region Domain - Alias
                    case Alias alias:
                        {
                            if (string.IsNullOrWhiteSpace(alias?.host) || item.Key == "domain")
                                continue;

                            // Обновляем имя домена
                            alias.host = Regex.Replace(alias.host.ToLower(), "^www\\.", "");
                            break;
                        }
                    #endregion

                    default:
                        continue;
                }
                #endregion

                #region Записываем новые данные и перезаписываем старые
                if (item.Value.Id > 0)
                {
                    // Обновляем старые значения
                    if (collection.FirstOrDefault(i => i.Id == item.Value.Id) is T value)
                        CommonModels.Update(value, item.Value);
                }
                else
                {
                    // Добовлям новые значения
                    collection.Add(item.Value);
                    NewIds.Add(item.Key, item.Value);
                }
                #endregion
            }
        }
        #endregion

        #region AddRange
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="DependentTableId">Зависимая таблица</param>
        /// <param name="data"></param>
        /// <param name="NewIds"></param>
        public static void AddRange<T>(this DbSet<T> collection, int DependentTableId, IDictionary<string, T> data, out IDictionary<string, IId> NewIds) where T: class, IId
        {
            NewIds = new Dictionary<string, IId>();
            if (data == null)
                return;

            foreach (var item in data)
            {
                #region Проверяем данные
                switch ((dynamic)item.Value)
                {
                    #region Domain - IgnoreToLog
                    case IgnoreToLog ignoreToLog:
                        {
                            if (string.IsNullOrWhiteSpace(ignoreToLog?.rule))
                                continue;
                            ignoreToLog.DomainId = DependentTableId;
                            break;
                        }
                    #endregion

                    #region Domain - TemplateId
                    case TemplateId templateId:
                        {
                            if (templateId.Template == 0)
                                continue;

                            templateId.Id = 0;
                            templateId.DomainId = DependentTableId;
                            break;
                        }
                    #endregion

                    #region SyncBackup - IgnoreFileOrFolders
                    case IgnoreFileOrFolders ignoreFileOrFolders:
                        {
                            if (string.IsNullOrWhiteSpace(ignoreFileOrFolders?.Patch))
                                continue;

                            ignoreFileOrFolders.TaskId = DependentTableId;
                            break;
                        }
                    #endregion

                    default:
                        continue;
                }
                #endregion

                NewIds.TryAdd(item.Key, item.Value);
                collection.Add(item.Value);
            }
        }
        #endregion
    }
}
