using ISPCore.Engine.Base;
using ISPCore.Engine.Base.SqlAndCache;
using ISPCore.Engine.Databases;
using ISPCore.Models.Base;
using ISPCore.Models.Databases;
using ISPCore.Models.RequestsFilter.Base;
using ISPCore.Models.RequestsFilter.Base.Enums;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ModelCache = ISPCore.Models.core.Cache.CheckLink;

namespace ISPCore.Engine.core.Cache.CheckLink
{
    partial class ISPCache
    {
        /// <summary>
        /// Кеш правил для доменов
        /// </summary>
        private static ConcurrentDictionary<int, ModelCache.Domain> MassGetDomain = new ConcurrentDictionary<int, ModelCache.Domain>();


        /// <summary>
        /// Получить домен
        /// </summary>
        /// <param name="Id">Id домена</param>
        /// <returns>DomainCacheModel</returns>
        public static ModelCache.Domain GetDomain(int Id)
        {
            #region Достаем данные из кеша
            if (MassGetDomain.TryGetValue(Id, out var cache))
            {
                cache.IsCache = true;
                return cache;
            }
            #endregion

            // Меняем режим доступа к SQL
            SqlToMode.SetMode(SqlMode.Read);

            #region Список правил
            List<Models.RequestsFilter.Base.Rules.RuleReplace> RulesReplace = new List<Models.RequestsFilter.Base.Rules.RuleReplace>();
            List<Rule> RulesAllow = new List<Rule>();
            List<Rule> RulesDeny = new List<Rule>();
            List<Rule> Rules2FA = new List<Rule>();
            List<Rule> RulesOverrideAllow = new List<Rule>();
            List<Rule> RulesOverrideDeny = new List<Rule>();
            List<Rule> RulesOverride2FA = new List<Rule>();
            List<RuleArg> RuleArgs = new List<RuleArg>();
            #endregion

            // База CoreDB
            using (var coreDB = Service.Get<CoreDB>())
            {
                #region Если домена нету в базе 
                cache = new ModelCache.Domain();
                var domain = coreDB.RequestsFilter_Domains.FindAndInclude(Id, AsNoTracking: true);
                if (domain == null)
                {
                    // Отдаем пустой кеш без  правил
                    SqlToMode.SetMode(SqlMode.ReadOrWrite);
                    return cache;
                }
                #endregion

                // Блокировка IP в 'Брандмауэр' глобально или только для домена
                cache.typeBlockIP = domain.typeBlockIP;

                // Настройки логирования запросов
                cache.confToLog = domain.confToLog;

                // Защита сайта от BruteForce и Ботов
                cache.StopBruteForce = domain.StopBruteForce;
                cache.Auth2faToAccess = domain.Auth2faToAccess;

                #region Найтройки AntiBot
                // Клонируем обьект
                cache.AntiBot = domain.AntiBot.Clone();
                
                // Переопределяем правило
                cache.AntiBot.BackgroundCheckToAddExtensions = string.IsNullOrWhiteSpace(cache.AntiBot.BackgroundCheckToAddExtensions) ? "^$" : $"(\\.{cache.AntiBot.BackgroundCheckToAddExtensions.Replace(",", "|\\.")})";
                #endregion

                // Достаем список правил для игнорирования логов
                var IgnoreToLogs = domain.IgnoreToLogs.Select(i => i.rule).ToList();
                if (IgnoreToLogs.Count > 0)
                    cache.IgnoreLogToRegex = "^(" + string.Join('|', domain.IgnoreToLogs.Select(i => i.rule)) + ")$";

                #region Настройки лимитирования запросов
                if (domain.limitRequest.UseGlobalConf || domain.limitRequest.MinuteLimit > 0 || domain.limitRequest.HourLimit > 0 || domain.limitRequest.DayLimit > 0)
                {
                    cache.limitRequest.IsEnabled = true;                                                                              // Режим лимитирования запросов включен
                    cache.limitRequest.UseGlobalConf = domain.limitRequest.UseGlobalConf;                                             // Использовать глобальные или локальные настройки
                    cache.limitRequest.MinuteLimit = domain.limitRequest.MinuteLimit;                                                 // Минутный лимит запросов
                    cache.limitRequest.HourLimit = domain.limitRequest.HourLimit;                                                     // Часовой лимит запросов
                    cache.limitRequest.DayLimit = domain.limitRequest.DayLimit;                                                       // Метод блокировки при достижении лимита запросов
                    cache.limitRequest.BlockType = domain.limitRequest.BlockType;                                                     // Суточный лимит запросов
                    cache.limitRequest.MaxRequestToAgainСheckingreCAPTCHA = domain.limitRequest.MaxRequestToAgainСheckingreCAPTCHA;   // Количество запросов перед повторной проверкой reCAPTCHA
                }
                #endregion

                #region Собираем правила c шаблонов
                foreach (var TemplateId in domain.Templates.Select(t => t.Template))
                {
                    // Поиск шаблона
                    if (coreDB.RequestsFilter_Templates.FindAndInclude(TemplateId, AsNoTracking: true) is Models.RequestsFilter.Templates.Template tpl)
                    {
                        // Позволяет удалить кеш домена если изменится шаблон
                        cache.TemplateIds.Add(TemplateId);

                        // Собираем правила c шаблона
                        SortToRule(tpl.Rules, IsOverrides: false);
                        SortToRule(tpl.RuleOverrides, IsOverrides: true);

                        // Cписок аргументов шаблона
                        RuleArgs.AddRange(tpl.RuleArgs);

                        // Правила шаблона - "Замена ответа"
                        RulesReplace.AddRange(tpl.RuleReplaces);
                    }
                }
                #endregion

                // Основные правила домена
                SortToRule(domain.Rules, IsOverrides: false);
                SortToRule(domain.RuleOverrides, IsOverrides: true);

                // Cписок аргументов домена
                RuleArgs.AddRange(domain.RuleArgs);

                // Правила домена - "Замена ответа"
                RulesReplace.AddRange(domain.RuleReplaces);

                // Конвертируем Rule в ModelCache.Rule
                ConvertToModelCache(RuleArgs, RulesAllow, cache.RuleAllow);
                ConvertToModelCache(RuleArgs, RulesDeny, cache.RuleDeny);
                ConvertToModelCache(RuleArgs, Rules2FA, cache.Rule2FA);
                ConvertToModelCache(RuleArgs, RulesOverrideAllow, cache.RuleOverrideAllow);
                ConvertToModelCache(RuleArgs, RulesOverrideDeny, cache.RuleOverrideDeny);
                ConvertToModelCache(RuleArgs, RulesOverride2FA, cache.RuleOverride2FA);

                // Конвертируем RuleReplace в ModelCache.RuleReplaces
                ConvertRuleReplaceToModelCache(RulesReplace, cache.RuleReplaces);

                // Меняем режим доступа к SQL
                SqlToMode.SetMode(SqlMode.ReadOrWrite);

                // Отдаем данные
                MassGetDomain.AddOrUpdate(Id, cache, (i, d) => cache);
                return cache;
            }

            #region Локальный метод SortToRule
            void SortToRule(IEnumerable<Rule> inRules, bool IsOverrides)
            {
                foreach (var rile in inRules)
                {
                    if (IsOverrides)
                    {
                        switch (rile.order)
                        {
                            case ActionCheckLink.allow:
                                RulesOverrideAllow.Add(rile);
                                break;
                            case ActionCheckLink.deny:
                                RulesOverrideDeny.Add(rile);
                                break;
                            case ActionCheckLink.Is2FA:
                                RulesOverride2FA.Add(rile);
                                break;
                        }
                    }
                    else
                    {
                        switch (rile.order)
                        {
                            case ActionCheckLink.allow:
                                RulesAllow.Add(rile);
                                break;
                            case ActionCheckLink.deny:
                                RulesDeny.Add(rile);
                                break;
                            case ActionCheckLink.Is2FA:
                                Rules2FA.Add(rile);
                                break;
                        }
                    }
                }
            }
            #endregion
        }

