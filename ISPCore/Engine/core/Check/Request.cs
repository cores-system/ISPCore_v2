using ISPCore.Engine.Base;
using ISPCore.Engine.Base.SqlAndCache;
using ISPCore.Engine.core.Cache.CheckLink;
using ISPCore.Engine.Middleware;
using ISPCore.Models.core;
using ISPCore.Models.core.Cache.CheckLink.Common;
using ISPCore.Models.Databases;
using ISPCore.Models.Databases.json;
using ISPCore.Models.RequestsFilter.Base;
using ISPCore.Models.RequestsFilter.Domains;
using ISPCore.Models.RequestsFilter.Domains.Log;
using ISPCore.Models.RequestsFilter.Domains.Types;
using ISPCore.Models.RequestsFilter.Monitoring;
using ISPCore.Models.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ModelCache = ISPCore.Models.core.Cache.CheckLink;

namespace ISPCore.Engine.core.Check
{
    public static class Request
    {
        static JsonDB jsonDB = Service.Get<JsonDB>();
        static IMemoryCache memoryCache = Service.Get<IMemoryCache>();

        #region Check
        public static Task Check(HttpContext context)
        {
            #region Получаем параметры запроса
            var gRequest = new Regex(@"^(\?ip=([^&]+)&|\?)method=([^&]+)&host=([^&]+)&uri=([^\n\r]+)").Match(context.Request.QueryString.Value).Groups;
            string IP = gRequest[2].Value, method = gRequest[3].Value.ToUpper(), host = Regex.Replace(gRequest[4].Value, @"^www\.", "", RegexOptions.IgnoreCase), uri = WebUtility.UrlDecode(gRequest[5].Value);

            // Проверяем правильно ли мы спарсили данные 
            if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(uri))
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                return context.Response.WriteAsync("Не сохранен порядок запроса<br />Пример: /core/check/request?ip=127.0.0.1&method=GET&host=test.com&uri=/", context.RequestAborted);
            }

            // IP адрес пользователя
            if (string.IsNullOrWhiteSpace(IP))
                IP = context.Connection.RemoteIpAddress.ToString();
            #endregion

            #region Получаем параметры POST запроса
            string FormData = string.Empty;
            if (context.Request.Method == "POST")
            {
                StringBuilder mass = new StringBuilder();
                foreach (var arg in context.Request.Form)
                {
                    // Удаляем из ключа массивы [] и проверяем на пустоту
                    string key = Regex.Replace(arg.Key, @"\[([^\]]+)?\]", "", RegexOptions.IgnoreCase);
                    if (string.IsNullOrWhiteSpace(key))
                        continue;

                    mass.Append($"&{key}={arg.Value[0]}");
                }

                FormData = mass.ToString();
            }
            #endregion

            #region ViewBag
            var viewBag = new ViewBag();
            viewBag.DebugEnabled = jsonDB.Base.DebugEnabled;       // Режим дебага, выводит json правил
            viewBag.IsErrorRule = false;                           // Переменная для ловли ошибок regex
            #endregion

            // Если домена нету в базе, то выдаем 303 или заглушку
            if (ISPCache.DomainToID(host) is int DomainID && DomainID == 0)
                return jsonDB.Base.EnableToDomainNotFound ? ViewDomainNotFound(context) : View(context, viewBag, ActionCheckLink.allow);

            // Если у IP есть полный доступ к сайтам или к сайту
            if (CheckLinkWhitelistToAllDomain())
                return View(context, viewBag, ActionCheckLink.allow);

            #region Получаем User-Agent и Referer
            // User-Agent
            string userAgent = string.Empty;
            if (context.Request.Headers.TryGetValue("User-Agent", out var tmp_userAgent))
                userAgent = tmp_userAgent.ToString();

            // Referer
            string Referer = string.Empty;
            if (context.Request.Headers.TryGetValue("Referer", out var tmp_Referer))
                Referer = tmp_Referer.ToString();
            #endregion

            // Достаем данные домена
            var Domain = ISPCache.GetDomain(DomainID);
            
            // Достаем настройки AntiBot из кеша
            var antiBotToGlobalConf = AntiBot.GlobalConf(jsonDB.AntiBot);

            // Достаем настройки WhiteList из кеша
            var whiteList = Engine.Base.SqlAndCache.WhiteList.GetCache(jsonDB.WhiteList);

            #region Проверяем "IP/User-Agent" в блокировке IPtables
            // Проверяем IP в блокировке IPtables по домену
            if (IPtablesMiddleware.CheckIP(IP, memoryCache, out IPtables BlockedData, host))
            {
                // Логируем пользователя
                AddJurnalTo200(IsIPtables: true);
                SetCountRequestToHour(TypeRequest._200, host, Domain.confToLog.EnableCountRequest);

                // Отдаем ответ
                context.Response.StatusCode = 401;
                context.Response.ContentType = "text/html; charset=utf-8";
                return context.Response.WriteAsync(IPtablesMiddleware.BlockedToHtml(IP, BlockedData.Description, BlockedData.TimeExpires), context.RequestAborted);
            }

            // Проверяем User-Agent в блокировке IPtables
            if (IPtablesMiddleware.CheckUserAgent(userAgent))
            {
                // Логируем пользователя
                AddJurnalTo200(IsIPtables: true);
                SetCountRequestToHour(TypeRequest._200, host, Domain.confToLog.EnableCountRequest);

                // Отдаем ответ
                context.Response.StatusCode = 401;
                context.Response.ContentType = "text/html; charset=utf-8";
                return context.Response.WriteAsync(IPtablesMiddleware.BlockedHtmlToUserAgent(userAgent), context.RequestAborted);
            }
            #endregion

