using ISPCore.Engine.Auth;
using ISPCore.Engine.Base;
using ISPCore.Engine.Base.SqlAndCache;
using ISPCore.Engine.core.Cache.CheckLink;
using ISPCore.Engine.Hash;
using ISPCore.Engine.Middleware;
using ISPCore.Engine.Security;
using ISPCore.Models.core;
using ISPCore.Models.core.Cache.CheckLink;
using ISPCore.Models.core.Cache.CheckLink.Common;
using ISPCore.Models.RequestsFilter.Base.Enums;
using ISPCore.Models.RequestsFilter.Domains;
using ISPCore.Models.RequestsFilter.Domains.Log;
using ISPCore.Models.RequestsFilter.Domains.Types;
using ISPCore.Models.RequestsFilter.Monitoring;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ModelCache = ISPCore.Models.core.Cache.CheckLink;
using ModelIPtables = ISPCore.Models.Security.IPtables;

namespace ISPCore.Engine.core.Check
{
    public partial class Request
    {
        public static Task Check(HttpContext context)
        {
            #region Получаем параметры запроса
            StringBuilder tmp_uri = new StringBuilder();
            string IP = string.Empty, uri = string.Empty, host = string.Empty, method = string.Empty;

            #region Локальный метод - "AddArgsToUri"
            void AddArgsToUri(string key, StringValues mass)
            {
                if (mass.Count > 1)
                {
                    foreach (string value in mass.Skip(1))
                        tmp_uri.Append($"&{key}={value}");
                }
            }
            #endregion
            
            foreach (var item in context.Request.Query)
            {
                switch (item.Key)
                {
                    case "ip":
                        IP = item.Value.First();
                        AddArgsToUri(item.Key, item.Value);
                        break;
                    case "method":
                        method = item.Value.First().ToUpper();
                        AddArgsToUri(item.Key, item.Value);
                        break;
                    case "host":
                        host = Regex.Replace(item.Value.First().ToLower(), @"^www\.", "", RegexOptions.IgnoreCase).Trim();
                        AddArgsToUri(item.Key, item.Value);
                        break;
                    case "uri":
                        uri = item.Value.First();
                        AddArgsToUri(item.Key, item.Value);
                        break;
                    default:
                        tmp_uri.Append($"&{item.Key}={item.Value}");
                        break;
                }
            }
            
            // Форматируем url
            uri = WebUtility.UrlDecode(uri + tmp_uri.ToString());
            #endregion

            #region Получаем параметры POST запроса
            string FormData = string.Empty;
            if (context.Request.Method == "POST" && context.Request.HasFormContentType)
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
                return jsonDB.Base.EnableToDomainNotFound ? ViewDomainNotFound(context) : View(context, viewBag, ActionCheckLink.allow, TypeRequest._303, NotCache: true);

            // Если у IP есть полный доступ к сайтам или к сайту
            if (CheckLinkWhitelistToAllDomain())
                return View(context, viewBag, ActionCheckLink.allow, TypeRequest._303, NotCache: true);

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

            #region Проверяем "IP/User-Agent" в блокировке IPtables
            // Проверяем IP в блокировке IPtables по домену
            if (IPtables.CheckIP(IP, memoryCache, out ModelIPtables BlockedData, host))
            {
                // Логируем пользователя
                AddJurnalTo200(IsIPtables: true);
                SetCountRequestToHour(TypeRequest.IPtables, host, Domain.confToLog.EnableCountRequest);

                // StatusCode
                context.Response.StatusCode = 401;
                if (Startup.cmd.StatusCode.IPtables)
                    return Task.FromResult(true);

                // Отдаем html
                context.Response.ContentType = "text/html; charset=utf-8";
                return context.Response.WriteAsync(IPtables.BlockedToHtml(IP, BlockedData.Description, BlockedData.TimeExpires), context.RequestAborted);
            }

            // Проверяем User-Agent в блокировке IPtables
            if (IPtables.CheckUserAgent(userAgent))
            {
                // Логируем пользователя
                AddJurnalTo200(IsIPtables: true);
                SetCountRequestToHour(TypeRequest.IPtables, host, Domain.confToLog.EnableCountRequest);

                // Код ответа
                context.Response.StatusCode = 401;
                if (Startup.cmd.StatusCode.IPtables)
                    return Task.FromResult(true);

                // Отдаем html
                context.Response.ContentType = "text/html; charset=utf-8";
                return context.Response.WriteAsync(IPtables.BlockedToHtml("Ваш User-Agent в списке запрещенных"), context.RequestAborted);
            }
            #endregion

            // Статистика запросов - "req/m"
            SetCountRequestToMinute(IP, TypeRequest.All, host, DomainID, Domain.confToLog.EnableCountRequest);

            // Достаем настройки AntiBot из кеша
            var antiBotToGlobalConf = AntiBot.GlobalConf(jsonDB.AntiBot);

            // Что-бы лишний раз не дергать WhiteList
            AntiBotType antiBotType = (antiBotToGlobalConf.conf.Enabled || Domain.AntiBot.UseGlobalConf) ? antiBotToGlobalConf.conf.type : Domain.AntiBot.type;
            bool LimitRequestEnabled = Domain.limitRequest.IsEnabled || antiBotToGlobalConf.conf.limitRequest.IsEnabled;

            // IP нету в системном белом списке
            // IP нету в пользовательском белом списке
            if ((antiBotType != AntiBotType.Off || LimitRequestEnabled) &&
                !WhitePtr.IsWhiteIP(IP) && !WhiteUserList.IsWhiteIP(IP))
            {
                #region Лимит запросов
                if (LimitRequestEnabled)
                {
                    // Настройки лимита запросов
                    var limitRequest = (antiBotToGlobalConf.conf.limitRequest.IsEnabled || Domain.limitRequest.UseGlobalConf) ? antiBotToGlobalConf.conf.limitRequest : Domain.limitRequest;

                    // Проверяем белый список UserAgent
                    if (!WhiteUserList.IsWhiteUserAgent(userAgent))
                    {
                        #region Локальный метод - "IsWhitePtr"
                        bool IsWhitePtr(out string PtrHostName)
                        {
                            #region Кеш ответа
                            string memKey = $"local-YnAqLmG:IsWhitePtr-{IP}";
                            if (memoryCache.TryGetValue(memKey, out (bool res, string PtrHostName, DateTime LastUpdateToConf) _cache) && WhiteUserList.LastUpdateCache == _cache.LastUpdateToConf)
                            {
                                PtrHostName = _cache.PtrHostName;
                                return _cache.res;
                            }
                            #endregion
                            
                            PtrHostName = null;

                            // На время проверки добавляем IP в белый список 
                            memoryCache.Set(memKey, (true, PtrHostName, WhiteUserList.LastUpdateCache), TimeSpan.FromMinutes(5));

                            #region DNSLookup
                            try
                            {
                                // Белый список Ptr
                                string WhitePtrRegex = WhiteUserList.PtrRegex;
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
                                            WhitePtr.Add(IP, DnsHost.HostName, DateTime.Now.AddDays(9));

                                            // Успех
                                            return true;
                                        }
                                    }

                                }
                            }
                            catch { }
                            #endregion

                            // Запрещаем повторную проверку IP в течении 3х часов
                            memoryCache.Set(memKey, (false, PtrHostName, WhiteUserList.LastUpdateCache), TimeSpan.FromHours(3));

                            // IP нету в белом списке PTR
                            return false;
                        }
                        #endregion

                        #region Локальный метод - "СheckingToreCAPTCHA"
                        bool СheckingToreCAPTCHA(int ExpiresToMinute)
                        {
                            // Проверяем Ptr
                            if (IsWhitePtr(out _))
                                return false;

                            #region Валидный пользователь
                            if (memoryCache.TryGetValue(KeyToMemoryCache.LimitRequestToreCAPTCHA(IP), out (int countRequest, int ExpiresToMinute) item))
                            {
                                // Пользователь превысил допустимый лимит
                                if (item.countRequest >= limitRequest.MaxRequestToAgainСheckingreCAPTCHA)
                                {
                                    // Удаляем запись
                                    memoryCache.Remove(KeyToMemoryCache.LimitRequestToreCAPTCHA(IP));
                                    return false;
                                }

                                // Считаем количество запросов
                                item.countRequest++;
                                memoryCache.Set(KeyToMemoryCache.LimitRequestToreCAPTCHA(IP), item, TimeSpan.FromMinutes(item.ExpiresToMinute));
                                return false;
                            }
                            #endregion

                            // Путь к шаблону
                            string tplToUrl = File.Exists($"{Folders.Tpl.LimitRequest}/reCAPTCHA.html") ? "/statics/tpl/LimitRequest/reCAPTCHA.html" : "/statics/tpl/LimitRequest/default/reCAPTCHA.html";

                            // Параметры для замены полей
                            string json = "{CoreApiUrl: '"+ jsonDB.Base.CoreAPI + "', reCAPTCHASitekey: '"+ jsonDB.Security.reCAPTCHASitekey + "', IP: '"+ IP + "', ExpiresToMinute: '" + ExpiresToMinute.ToString() + "', hash: '" + md5.text($"{IP}:{ExpiresToMinute}:{PasswdTo.salt}") + "'}";                        

                            // Ответ
                            context.Response.ContentType = "text/html; charset=utf-8";
                            context.Response.WriteAsync(AntiBot.Html(tplToUrl, json), context.RequestAborted);
                            return true;
                        }
                        #endregion

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
                            if (IsWhitePtr(out string PtrHostName))
                                return;

                            // Записываем IP в кеш IPtables и журнал
                            SetBlockedToIPtables(Domain, IP, host, Msg, Expires, uri, userAgent, PtrHostName);
                        }
                        #endregion