        #region ConvertToModelCache
        /// <summary>
        /// Конвертируем правила в ModelCache.Rule
        /// </summary>
        /// <param name="Входящий список правил sql">Входящий список правил "Аргументы" sql</param>
        /// <param name="inRules">Входящий список правил sql</param>
        /// <param name="outRule">Исходящее правило ModelCache.Rule</param>
        public static void ConvertToModelCache(List<RuleArg> inRuleArgs, List<Rule> inRules, ModelCache.Rules.Rule outRule)
        {
            #region Локальный метод GetArgs
            (bool IsSuccess, string argReplace, string RuleGetToRegex, string RulePostToRegex) GetArgs(string rule)
            {
                if (!rule.Contains("{arg:"))
                    return (false, null, null, null);

                #region Сборка RuleArgs
                List<string> outRuleArgsToGet = new List<string>();                 // Аргументы для GET запроса
                List<string> outRuleArgsToPOST = new List<string>();                // Аргументы для POST запроса

                // Достаем имена аргументов которые нужно найти и подключить
                foreach (var argName in new Regex("{arg:([^}]+)}").Match(rule).Groups[1].Value.ToLower().Split(','))
                {
                    // Достаем аргументы по имени из SQL
                    foreach (var arg in inRuleArgs.Where(i => Regex.IsMatch(i.Name, argName, RegexOptions.IgnoreCase)))
                    {
                        // Пропускаем неправильные аргументы
                        if (!arg.rule.Contains('='))
                            continue;

                        // Удаляем коментарий из правила
                        string ruleArg = Regex.Replace(arg.rule, "^\"([^\"]+)?\";", "");

                        switch (arg.Method)
                        {
                            case RequestMethod.all:
                                outRuleArgsToGet.Add($@"((\?|&){ruleArg})?");    // Добовляем правило в аргументы для GET запроса
                                outRuleArgsToPOST.Add($@"(&{ruleArg})?");   // Добовляем правило в аргументы для POST запроса
                                break;
                            case RequestMethod.POST:
                                outRuleArgsToPOST.Add($@"(&{ruleArg})?");   // Добовляем правило в аргументы для POST запроса
                                break;
                            case RequestMethod.GET:
                                outRuleArgsToGet.Add($@"((\?|&){ruleArg})?");    // Добовляем правило в аргументы для GET запроса
                                break;
                        }
                    }
                }
                #endregion

                // Аргумент для вырезки из правила
                string argReplace = new Regex("({arg:[^}]+})").Match(rule).Groups[1].Value;

                // Готовый список GET аргументов для Regex
                string RuleGetToRegex = "";
                if (outRuleArgsToGet.Count > 0) {
                    RuleGetToRegex = "(" + string.Join('|', outRuleArgsToGet) + ")*";
                }

                // Готовый список POST аргументов для Regex
                string RulePostToRegex = "^$";
                if (outRuleArgsToPOST.Count > 0) {
                    RulePostToRegex = "^(" + string.Join('|', outRuleArgsToPOST) + ")*$";
                }

                // Отдаем ответ
                return (true, argReplace, RuleGetToRegex, RulePostToRegex);
            }
            #endregion

            // Переменные для правил
            List<string> RulesGetToRegex = new List<string>();
            List<string> RulesPostToRegex = new List<string>();
            List<string> RulesArgsCheckPostToRegex = new List<string>();

            foreach (var rule in inRules)
            {
                // Пропускаем неактивные правила
                if (!rule.IsActive || string.IsNullOrWhiteSpace(rule.rule))
                    continue;

                // Удаляем коментарий
                rule.rule = Regex.Replace(rule.rule, "^\"([^\"]+)?\";", "");

                #region Локальный метод 'AddRuleToGet'
                void AddRuleToGet(string item)
                {
                    var res = GetArgs(item);
                    if (res.IsSuccess)
                        item = item.Replace(res.argReplace, res.RuleGetToRegex);

                    RulesGetToRegex.Add(item);
                }
                #endregion

                #region Локальный метод 'AddRuleToPost'
                void AddRuleToPost(string item)
                {
                    var res = GetArgs(item);
                    if (res.IsSuccess)
                    {
                        // Правило с аргументами добовляем в 'RulesArgsCheckPostToRegex'
                        string newRule = item.Replace(res.argReplace, res.RuleGetToRegex);
                        RulesArgsCheckPostToRegex.Add(newRule);

                        // Добовляем правило и список аргументов для полной проверки POST запроса
                        outRule.postRules.Add(new ModelCache.Rules.PostRule()
                        {
                            rule = $"^{newRule}$",
                            RulePostToRegex = res.RulePostToRegex
                        });
                    }
                    else
                    {
                        // Правило без аргументов
                        RulesPostToRegex.Add(item);
                    }
                }
                #endregion

                // Распределяем правила по методу запроса
                switch (rule.Method)
                {
                    case RequestMethod.GET:
                        AddRuleToGet(rule.rule);
                        break;
                    case RequestMethod.POST:
                        AddRuleToPost(rule.rule);
                        break;
                    case RequestMethod.all:
                        AddRuleToGet(rule.rule);
                        AddRuleToPost(rule.rule);
                        break;
                }
            }

            // Готовый список GET аргументов для Regex
            if (RulesGetToRegex.Count > 0)
                outRule.RuleGetToRegex = "^(" + string.Join('|', RulesGetToRegex) + ")$";

            // Готовый список POST аргументов для Regex
            if (RulesPostToRegex.Count > 0)
                outRule.RulePostToRegex = "^(" + string.Join('|', RulesPostToRegex) + ")$";

            // Готовый список POST аргументов Regex, для быстрой проверки POST запросов
            if (RulesArgsCheckPostToRegex.Count > 0)
                outRule.RuleArgsCheckPostToRegex = "^(" + string.Join('|', RulesArgsCheckPostToRegex) + ")$";
        }
        #endregion