            // IP нету в пользовательском белом списке
            // IP нету в глобальном белом списке
            if (!Regex.IsMatch(IP, whiteList.IpRegex) &&
                !WhitePtr.IsWhiteIP(IP))
            {
                #region AntiBot
                if (!AntiBot.ValidRequest(((antiBotToGlobalConf.conf.Enabled || Domain.AntiBot.UseGlobalConf) ? antiBotToGlobalConf.conf.type : Domain.AntiBot.type), host, method, uri, context, Domain, out string outHtml))
                {
                    // Логируем пользователя
                    AddJurnalTo200(IsAntiBot: true);
                    SetCountRequestToHour(TypeRequest._200, host, Domain.confToLog.EnableCountRequest);

                    // Выводим html пользователю
                    context.Response.ContentType = "text/html; charset=utf-8";
                    return context.Response.WriteAsync(outHtml, context.RequestAborted);
                }
                #endregion

                #region Лимит запросов
                if (Domain.limitRequest.IsEnabled || antiBotToGlobalConf.conf.limitRequest.IsEnabled)
                {
                    // Настройки лимита запросов
                    var limitRequest = (antiBotToGlobalConf.conf.limitRequest.IsEnabled || Domain.limitRequest.UseGlobalConf) ? antiBotToGlobalConf.conf.limitRequest : Domain.limitRequest;

                    // Проверяем белый список UserAgent
                    if (!Regex.IsMatch(userAgent, whiteList.UserAgentRegex, RegexOptions.IgnoreCase))
                    {
                        #region Локальный метод - "CheckToLimit"
                        bool CheckToLimit(string keyName, int limit, int ExpiresToMinute, out CacheValue _cacheValue)
                        {
                            string key = $"LimitRequestTo{keyName}-{IP}_{host}";
                            if (memoryCache.TryGetValue(key, out CacheValue item))
                            {
                                item.value++;
                                _cacheValue = item;
                                if (item.value > limit)
                                    return true;
                            }
                            else
                            {
                                _cacheValue = new CacheValue() { value = 1, Expires = DateTime.Now.AddMinutes(ExpiresToMinute) };
                                memoryCache.Set(key, _cacheValue, TimeSpan.FromMinutes(ExpiresToMinute));
                            }

                            return false;
                        }
                        #endregion

                        #region Локальный метод - "BlockedToIP"
                        void BlockedToIP(string Msg, DateTime Expires)
                        {
                            string KeyLimitRequestToBlockedWait = $"LimitRequestToBlockedWait-{IP}_{host}";
                            if (memoryCache.TryGetValue(KeyLimitRequestToBlockedWait, out _))
                                return;
                            memoryCache.Set(KeyLimitRequestToBlockedWait, (byte)0, TimeSpan.FromMinutes(5));

                            #region DNSLookup
                            string PtrHostName = null;
                            try
                            {
                                // Белый список Ptr
                                string WhitePtrRegex = whiteList.PtrRegex;
                                if (WhitePtrRegex != "^$" && !string.IsNullOrWhiteSpace(WhitePtrRegex))
                                {
                                    // Получаем имя хоста по IP
                                    var DnsHost = Dns.GetHostEntryAsync(IP).Result;

                                    // Получаем IP хоста по имени
                                    DnsHost = Dns.GetHostEntryAsync(DnsHost.HostName).Result;

                                    // Проверяем имя хоста и IP на совпадение 
                                    if (DnsHost.AddressList.Where(i => i.ToString() == IP).FirstOrDefault() != null)
                                    {
                                        PtrHostName = DnsHost.HostName;

                                        // Проверяем имя хоста на белый список DNSLookup
                                        if (Regex.IsMatch(DnsHost.HostName, WhitePtrRegex, RegexOptions.IgnoreCase))
                                        {
                                            // Добовляем IP в белый список на 9 дней
                                            WhitePtr.Add(IP, DateTime.Now.AddDays(9));
                                            memoryCache.Remove(KeyLimitRequestToBlockedWait);
                                            return;
                                        }
                                    }

                                }
                            }
                            catch { }
                            #endregion

                            // Записываем IP в кеш IPtables и журнал
                            SetBlockedToIPtables(Msg, Expires, PtrHostName);

                            // Сносим временную запись
                            memoryCache.Remove(KeyLimitRequestToBlockedWait);
                        }
                        #endregion

                        // Переменная для кеша
                        CacheValue cacheValue;

                        // Проверяем минутный лимит
                        if (limitRequest.MinuteLimit != 0 && CheckToLimit("Minute", limitRequest.MinuteLimit, 1, out cacheValue))
                        {
                            BlockedToIP("Превышен минутный лимит на запросы", cacheValue.Expires);
                            memoryCache.Remove($"LimitRequestToMinute-{IP}_{host}");
                        }

                        // Проверяем часовой лимит
                        if (limitRequest.HourLimit != 0 && CheckToLimit("Hour", limitRequest.HourLimit, 60, out cacheValue))
                        {
                            BlockedToIP("Превышен часовой лимит на запросы", cacheValue.Expires);
                            memoryCache.Remove($"LimitRequestToHour-{IP}_{host}");
                        }

                        // Проверяем дневной лимит
                        if (limitRequest.DayLimit != 0 && CheckToLimit("Day", limitRequest.DayLimit, 1440, out cacheValue))
                        {
                            BlockedToIP("Превышен дневной лимит на запросы", cacheValue.Expires);
                            memoryCache.Remove($"LimitRequestToHour-{IP}_{host}");
                            memoryCache.Remove($"LimitRequestToDay-{IP}_{host}");
                        }
                    }
                }
                #endregion
            }