                        // Переменная для кеша
                        CacheValue cacheValue;

                        #region Метод блокировки
                        LimitToBlockType BlockType = limitRequest.BlockType;
                        if (BlockType == LimitToBlockType.reCAPTCHA && (antiBotToGlobalConf.conf.limitRequest.IsEnabled || Domain.limitRequest.UseGlobalConf))
                        {
                            // Если домена нету в глобальных настройках
                            if (!Regex.IsMatch(host, antiBotToGlobalConf.DomainsToreCaptchaRegex, RegexOptions.IgnoreCase))
                                BlockType = LimitToBlockType._403;
                        }
                        #endregion

                        #region Минутный лимит
                        if (limitRequest.MinuteLimit != 0 && CheckToLimit("Minute", limitRequest.MinuteLimit, 1, out cacheValue))
                        {
                            switch (BlockType)
                            {
                                case LimitToBlockType._403:
                                    BlockedToIP("Превышен минутный лимит на запросы", cacheValue.Expires);
                                    memoryCache.Remove($"LimitRequestToMinute-{IP}_{host}");
                                    break;
                                case LimitToBlockType.reCAPTCHA:
                                    {
                                        if (СheckingToreCAPTCHA(2))
                                            return Task.FromResult(true);
                                    }
                                    break;
                            }
                        }
                        #endregion