        #region ConvertRuleReplaceToModelCache
        /// <summary>
        /// Конвертируем правила в ModelCache.RuleReplace
        /// </summary>
        /// <param name="inRules">Входящий список правил sql</param>
        /// <param name="outRule">Исходящее правило ModelCache.RuleReplace</param>
        public static void ConvertRuleReplaceToModelCache(IEnumerable<Models.RequestsFilter.Base.Rules.RuleReplace> inRules, ModelCache.Rules.RuleReplaces outRule)
        {
            // Список GET/POST данных для быстрой проверки
            List<string> RuleGetToRegex = new List<string>();
            List<string> RulePostToRegex = new List<string>();

            // Собираем правила
            foreach (var inRule in inRules)
            {
                if (!inRule.IsActive)
                    continue;

                // Удаляем коментарий
                string uri = Regex.Replace(inRule.uri, "^\"([^\"]+)?\";", "");

                #region Локальный метод - "AddRuleTo"
                void AddRuleTo(List<string> mass, string args, string defaultOut, out string ArgsToRegex)
                {
                    // Список GET/POST аргументов
                    List<string> tmp = new List<string>();

                    // Собираем аргументы
                    if (!string.IsNullOrWhiteSpace(args))
                    {
                        foreach (var arg in args.Split(','))
                        {
                            if (string.IsNullOrWhiteSpace(arg))
                                continue;

                            tmp.Add(arg);
                        }
                    }

                    #region Добовляем данные в массив быстрой проверки
                    if (tmp.Count == 0)
                    {
                        mass.Add($@"{uri}(((\?|&)[^=]+=([^&]+)?)?)*");
                    }
                    else
                    {
                        mass.Add($@"{uri}((((\?|&)[^=]+=([^&]+)?)?)*(\?|&)({string.Join('|', tmp)})=[^&]+(((\?|&)[^=]+=([^&]+)?)?)*)");
                    }
                    #endregion

                    // 
                    ArgsToRegex = (tmp.Count == 0 ? defaultOut : $@"(\?|&)({string.Join('|', tmp)})=");
                }
                #endregion

                // Добовляем правила в массивы - "RuleGetToRegex/RulePostToRegex"
                AddRuleTo(RuleGetToRegex, inRule.GetArgs, "^$", out string GetArgsToRegex);
                AddRuleTo(RulePostToRegex, inRule.PostArgs, ".*", out string PostArgsToRegex);

                #region Добовляем правило в кеш
                outRule.Rules.Add(new ModelCache.Rules.RuleReplace()
                {
                    GetArgsToRegex = GetArgsToRegex,
                    GetArgs = string.IsNullOrWhiteSpace(inRule.GetArgs) ? string.Empty : inRule.GetArgs,
                    PostArgsToRegex = PostArgsToRegex,
                    PostArgs = string.IsNullOrWhiteSpace(inRule.PostArgs) ? string.Empty : inRule.PostArgs,
                    RegexWhite = inRule.RegexWhite,
                    ResponceUri = inRule.ResponceUri,
                    TypeResponse = inRule.TypeResponse,
                    ContentType = string.IsNullOrWhiteSpace(inRule.ContentType) ? "text/html; charset=utf-8" : inRule.ContentType,
                    kode = string.IsNullOrWhiteSpace(inRule.kode) ? "" : inRule.kode,
                    uri = uri,
                    Id = inRule.Id,
                    IsActive = true
                });
                #endregion
            }

            if (RuleGetToRegex.Count != 0) {
                outRule.RuleGetToRegex = $@"^({string.Join('|', RuleGetToRegex)})$";
            }

            if (RulePostToRegex.Count != 0) {
                outRule.RulePostToRegex = $@"^({string.Join('|', RulePostToRegex)})$";
            }
        }
        #endregion
    }
}