            #region Защита от Brute Force
            if (Domain.StopBruteForce != BruteForceType.Not)
            {
                // Настройки
                var BrutConf = jsonDB.BruteForceConf;

                // Переменная для кеша
                CacheValue cacheValue;

                #region Локальный метод - "CheckToLimit"
                bool CheckToLimit(string keyName, int limit, int ExpiresToMinute, out CacheValue _cacheValue)
                {
                    string key = $"BruteForceTo{keyName}-{IP}_{host}";
                    if (memoryCache.TryGetValue(key, out CacheValue item))
                    {
                        _cacheValue = item;
                        if (item.value >= limit)
                            return true;
                    }

                    _cacheValue = null;
                    return false;
                }
                #endregion

                #region Локальный метод - "BlockedToIP"
                void BlockedToIP(string Msg, DateTime Expires)
                {
                    string KeyLimitRequestToBlockedWait = $"BruteForceToBlockedWait-{IP}_{host}";
                    if (memoryCache.TryGetValue(KeyLimitRequestToBlockedWait, out _))
                        return;
                    memoryCache.Set(KeyLimitRequestToBlockedWait, (byte)0, TimeSpan.FromMinutes(5));

                    // Записываем IP в кеш IPtables и журнал
                    SetBlockedToIPtables(Msg, Expires, null);

                    // Сносим временную запись
                    memoryCache.Remove(KeyLimitRequestToBlockedWait);
                }
                #endregion

                // Блокировка IP
                if (CheckToLimit("Day", BrutConf.DayLimit, 1440, out cacheValue) || CheckToLimit("Hour", BrutConf.HourLimit, 60, out cacheValue) || CheckToLimit("Minute", BrutConf.MinuteLimit, 1, out cacheValue))
                {
                    BlockedToIP("Защита от Brute Force", cacheValue.Expires);
                }

                // Была авторизация
                if (BruteForce.IsLogin(Domain.StopBruteForce, method, uri, FormData))
                {
                    #region Локальный метод - "SetValue"
                    void SetValue(string keyName, int ExpiresToMinute)
                    {
                        string key = $"BruteForceTo{keyName}-{IP}_{host}";
                        if (memoryCache.TryGetValue(key, out CacheValue item))
                        {
                            item.value++;
                        }
                        else
                        {
                            memoryCache.Set(key, new CacheValue() { value = 1, Expires = DateTime.Now.AddMinutes(ExpiresToMinute) }, TimeSpan.FromMinutes(ExpiresToMinute));
                        }
                    }
                    #endregion

                    // Обновляем счетчики
                    SetValue("Day", 1440);
                    SetValue("Hour", 60);
                    SetValue("Minute", 1);
                }
            }
            #endregion

            #region ViewBag
            if (jsonDB.Base.DebugEnabled)
            {
                viewBag.IP = IP;
                viewBag.jsonDomain = JsonConvert.SerializeObject(Domain, Formatting.Indented);
                viewBag.antiBotToGlobalConf = JsonConvert.SerializeObject(antiBotToGlobalConf, Formatting.Indented);
                viewBag.FormData = FormData;
            }
            
            viewBag.method = method;
            viewBag.host = host;
            viewBag.uri = uri;
            viewBag.Referer = context.Request.Headers["Referer"];
            viewBag.UserAgent = userAgent;
            #endregion