                        #region Часовой лимит
                        if (limitRequest.HourLimit != 0 && CheckToLimit("Hour", limitRequest.HourLimit, 60, out cacheValue))
                        {
                            switch (BlockType)
                            {
                                case LimitToBlockType._403:
                                    BlockedToIP("Превышен часовой лимит на запросы", cacheValue.Expires);
                                    memoryCache.Remove($"LimitRequestToHour-{IP}_{host}");
                                    break;
                                case LimitToBlockType.reCAPTCHA:
                                    {
                                        if (СheckingToreCAPTCHA(61))
                                            return Task.FromResult(true);
                                    }
                                    break;
                            }
                        }
                        #endregion

                        #region Дневной лимит
                        if (limitRequest.DayLimit != 0 && CheckToLimit("Day", limitRequest.DayLimit, 1440, out cacheValue))
                        {
                            switch (BlockType)
                            {
                                case LimitToBlockType._403:
                                    BlockedToIP("Превышен дневной лимит на запросы", cacheValue.Expires);
                                    memoryCache.Remove($"LimitRequestToHour-{IP}_{host}");
                                    memoryCache.Remove($"LimitRequestToDay-{IP}_{host}");
                                    break;
                                case LimitToBlockType.reCAPTCHA:
                                    {
                                        if (СheckingToreCAPTCHA(1441))
                                            return Task.FromResult(true);
                                    }
                                    break;
                            }
                        }
                        #endregion
                    }
                }
                #endregion

                #region AntiBot
                if (!AntiBot.ValidRequest(IP, antiBotType, host, method, uri, context, Domain, out string outHtml))
                {
                    // Логируем пользователя
                    AddJurnalTo200(IsAntiBot: true);
                    SetCountRequestToHour(TypeRequest._200, host, Domain.confToLog.EnableCountRequest);

                    // Выводим html пользователю
                    context.Response.ContentType = "text/html; charset=utf-8";
                    return context.Response.WriteAsync(outHtml, context.RequestAborted);
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
                    SetBlockedToIPtables(Domain, IP, host, Msg, Expires, uri, userAgent, null);

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

            viewBag.CreateCacheView = Domain.CreateTime;
            viewBag.method = method;
            viewBag.host = host;
            viewBag.uri = uri;
            viewBag.Referer = Referer;
            viewBag.UserAgent = userAgent;
            #endregion

            #region Кеш ответа
            if (jsonDB.Cache.Checklink != 0)
            {
                // Кеш есть и он валиден
                if (memoryCache.TryGetValue(KeyToMemoryCache.CheckLinkToCache(method, host, uri), out ResponseView responseView) && responseView.CacheTime == Domain.CreateTime)
                {
                    #region Записываем данные запроса
                    switch (responseView.TypeRequest)
                    {
                        case TypeRequest._200:
                            AddJurnalTo200();
                            break;
                        case TypeRequest._303:
                            AddJurnalTo403And303(Is303: true);
                            break;
                        case TypeRequest._403:
                            AddJurnalTo403And303(Is403: true);
                            break;
                    }
                    #endregion

                    // Счетчик запросов
                    SetCountRequestToHour(responseView.TypeRequest, host, Domain.confToLog.EnableCountRequest);

                    #region Кеш - "Замена ответа"
                    if (responseView.Is303)
                    {
                        if (string.IsNullOrEmpty(responseView.ResponceUri))
                        {
                            // Пользовательский код
                            context.Response.ContentType = responseView.ContentType;
                            return context.Response.WriteAsync(responseView.kode, context.RequestAborted);
                        }
                        else
                        {
                            // Редирект на указаный URL
                            return RewriteTo.Local(context, responseView.ResponceUri);
                        }
                    }
                    #endregion

                    // Отдаем кеш
                    viewBag.IsCacheView = true;
                    viewBag.IsErrorRule = responseView.IsErrorRule;
                    return View(context, viewBag, responseView.ActionCheckLink, responseView.TypeRequest);
                }
            }
            #endregion

            #region Замена ответа - 302/код
            try
            {
                // Проверка url и GET аргументов
                if (Domain.CheckRuleToReplace && Regex.IsMatch(uri, Domain.RuleReplaces.RuleGetToRegex, RegexOptions.IgnoreCase))
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
                                #region Локальный метод - "SetCacheToView"
                                void SetCacheToView(string _responceUri = null)
                                {
                                    if (jsonDB.Cache.Checklink != 0)
                                    {
                                        memoryCache.Set(KeyToMemoryCache.CheckLinkToCache(viewBag.method, viewBag.host, viewBag.uri), new ResponseView()
                                        {
                                            Is303 = true,
                                            CacheTime = Domain.CreateTime,
                                            TypeRequest = TypeRequest._200,
                                            ContentType = rule.ContentType,
                                            kode = rule.kode,
                                            ResponceUri = _responceUri,

                                        }, TimeSpan.FromMilliseconds(jsonDB.Cache.Checklink));
                                    }
                                }
                                #endregion

                                // Записываем данные пользователя
                                AddJurnalTo200();
                                SetCountRequestToHour(TypeRequest._200, host, Domain.confToLog.EnableCountRequest);

                                // Тип ответа
                                if (rule.TypeResponse == TypeResponseRule.kode)
                                {
                                    // Кеш
                                    SetCacheToView();

                                    // Пользовательский код
                                    context.Response.ContentType = rule.ContentType;
                                    return context.Response.WriteAsync(rule.kode, context.RequestAborted);
                                }
                                else
                                {
                                    if (string.IsNullOrWhiteSpace(rule.ResponceUri))
                                    {
                                        // Кеш
                                        string res = g[2].Value + Regex.Replace(_argsGet, "^&", "?");
                                        SetCacheToView(res);

                                        // Если url для 302 не указан
                                        return RewriteTo.Local(context, res);
                                    }
                                    else
                                    {
                                        // Кеш
                                        string res = rule.ResponceUri.Replace("{arg}", Regex.Replace(_argsGet, "^&", "?"));
                                        SetCacheToView(res);

                                        // Редирект на указаный URL
                                        return RewriteTo.Local(context, res);
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
                return View(context, viewBag, ActionCheckLink.deny, TypeRequest._500);
            }
            #endregion

            // Переопределенные правила
            if (Domain.CheckRuleToOverride && OpenPageToRule(Domain.RuleOverrideAllow, Domain.RuleOverride2FA, Domain.RuleOverrideDeny) is Task pageToRuleOverride)
                return pageToRuleOverride;

            // Обычные правила
            if (Domain.CheckRuleToBase && OpenPageToRule(Domain.RuleAllow, Domain.Rule2FA, Domain.RuleDeny) is Task pageToRule)
                return pageToRule;

            // Записываем данные запроса
            AddJurnalTo403And303(Is303: true);
            SetCountRequestToHour(TypeRequest._303, host, Domain.confToLog.EnableCountRequest);
            SetCountRequestToMinute(IP, TypeRequest._303, host, DomainID, Domain.confToLog.EnableCountRequest);

            // Если не одно правило не подошло
            return View(context, viewBag, ActionCheckLink.allow, TypeRequest._303);

            #region Локальный метод - "OpenPageToRule"
            Task OpenPageToRule(ModelCache.Rules.Rule RuleAllow, ModelCache.Rules.Rule Rule2FA, ModelCache.Rules.Rule RuleDeny)
            {
                #region Разрешенные запросы
                if (IsRequestTheRules(RuleAllow))
                {
                    // Записываем данные пользователя
                    AddJurnalTo403And303(Is303: true);
                    SetCountRequestToHour(TypeRequest._303, host, Domain.confToLog.EnableCountRequest);
                    SetCountRequestToMinute(IP, TypeRequest._303, host, DomainID, Domain.confToLog.EnableCountRequest);

                    // Разрешаем запрос
                    return View(context, viewBag, ActionCheckLink.allow, TypeRequest._303);
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
                        return View(context, viewBag, ActionCheckLink.allow, TypeRequest._200);

                    // Просим пройти 2FA авторизацию
                    viewBag.CoreAPI = jsonDB.Base.CoreAPI;
                    return View(context, viewBag, ActionCheckLink.Is2FA, TypeRequest._200);
                }
                #endregion

                #region Запрещенные запросы
                else if (IsRequestTheRules(RuleDeny))
                {
                    // Записываем данные пользователя
                    AddJurnalTo403And303(Is403: true);
                    SetCountRequestToHour(TypeRequest._403, host, Domain.confToLog.EnableCountRequest);

                    // Отдаем страницу 403
                    return View(context, viewBag, ActionCheckLink.deny, TypeRequest._403);
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
                        case "HEAD":
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
                if (Domain.confToLog.IsActive && Domain.confToLog.Jurn200 != WriteLogMode.off && !Regex.IsMatch(uri, Domain.IgnoreLogToRegex, RegexOptions.IgnoreCase))
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

            #region Локальный метод - "CheckLinkWhitelistToAllDomain"
            bool CheckLinkWhitelistToAllDomain()
            {
                // Глобальный доступ для IP ко всем сайтам
                // Глобальный доступ для IP в этому сайту
                if (memoryCache.TryGetValue(KeyToMemoryCache.CheckLinkWhitelistToAllDomain(IP), out byte _) || memoryCache.TryGetValue(KeyToMemoryCache.CheckLinkWhitelistToAll(host, IP), out byte _))
                    return true;

                // IP для проверки в формате /24
                string ipCache = Regex.Replace(IP, @"\.[0-9]+$", "");

                // Глобальный доступ для IP ко всем сайтам
                // Глобальный доступ для IP в этому сайту
                if (memoryCache.TryGetValue(KeyToMemoryCache.CheckLinkWhitelistToAllDomain(ipCache), out byte _) || memoryCache.TryGetValue(KeyToMemoryCache.CheckLinkWhitelistToAll(host, ipCache), out byte _))
                    return true;
                
                // IP нету в белом списке
                return false;
            }
            #endregion
        }
    }
}