            #region Замена ответа - 302/код
            try
            {
                // Проверка url и GET аргументов
                if (Regex.IsMatch(uri, Domain.RuleReplaces.RuleGetToRegex, RegexOptions.IgnoreCase))
                {
                    // Проверка POST аргументов
                    if (method != "POST" || (method == "POST" && Regex.IsMatch($"{(new Regex(@"^(/[^\?\&]+)").Match(uri).Groups[1].Value)}{FormData}", Domain.RuleReplaces.RulePostToRegex, RegexOptions.IgnoreCase)))
                    {
                        // Разделяем URL на аргументы
                        var g = new Regex(@"^(/([^\?&]+)?((\?|&).*)?)$").Match(uri).Groups;
                        string args = g[3].Value.Replace("?", "&");

                        #region Локальный метод - GetRule
                        ModelCache.Rules.RuleReplace GetRule()
                        {
                            foreach (var item in Domain.RuleReplaces.Rules)
                            {
                                // Url не подходит
                                if (!Regex.IsMatch($"/{g[2].Value}", $"^{item.uri}$", RegexOptions.IgnoreCase))
                                    continue;

                                if (method == "POST")
                                {
                                    // Get или POST аргументы подходят
                                    if (Regex.IsMatch(args, item.GetArgsToRegex, RegexOptions.IgnoreCase) || Regex.IsMatch(FormData, item.PostArgsToRegex, RegexOptions.IgnoreCase))
                                        return item;
                                }
                                else
                                {
                                    // Get аргументы подходят
                                    if (Regex.IsMatch(args, item.GetArgsToRegex, RegexOptions.IgnoreCase))
                                        return item;
                                }
                            }

                            return null;
                        }
                        #endregion

                        // Находим правило которое подходит для нашего запроса
                        if (GetRule() is ModelCache.Rules.RuleReplace rule)
                        {
                            #region Локальный метод - "ReplaceArgs"
                            string ReplaceArgs(string _args, string regexArgs)
                            {
                                StringBuilder mass = new StringBuilder();
                                foreach (var arg in _args.Split('&'))
                                {
                                    if (string.IsNullOrWhiteSpace(arg) || !arg.Contains('='))
                                        continue;

                                    var tmpArg = arg.Split('=');

                                    // Список аргументов для замены
                                    if (Regex.IsMatch(regexArgs, tmpArg[0], RegexOptions.IgnoreCase))
                                    {
                                        mass.Append("&" + tmpArg[0] + "=" + Regex.Replace(tmpArg[1], rule.RegexWhite, ""));
                                    }
                                    else { mass.Append("&" + arg); }
                                }

                                return mass.ToString();
                            }
                            #endregion

                            #region Локальный метод - "ResponseContent"
                            Task ResponseContent(string _argsGet)
                            {
                                // Записываем данные пользователя
                                AddJurnalTo200();
                                SetCountRequestToHour(TypeRequest._200, host, Domain.confToLog.EnableCountRequest);

                                // Тип ответа
                                if (rule.TypeResponse == TypeResponseRule.kode)
                                {
                                    // Пользовательский код
                                    context.Response.ContentType = rule.ContentType;
                                    return context.Response.WriteAsync(rule.kode, context.RequestAborted);
                                }
                                else
                                {
                                    if (string.IsNullOrWhiteSpace(rule.ResponceUri))
                                    {
                                        // Если url для 302 не указан
                                        return RewriteTo.Local(context, g[2].Value + Regex.Replace(_argsGet, "^&", "?"));
                                    }
                                    else
                                    {
                                        // Редирект на указаный URL
                                        return RewriteTo.Local(context, rule.ResponceUri.Replace("{arg}", Regex.Replace(_argsGet, "^&", "?")));
                                    }
                                }
                            }
                            #endregion

                            // Если аргументы для проверки не указаны
                            if (rule.GetArgs == "" && rule.PostArgs == "") {
                                return ResponseContent(string.Empty);
                            }

                            #region argsGet / argsPOST
                            string argsGet = ReplaceArgs(args, rule.GetArgs);
                            string argsPOST = string.Empty;

                            if (method == "POST")
                            {
                                argsPOST = ReplaceArgs(FormData, rule.PostArgs);
                            }
                            #endregion

                            // Замена ответа, если в аргументах есть лишние символы
                            if (args != argsGet || FormData != argsPOST) {
                                return ResponseContent(argsGet);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Записываем данные ошибки в журнал
                AddJurnalTo500(ex.Message);
                SetCountRequestToHour(TypeRequest._500, host, Domain.confToLog.EnableCountRequest);

                // Выводим ошибку
                viewBag.IsErrorRule = true;
                viewBag.ErrorTitleException = "Ошибка в правиле";
                viewBag.ErrorRuleException = jsonDB.Base.DebugEnabled ? ex.Message : "Данные ошибки доступны в журнале 500";
                return View(context, viewBag, ActionCheckLink.deny);
            }
            #endregion

            // Переопределенные правила
            if (OpenPageToRule(Domain.RuleOverrideAllow, Domain.RuleOverride2FA, Domain.RuleOverrideDeny) is Task pageToRuleOverride)
                return pageToRuleOverride;

            // Обычные правила
            if (OpenPageToRule(Domain.RuleAllow, Domain.Rule2FA, Domain.RuleDeny) is Task pageToRule)
                return pageToRule;

            // Записываем данные пользователя
            AddJurnalTo403And303(Is303: true);
            SetCountRequestToHour(TypeRequest._303, host, Domain.confToLog.EnableCountRequest);

            // Если не одно правило не подошло
            return View(context, viewBag, ActionCheckLink.allow);

            #region Локальный метод - "OpenPageToRule"
            Task OpenPageToRule(ModelCache.Rules.Rule RuleAllow, ModelCache.Rules.Rule Rule2FA, ModelCache.Rules.Rule RuleDeny)
            {
                #region Разрешенные запросы
                if (IsRequestTheRules(RuleAllow))
                {
                    // Записываем данные пользователя
                    AddJurnalTo403And303(Is303: true);
                    SetCountRequestToHour(TypeRequest._303, host, Domain.confToLog.EnableCountRequest);

                    // Если режим дебага выключен
                    if (!jsonDB.Base.DebugEnabled)
                    {
                        context.Response.StatusCode = 303;
                        return context.Response.WriteAsync("303", context.RequestAborted);
                    }

                    // Разрешаем запрос
                    return View(context, viewBag, ActionCheckLink.allow);
                }
                #endregion

                #region Правила 2FA
                else if (IsRequestTheRules(Rule2FA))
                {
                    // Записываем данные пользователя
                    AddJurnalTo200(Is2FA: true);
                    SetCountRequestToHour(TypeRequest._200, host, Domain.confToLog.EnableCountRequest);

                    // Если IP для 2FA уже есть
                    if (memoryCache.TryGetValue(KeyToMemoryCache.CheckLinkWhitelistTo2FA(host, IP), out byte _))
                    {
                        // Авторизация в Telegram
                        if (!TelegramBot.IsAuth(IP, Is2FA: true))
                        {
                            context.Response.ContentType = "text/html";
                            return context.Response.WriteAsync(TelegramBot.AuthToHtml(IP), context.RequestAborted);
                        }

                        // Успех
                        return View(context, viewBag, ActionCheckLink.allow);
                    }

                    // Просим пройти 2FA авторизацию
                    viewBag.CoreAPI = jsonDB.Base.CoreAPI;
                    return View(context, viewBag, ActionCheckLink.Is2FA);
                }
                #endregion

                #region Запрещенные запросы
                else if (IsRequestTheRules(RuleDeny))
                {
                    // Записываем данные пользователя
                    AddJurnalTo403And303(Is403: true);
                    SetCountRequestToHour(TypeRequest._403, host, Domain.confToLog.EnableCountRequest);

                    // Отдаем страницу 403
                    return View(context, viewBag, ActionCheckLink.deny);
                }
                #endregion

                // Если не одно правило не подошло
                return null;
            }
            #endregion

            #region Локальный метод - "IsRequestTheRules"
            bool IsRequestTheRules(ModelCache.Rules.Rule rule)
            {
                #region Быстрая проверка 'GET/POST' запроса
                try
                {
                    switch (method)
                    {
                        case "GET":
                            return rule.RuleGetToRegex != "^$" && Regex.IsMatch(uri, rule.RuleGetToRegex, RegexOptions.IgnoreCase);
                        case "POST":
                            {
                                if (rule.RulePostToRegex != "^$" && Regex.IsMatch(uri, rule.RulePostToRegex, RegexOptions.IgnoreCase))
                                    return true;
                                break;
                            }
                        default:
                            {
                                // Записываем данные ошибки в журнал
                                AddJurnalTo500($"Метод '{method}' не поддерживается", IsException: true);
                                SetCountRequestToHour(TypeRequest._500, host, Domain.confToLog.EnableCountRequest);
                                return true;
                            }
                    }

                    // Быстрая проверка POST запроса
                    if (rule.RuleArgsCheckPostToRegex == "^$" || !Regex.IsMatch(uri, rule.RuleArgsCheckPostToRegex, RegexOptions.IgnoreCase))
                        return false;
                }
                catch (Exception ex)
                {
                    // Записываем данные ошибки в журнал
                    AddJurnalTo500(ex.Message);
                    SetCountRequestToHour(TypeRequest._500, host, Domain.confToLog.EnableCountRequest);

                    // Если есть ошибка в одном из регексов
                    viewBag.IsErrorRule = true;
                    viewBag.ErrorTitleException = "Ошибка в правиле";
                    viewBag.ErrorRuleException = jsonDB.Base.DebugEnabled ? ex.Message : "Данные ошибки доступны в журнале 500";
                    return true;
                }
                #endregion

                #region Полная проверка POST запроса
                // Если POST правил нету - (ложное срабатывание в 'RuleArgsCheckPostToRegex')
                if (rule.postRules == null && rule.postRules.Count < 1)
                    return false;

                foreach (var postRules in rule.postRules)
                {
                    try
                    {
                        if (Regex.IsMatch(uri, postRules.rule, RegexOptions.IgnoreCase))
                        {
                            // Проверяем есть ли в запросе POST аргументы не указаные в списке правил
                            if (Regex.IsMatch(FormData, postRules.RulePostToRegex, RegexOptions.IgnoreCase))
                                return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Записываем данные ошибки в журнал
                        AddJurnalTo500(ex.Message);
                        SetCountRequestToHour(TypeRequest._500, host, Domain.confToLog.EnableCountRequest);

                        // Если есть ошибка в одном из регексов
                        viewBag.IsErrorRule = true;
                        viewBag.ErrorTitleException = "Ошибка в правиле";
                        viewBag.ErrorRuleException = jsonDB.Base.DebugEnabled ? ex.Message : "Данные ошибки доступны в журнале 500";
                        return true;
                    }
                }
                #endregion

                // Если ничего не подошло
                return false;
            }
            #endregion

            #region Локальный метод - "AddJurnalTo500"
            void AddJurnalTo500(string errorMsg, bool IsException = false)
            {
                if (IsException)
                {
                    viewBag.IsErrorRule = true;
                    viewBag.ErrorTitleException = "Ошибка в запросе";
                    viewBag.ErrorRuleException = errorMsg;
                }

                if (Domain.confToLog.Jurn500 != WriteLogMode.off)
                {
                    // Игнорирование логов
                    if (!Regex.IsMatch(uri, Domain.IgnoreLogToRegex, RegexOptions.IgnoreCase))
                    {
                        var model = new Jurnal500()
                        {
                            IP = IP,
                            Host = host,
                            Method = method,
                            Uri = uri,
                            FormData = FormData,
                            UserAgent = userAgent,
                            Referer = context.Request.Headers["Referer"],
                            Time = DateTime.Now,
                            ErrorMsg = errorMsg
                        };

                        // Записываем данные в журнал
                        switch (Domain.confToLog.Jurn500)
                        {
                            case WriteLogMode.File:
                                WriteLogTo.FileStream(model);
                                break;
                            case WriteLogMode.SQL:
                                WriteLogTo.SQL(model);
                                break;
                            case WriteLogMode.all:
                                WriteLogTo.SQL(model);
                                WriteLogTo.FileStream(model);
                                break;
                        }
                    }
                }
            }
            #endregion

            #region Локальный метод - "AddJurnalTo403And303"
            void AddJurnalTo403And303(bool Is403 = false, bool Is303 = false)
            {
                if (viewBag.IsErrorRule)
                    return;

                // Игнорирование логов
                if (Domain.confToLog.IsActive && (Domain.confToLog.Jurn403 != WriteLogMode.off || Domain.confToLog.Jurn303 != WriteLogMode.off) && !Regex.IsMatch(uri, Domain.IgnoreLogToRegex, RegexOptions.IgnoreCase))
                {
                    ThreadPool.QueueUserWorkItem(ob =>
                    {
                        var geoIP = (Country: "Disabled", City: "Disabled", Region: "Disabled");
                        if (Domain.confToLog.EnableGeoIP)
                            geoIP = GeoIP2.City(IP);


                        #region 403
                        if (Is403)
                        {
                            var model = new Jurnal403()
                            {
                                IP = IP,
                                Host = host,
                                Method = method,
                                Uri = uri,
                                FormData = FormData,
                                UserAgent = userAgent,
                                Referer = Referer,
                                Country = geoIP.Country,
                                City = geoIP.City,
                                Region = geoIP.Region,
                                Time = DateTime.Now
                            };

                            // Записываем данные в журнал
                            switch (Domain.confToLog.Jurn403)
                            {
                                case WriteLogMode.File:
                                    WriteLogTo.FileStream(model);
                                    break;
                                case WriteLogMode.SQL:
                                    WriteLogTo.SQL(model);
                                    break;
                                case WriteLogMode.all:
                                    WriteLogTo.SQL(model);
                                    WriteLogTo.FileStream(model);
                                    break;
                            }
                        }
                        #endregion

                        #region 303
                        if (Is303)
                        {
                            var model = new Jurnal303()
                            {
                                IP = IP,
                                Host = host,
                                Method = method,
                                Uri = uri,
                                FormData = FormData,
                                UserAgent = userAgent,
                                Referer = Referer,
                                Country = geoIP.Country,
                                City = geoIP.City,
                                Region = geoIP.Region,
                                Time = DateTime.Now
                            };

                            // Записываем данные в журнал
                            switch (Domain.confToLog.Jurn303)
                            {
                                case WriteLogMode.File:
                                    WriteLogTo.FileStream(model);
                                    break;
                                case WriteLogMode.SQL:
                                    WriteLogTo.SQL(model);
                                    break;
                                case WriteLogMode.all:
                                    WriteLogTo.SQL(model);
                                    WriteLogTo.FileStream(model);
                                    break;
                            }
                        }
                        #endregion
                    });
                }
            }
            #endregion

            #region Локальный метод - "AddJurnalTo200"
            void AddJurnalTo200(bool IsAntiBot = false, bool Is2FA = false, bool IsIPtables = false)
            {
                // Игнорирование логов
                if (Domain.confToLog.IsActive && !Regex.IsMatch(uri, Domain.IgnoreLogToRegex, RegexOptions.IgnoreCase))
                {
                    ThreadPool.QueueUserWorkItem(ob =>
                    {
                        var geoIP = (Country: "Disabled", City: "Disabled", Region: "Disabled");
                        if (Domain.confToLog.EnableGeoIP)
                            geoIP = GeoIP2.City(IP);

                        #region Тип журнала
                        var typeJurn = TypeJurn200.Unknown;

                        if (IsAntiBot)
                            typeJurn = TypeJurn200.AntiBot;

                        if (Is2FA)
                            typeJurn = TypeJurn200._2FA;

                        if (IsIPtables)
                            typeJurn = TypeJurn200.IPtables;
                        #endregion

                        var model = new Jurnal200()
                        {
                            typeJurn = typeJurn,
                            IP = IP,
                            Host = host,
                            Method = method,
                            Uri = uri,
                            FormData = FormData,
                            UserAgent = userAgent,
                            Referer = Referer,
                            Country = geoIP.Country,
                            City = geoIP.City,
                            Region = geoIP.Region,
                            Time = DateTime.Now
                        };

                        // Записываем данные в журнал
                        switch (Domain.confToLog.Jurn200)
                        {
                            case WriteLogMode.File:
                                WriteLogTo.FileStream(model);
                                break;
                            case WriteLogMode.SQL:
                                WriteLogTo.SQL(model);
                                break;
                            case WriteLogMode.all:
                                WriteLogTo.FileStream(model);
                                break;
                        }
                    });
                }
            }
            #endregion

            #region Локальный метод - "SetBlockedToIPtables"
            void SetBlockedToIPtables(string Msg, DateTime Expires, string PtrHostName)
            {
                // Данные для статистики
                SetCountRequestToHour(TypeRequest._401, host, Domain.confToLog.EnableCountRequest);

                // Записываем IP в кеш IPtables
                memoryCache.Set(KeyToMemoryCache.IPtables(IP, host), new IPtables(Msg, Expires), Expires);

                // Дублируем информацию в SQL
                WriteLogTo.SQL(new BlockedIP()
                {
                    IP = IP,
                    BlockingTime = Expires,
                    Description = Msg,
                    typeBlockIP = TypeBlockIP.domain,
                    BlockedHost = host
                });

                // Игнорирование логов
                if (Domain.confToLog.IsActive && !Regex.IsMatch(uri, Domain.IgnoreLogToRegex, RegexOptions.IgnoreCase))
                {
                    var geoIP = (Country: "Disabled", City: "Disabled", Region: "Disabled");
                    if (Domain.confToLog.EnableGeoIP)
                        geoIP = GeoIP2.City(IP);

                    // Модель
                    Jurnal401 model = new Jurnal401()
                    {
                        Host = host,
                        IP = IP,
                        Msg = Msg,
                        Ptr = PtrHostName,
                        UserAgent = userAgent,
                        Country = geoIP.Country,
                        City = geoIP.City,
                        Region = geoIP.Region,
                        Time = DateTime.Now
                    };

                    // Записываем данные в журнал
                    switch (Domain.confToLog.Jurn401)
                    {
                        case WriteLogMode.File:
                            WriteLogTo.FileStream(model);
                            break;
                        case WriteLogMode.SQL:
                            WriteLogTo.SQL(model);
                            break;
                        case WriteLogMode.all:
                            WriteLogTo.SQL(model);
                            WriteLogTo.FileStream(model);
                            break;
                    }
                }
            }
            #endregion

            #region Локальный метод - "CheckLinkWhitelistToAllDomain"
            bool CheckLinkWhitelistToAllDomain()
            {
                // Глобальный доступ для IP ко всем сайтам
                // Глобальный доступ для IP в этому сайту
                if (memoryCache.TryGetValue(KeyToMemoryCache.CheckLinkWhitelistToAllDomain(IP), out byte _) || memoryCache.TryGetValue(KeyToMemoryCache.CheckLinkWhitelistToAll(host, IP), out byte _))
                    return true;

                // IP для проверки в формате /24
                string ipCache = Regex.Replace(IP, @"\.[0-9]+$", ""); ;

                // Глобальный доступ для IP ко всем сайтам
                // Глобальный доступ для IP в этому сайту
                if (memoryCache.TryGetValue(KeyToMemoryCache.CheckLinkWhitelistToAllDomain(ipCache), out byte _) || memoryCache.TryGetValue(KeyToMemoryCache.CheckLinkWhitelistToAll(host, ipCache), out byte _))
                    return true;
                
                // IP нету в белом списке
                return false;
            }
            #endregion
        }
        #endregion

        #region SetCountRequestToHour
        public static void SetCountRequestToHour(TypeRequest type, string host, bool EnableCountRequest)
        {
            #region Локальный метод - "SetCount"
            void SetCount(NumberOfRequestHour dt)
            {
                switch (type)
                {
                    case TypeRequest._200:
                        dt.Count200++;
                        break;
                    case TypeRequest._303:
                        dt.Count303++;
                        break;
                    case TypeRequest._403:
                        dt.Count403++;
                        break;
                    case TypeRequest._401:
                        dt.Count401++;
                        break;
                    case TypeRequest._500:
                        dt.Count500++;
                        break;
                    case TypeRequest._2fa:
                        dt.Count2FA++;
                        break;
                }
            }
            #endregion

            if (EnableCountRequest)
            {
                string keyNumberOfRequestToHour = KeyToMemoryCache.IspNumberOfRequestToHour(DateTime.Now);
                if (memoryCache.TryGetValue(keyNumberOfRequestToHour, out IDictionary<string, NumberOfRequestHour> DataNumberOfRequestDay))
                {
                    // Если хост есть в кеше
                    if (DataNumberOfRequestDay.TryGetValue(host, out NumberOfRequestHour dtValue))
                    {
                        SetCount(dtValue);
                    }

                    // Если хоста нету в кеше
                    else
                    {
                        var dt = new NumberOfRequestHour();
                        dt.Time = DateTime.Now;
                        SetCount(dt);
                        DataNumberOfRequestDay.Add(host, dt);
                    }
                }
                else
                {
                    // Считаем запрос
                    var dt = new NumberOfRequestHour();
                    dt.Time = DateTime.Now;
                    SetCount(dt);

                    // Создаем кеш
                    memoryCache.Set(keyNumberOfRequestToHour, new Dictionary<string, NumberOfRequestHour>() { [host] = dt }, TimeSpan.FromHours(2));
                }
            }
        }
        #endregion

        #region ViewDomainNotFound
        public static Task ViewDomainNotFound(HttpContext context)
        {
            context.Response.StatusCode = 500;
            return context.Response.WriteAsync(@"<!DOCTYPE html>
<html lang='ru-RU'>
<head>
    <title>Ошибка</title>
    <meta http-equiv='Content-Type' content='text/html; charset=utf-8'>
    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
    <meta name='viewport' content='width=device-width, initial-scale=1, user-scalable=no'>
    <link rel='stylesheet' href='/statics/style.css'>
</head>
<body>
    <div class='error'>
        <div class='error-block'>

            <div class='code'>500</div>
            <div class='title'>Домен не найден</div>
            <pre>Добавьте домен в ISPCore и настройте фильтр запросов</pre>

            <div class='copyright'>
                <div>
                    &copy; 2018 <strong>ISPCore</strong>. All rights reserved.
                </div>
                <div>
                    <a href='/'>Главная сайта</a> / <a href='http://core-system.org/' target='_blank'>Core System</a>
                </div>
            </div>

        </div>
    </div>
</body>
</html>", context.RequestAborted);
        }
        #endregion

        #region View
        public static Task View(HttpContext context, ViewBag viewBag, ActionCheckLink Model)
        {
            #region Код ответа
            if (viewBag.IsErrorRule)
            {
                context.Response.StatusCode = 500;
            }
            else
            {
                switch (Model)
                {
                    case ActionCheckLink.allow:
                        {
                            context.Response.StatusCode = 303;
                            break;

                        }
                    case ActionCheckLink.Is2FA:
                        {
                            context.Response.StatusCode = 200;
                            break;

                        }
                    case ActionCheckLink.deny:
                        {
                            context.Response.StatusCode = 403;
                            break;

                        }
                    default:
                        {
                            context.Response.StatusCode = 500;
                            break;
                        }
                }
            }
            #endregion

            #region Локальный метод - "RenderTitle"
            string RenderTitle()
            {
                if (viewBag.IsErrorRule)
                {
                    return "Ошибка";
                }
                else
                {
                    switch (Model)
                    {
                        case ActionCheckLink.allow:
                            return "303";
                        case ActionCheckLink.deny:
                            return "Доступ запрещен";
                        case ActionCheckLink.Is2FA:
                            return "Aвторизация 2FA";
                        default:
                            return "Неизвестная ошибка";
                    }
                }
            }
            #endregion

            #region Локальный метод - "RenderScript"
            string RenderScript()
            {
                if (Model == ActionCheckLink.Is2FA)
                {
                    return @"
<script>
    function unlock(e)
    {
        e.preventDefault();
        document.getElementById('unlockError').style.display = 'none';

        var password = document.getElementById('unlockPassword').value;

        $.post('" + viewBag.CoreAPI + "/unlock/2fa', { password: password, host: '" + viewBag.host + "', method: '" + viewBag.method + "', uri: '" + WebUtility.UrlEncode(viewBag.uri) + "', referer: '" + WebUtility.UrlEncode(viewBag.Referer) + @"' }, function (data)
        {
            var json = JSON.parse(JSON.stringify(data));

            if (json.msg) {
                document.getElementById('unlockError').style.display = 'block';
                document.getElementById('unlockError').innerText = json.msg;
            }
            else if (json.result) {
                window.location.reload();
            }
            else {
                document.getElementById('unlockError').style.display = 'block';
                document.getElementById('unlockError').innerText = 'Неизвестная ошибка';
            }
        })
    }
</script>";
                }

                return string.Empty;
            }
            #endregion

            #region Локальный метод - "RenderBody"
            string RenderBody()
            {
                if (viewBag.IsErrorRule)
                {
                    return @"<div class='code'>500</div>
                    <div class='title'>"+ viewBag.ErrorTitleException + @"</div>
                    <pre>" + viewBag.ErrorRuleException + @"</pre>";
                }
                else if (Model == ActionCheckLink.Is2FA)
                {
                    return @"<div class='code'>2FA</div>
                    <div class='title'>Aвторизация</div>
                    <pre>Введите пароль безопасности для 2FA</pre>

                    <form method='post' action='/' onsubmit='unlock(event)'>
                        <div class='form-group'>
                            <div class='input-group form'>
                                <span class='input-group-addon'><i class='fa fa-lock'></i></span>
                                <input class='form-control' id='unlockPassword' type='password' name='password'>
                            </div>

                            <button type='submit' class='btn-unlock'>Unlock</button>

                            <div id='unlockError' class='errorMsg'>eroror</div>
                        </div>
                    </form>";
                }
                else if (Model == ActionCheckLink.allow)
                {
                    return @"<div class='code'>303</div>
                    <div class='title'>Отправить в backend</div>";
                }

                else if (Model == ActionCheckLink.deny)
                {
                    return @"<div class='code'>403</div>
                    <div class='title'>Доступ запрещен</div>";
                }

                else
                {
                    return @"<div class='code'>500</div>
                    <div class='title'>Неизвестная ошибка</div>";
                }
            }
            #endregion

            #region Локальный метод - "RenderDebug"
            string RenderDebug()
            {
                if (viewBag.DebugEnabled)
                {
                    return @"
<!--
IP:         " + viewBag?.IP + @"
UserAgent:  " + viewBag?.UserAgent + @"
method:     " + viewBag?.method + @"
host:       " + viewBag?.host + @"
uri:        " + viewBag?.uri + @"
FormData:   " + viewBag?.FormData + @"
Referer:    " + viewBag?.Referer + @"

" + viewBag.antiBotToGlobalConf?.Replace("\\\\", "\\")?.Replace("<!--", "&lt;!--")?.Replace("-->", "--&gt;") + @"


" + viewBag.jsonDomain?.Replace("\\\\", "\\")?.Replace("<!--", "&lt;!--")?.Replace("-->", "--&gt;") + @"
-->
";
                }

                return string.Empty;
            }
            #endregion

            // Html ответ
            return context.Response.WriteAsync(@"<!DOCTYPE html>
<html lang='ru-RU'>
<head>
    <title>" + RenderTitle() + @"</title>
    <meta http-equiv='Content-Type' content='text/html; charset=utf-8'>
    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
    <meta name='viewport' content='width=device-width, initial-scale=1, user-scalable=no'>
    <link rel='stylesheet' href='/statics/style.css'>
    " + (Model == ActionCheckLink.Is2FA ? "<script type='text/javascript' src='/statics/jquery.min.js'></script>" : string.Empty) + @"
</head>
<body>

" + RenderScript() + @"

    <div class='error'>
        <div class='error-block'>

            " + RenderBody() + @"

            <div class='copyright'>
                <div>
                    &copy; 2018 <strong>ISPCore</strong>. All rights reserved.
                </div>
                <div>
                    <a href='/'>Главная сайта</a> / <a href='http://core-system.org/' target='_blank'>Core System</a>
                </div>
            </div>
        </div>
    </div>
</body>
</html>

" + RenderDebug() + @"
", context.RequestAborted);
        }
        #endregion
    }
}
