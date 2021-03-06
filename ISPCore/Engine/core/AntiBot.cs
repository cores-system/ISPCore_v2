﻿using ISPCore.Engine.Auth;
using ISPCore.Engine.Base;
using ISPCore.Engine.Hash;
using ISPCore.Models.core.Cache;
using ISPCore.Models.Databases.json;
using ISPCore.Models.RequestsFilter.Base.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Linq;
using ISPCore.Models.Base;
using ISPCore.Engine.Base.SqlAndCache;
using System.IO;
using Newtonsoft.Json;
using Trigger = ISPCore.Models.Triggers.Events.core.AntiBot;

namespace ISPCore.Engine.core
{
    public static class AntiBot
    {
        static AntiBotCacheToGlobalConf globalConf = null;

        #region GlobalConf
        /// <summary>
        /// Кеш настроек AntiBot
        /// </summary>
        /// <param name="conf">Оригинальные настройки AntiBotM</param>
        public static AntiBotCacheToGlobalConf GlobalConf(Models.Databases.json.AntiBot conf)
        {
            // Кеш настроек
            if (globalConf != null && globalConf.conf.LastUpdateToConf == conf.LastUpdateToConf)
                return globalConf;

            #region Локальный метод - "JoinMass"
            string JoinMass(List<string> mass)
            {
                if (mass == null || mass.Count == 0)
                    return "^$";

                return $"^({string.Join("|", mass)})$";
            }
            #endregion

            #region Локальный метод - "JoinText"
            string JoinText(string text)
            {
                if (string.IsNullOrWhiteSpace(text))
                    return "^$";

                List<string> mass = new List<string>();
                foreach (var line in Regex.Replace(text, "[\r\n]+", "\n").Split('\n'))
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    mass.Add(Regex.Replace(line, "(^\\^|\\$$)", ""));
                }

                // Успех
                return JoinMass(mass);
            }
            #endregion
            
            // Создаем кеш
            globalConf = new AntiBotCacheToGlobalConf();
            globalConf.conf = conf.Clone();

            // Переопределяем
            globalConf.DomainsToreCaptchaRegex = JoinText(conf.DomainsToreCAPTCHA);
            globalConf.conf.BackgroundCheckToAddExtensions = string.IsNullOrWhiteSpace(conf.BackgroundCheckToAddExtensions) ? "^$" : $"(\\.{conf.BackgroundCheckToAddExtensions.Replace(",", "|\\.")})";

            // Успех
            return globalConf;
        }
        #endregion

        #region ValidRequest
        /// <summary>
        /// Проверка запроса
        /// </summary>
        /// <param name="IP">IPv4/6</param>
        /// <param name="antiBotType">Cпособ проверки</param>
        /// <param name="HostConvert">Домен</param>
        /// <param name="method">Метод запроса</param>
        /// <param name="uri">Url запроса</param>
        /// <param name="HttpContext">Используется для проверки cookie</param>
        /// <param name="domain">Кеш настроек домена</param>
        /// <param name="DomainID">Id домена</param>
        /// <param name="outHtml">html для вывода пользователю</param>
        public static bool ValidRequest(string IP, AntiBotType antiBotType, string HostConvert, string method, string uri, HttpContext HttpContext, Models.core.Cache.CheckLink.Domain domain, int DomainID, out string outHtml)
        {
            // По умолчанию null
            outHtml = null;

            // Не выбран способ проверки
            if (antiBotType == AntiBotType.Off)
                return true;

            #region Проверка Cookie
            if (IsValidCookie(HttpContext, IP, domain.AntiBot.HashKey, out string _verification))
            {
                Trigger.OnValidCookie((IP, HostConvert, DomainID, true, _verification));
                return true;
            }
            else { Trigger.OnValidCookie((IP, HostConvert, DomainID, false, _verification)); }
            #endregion

            // IMemoryCache
            var memoryCache = Service.Get<IMemoryCache>();

            // База
            var jsonDB = Service.Get<JsonDB>();

            #region Отдаем данные с кеша
            if (jsonDB.Cache.AntiBot != 0 && memoryCache.TryGetValue(KeyToMemoryCache.AntiBotToCache(IP), out (string tplToUrl, string json, AntiBotType type) _cache))
            {
                outHtml = Html(_cache.tplToUrl, _cache.json);
                Trigger.OnResponseView((IP, HostConvert, DomainID, _cache.type));
                return false;
            }
            #endregion

            // Достаем настройки AntiBot из кеша
            var antiBotToGlobalConf = GlobalConf(jsonDB.AntiBot);

            #region Проверка User-Agent
            if (HttpContext.Request.Headers.TryGetValue("User-Agent", out var userAgent))
            {
                // Проверка пользовательского User-Agent
                if (WhiteUserList.IsWhiteUserAgent(userAgent))
                    return true;

                string SearchBot = "(" +
                // https://yandex.ru/support/webmaster/robot-workings/check-yandex-robots.html
                "YandexBot|YandexAccessibilityBot|YandexMobileBot|YandexDirectDyn|YandexScreenshotBot|YandexImages|YandexVideo|YandexVideoParser|YandexMedia|YandexBlogs|YandexFavicons|YandexWebmaster|YandexPagechecker|YandexImageResizer|YandexAdNet|YandexDirect|YaDirectFetcher|YandexCalendar|YandexSitelinks|YandexMetrika|YandexNews|YandexCatalog|YandexMarket|YandexVertis|YandexForDomain|YandexSpravBot|YandexSearchShop|YandexMedianaBot|YandexOntoDB|YandexVerticals" +
                "|" +
                // https://support.google.com/webmasters/answer/1061943
                "APIs-Google|Mediapartners-Google|AdsBot-Google|Googlebot" +
                "|" +
                // https://help.mail.ru/webmaster/indexing/robots/robot_log
                // https://www.bing.com/webmaster/help/how-to-verify-bingbot-3905dc26
                "Mail.RU_Bot|Bingbot|msnbot" +
                ")";

                // Проверка User-Agent на поискового бота
                if (Regex.IsMatch(userAgent, SearchBot, RegexOptions.IgnoreCase))
                {
                    #region Локальный метод - "DNSLookup"
                    bool DNSLookup()
                    {
                        string ptr = null;
                        string memKey = $"local-fb23a52e:DNSLookup-{IP}";
                        if (memoryCache.TryGetValue(memKey, out bool _cacheToDNSLookup))
                            return _cacheToDNSLookup;

                        // Создаем временный кеш на время проверки
                        memoryCache.Set(memKey, true, TimeSpan.FromMinutes(5));

                        try
                        {
                            // Получаем имя хоста по IP
                            var host = Dns.GetHostEntryAsync(IP).Result;

                            // Получаем IP хоста по имени
                            host = Dns.GetHostEntryAsync(host.HostName).Result;

                            // Сохраняем PTR
                            ptr = host.HostName;

                            // Проверяем имя хоста и IP на совпадение 
                            if (host.AddressList.Where(i => i.ToString() == IP).FirstOrDefault() != null)
                            {
                                // Проверяем имя хоста на белый список DNSLookup
                                if (Regex.IsMatch(host.HostName, @".*\.(yandex.(ru|net|com)|googlebot.com|google.com|mail.ru|search.msn.com)$", RegexOptions.IgnoreCase))
                                {
                                    // Добовляем IP в белый
                                    int HourCacheToBot = IsGlobalConf() ? antiBotToGlobalConf.conf.HourCacheToBot : domain.AntiBot.HourCacheToBot;
                                    WhitePtr.Add(IP, host.HostName, DateTime.Now.AddHours(HourCacheToBot));
                                    Trigger.OnAddToWhitePtr((IP, HostConvert, DomainID, ptr, HourCacheToBot));
                                    return true;
                                }
                            }
                        }
                        catch { }

                        // Записываем IP в кеш IPtables и журнал
                        if (Check.Request.SetBlockedToIPtables(domain, IP, HostConvert, "AntiBot", DateTime.Now.AddMinutes(40), uri, userAgent, ptr))
                            Trigger.OnBlockedIP((IP, ptr, HostConvert, DomainID, "AntiBot", 40));

                        // Не удалось проверить PTR-запись
                        memoryCache.Remove(memKey);
                        return false;
                    }
                    #endregion

                    #region Режим проверки поискового бота
                    if (IsGlobalConf() ? antiBotToGlobalConf.conf.FirstSkipToBot : domain.AntiBot.FirstSkipToBot)
                    {
                        // Проверяем DNSLookup в потоке
                        ThreadPool.QueueUserWorkItem(i => DNSLookup());
                    }
                    else
                    {
                        // Плохой бот
                        if (!DNSLookup()) {
                            outHtml = "Не удалось проверить PTR-запись";
                            return false;
                        }
                    }
                    #endregion

                    // Бот может зайти на сайт 
                    return true;
                }
            }
            #endregion

            // Нужна капча или нет
            bool IsRecaptcha = antiBotType == AntiBotType.reCAPTCHA;

            // Если капча установлена глобально, то нужно проверить домен в списке
            if (IsRecaptcha && IsGlobalConf() && antiBotToGlobalConf.conf.type == AntiBotType.reCAPTCHA)
                IsRecaptcha = Regex.IsMatch(HostConvert, antiBotToGlobalConf.DomainsToreCaptchaRegex, RegexOptions.IgnoreCase);
            
            // Выбираем настройки какого конфига использовать
            AntiBotBase antiBotConf = IsGlobalConf() ? (AntiBotBase)antiBotToGlobalConf.conf : (AntiBotBase)domain.AntiBot;

            #region Проверка пользователя в фоновом режиме
            if (antiBotConf.BackgroundCheck)
            {
                // Ключ для проверки запросов
                string memKey = $"Core:AntiBot/CountBackgroundRequest-{IP}";

                if (method != "GET" && method != "HEAD")
                {
                    // Если до этого был GET/HEAD запрос
                    if (memoryCache.TryGetValue(memKey, out _))
                        return true;
                }
                else
                {
                    int CountBackgroundRequest;
                    if (!memoryCache.TryGetValue(memKey, out CountBackgroundRequest))
                        CountBackgroundRequest = 0;

                    // Пользователь не привысил значение
                    if (antiBotConf.CountBackgroundRequest > CountBackgroundRequest)
                    {
                        // Увеличиваем количиство запросов
                        if (!uri.Contains(".") || Regex.IsMatch(uri, antiBotConf.BackgroundCheckToAddExtensions, RegexOptions.IgnoreCase))
                        {
                            // Записываем/Перезаписываем количиство запросов
                            memoryCache.Set(memKey, ++CountBackgroundRequest, TimeSpan.FromHours(antiBotConf.BackgroundHourCacheToIP));
                        }

                        // Без заглушки AntiBot
                        return true;
                    }
                }
            }
            #endregion

            
            // reCAPTCHA, SignalR или JavaScript
            var tplName = (!IsRecaptcha && antiBotType == AntiBotType.reCAPTCHA) ? AntiBotType.SignalR : antiBotType;
            outHtml = Html(tplName, antiBotConf, jsonDB.Base.CoreAPI, IP, HostConvert, jsonDB.Security.reCAPTCHASitekey, jsonDB.Cache.AntiBot, domain.AntiBot.HashKey);
            Trigger.OnResponseView((IP, HostConvert, DomainID, tplName));
            return false;

            #region Локальный метод - "IsGlobalConf"
            bool IsGlobalConf()
            {
                return antiBotToGlobalConf.conf.Enabled || domain.AntiBot.UseGlobalConf;
            }
            #endregion
        }
        #endregion

        #region ValidCookie
        /// <summary>
        /// Проверка Cookie
        /// </summary>
        /// <param name="verification">reCAPTCHA/SignalR/js</param>
        /// <param name="expired">Время жизни Cookie</param>
        /// <param name="key">Ключ</param>
        /// <param name="IP">IPv4/6</param>
        /// <param name="AntiBotHashKey">Дополнительный хеш для проверки "SignalR/JS"</param>
        private static bool ValidCookie(string verification, string expired, string key, string IP, string AntiBotHashKey)
        {
            return DateTime.FromBinary(long.Parse(expired)) > DateTime.Now && key == md5.text($"{expired}:{IP}:{(verification == "reCAPTCHA" ? verification : AntiBotHashKey)}:{PasswdTo.salt}");
        }

        /// <summary>
        /// Получить валидные Cookie
        /// </summary>
        /// <param name="HourCacheToUser">Сколько часов валидны Cookie</param>
        /// <param name="IP">IPv4/6</param>
        /// <param name="verification">Пользователь прошел проверку в "reCAPTCHA/SignalR/JS"</param>
        /// <param name="AntiBotHashKey">Дополнительный хеш для проверки "SignalR/JS"</param>
        public static string GetValidCookie(int HourCacheToUser, string IP, string verification, string AntiBotHashKey)
        {
            // Когда куки станут недействительны
            string expired = DateTime.Now.AddHours(HourCacheToUser).ToBinary().ToString();

            // Ключ для проверки
            string key = md5.text($"{expired}:{IP}:{(verification == "reCAPTCHA" ? verification : AntiBotHashKey)}:{PasswdTo.salt}");

            // Результат
            return $"{verification}:{expired}:{key}";
        }

        /// <summary>
        /// Проверка Cookie
        /// </summary>
        /// <param name="HttpContext"></param>
        /// <param name="IP">IPv4/6</param>
        /// <param name="AntiBotHashKey">Дополнительный хеш для проверки "SignalR/JS"</param>
        public static bool IsValidCookie(HttpContext HttpContext, string IP, string AntiBotHashKey, out string verification)
        {
            verification = null;
            if (HttpContext.Request.Cookies.TryGetValue("isp.ValidCookie", out var cookie))
            {
                // Получаем время и ключ
                var g = new Regex("([a-zA-Z]+):(-[0-9]+):([a-z0-9]+)").Match(cookie.ToString()).Groups;

                // Проверяем на пустоту
                if (!string.IsNullOrWhiteSpace(g[1].Value) && !string.IsNullOrWhiteSpace(g[2].Value) && !string.IsNullOrWhiteSpace(g[3].Value))
                {
                    verification = g[1].Value;

                    // Cookie еще активны
                    // Ключ валидный
                    if (ValidCookie(g[1].Value, g[2].Value, g[3].Value, IP, AntiBotHashKey))
                        return true;
                }
            }

            return false;
        }
        #endregion

        #region Html
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tplToUrl">Ссылка на шаблон</param>
        /// <param name="json">Json</param>
        public static string Html(string tplToUrl, string json)
        {
            return @"<!DOCTYPE html>
<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'></head>
<body>Please enable to JavaScript
<script>
var json = " + json + @";

function FinReplace(html){
	for(var i in json){
		html = html.replace(new RegExp('{isp:'+i+'}','g'),json[i]);
	}
	return html;
}

var xhr = new XMLHttpRequest();
xhr.open('GET', '" + tplToUrl + @"', false);
xhr.send();
if (xhr.status == 200) {
	document.body.innerHTML = '';
	document.write(FinReplace(xhr.responseText));
}
</script></body></html>";
        }


        /// <summary>
        /// Получить html для AntiBot
        /// </summary>
        /// <param name="tplName">Имя шаблона</param>
        /// <param name="conf">Настройки AntiBot</param>
        /// <param name="CoreApiUrl">Ссылка на /core/</param>
        /// <param name="IP">IPv4/6</param>
        /// <param name="HostConvert">Домен</param>
        /// <param name="reCAPTCHASitekey">Секретный ключ reCAPTCHA</param>
        /// <param name="CacheAntiBot">Кеширование ответа</param>
        /// <param name="AntiBotHashKey">Дополнительный хеш для проверки "SignalR/JS"</param>
        static string Html(AntiBotType tplName, AntiBotBase conf, string CoreApiUrl, string IP, string HostConvert, string reCAPTCHASitekey, int CacheAntiBot, string AntiBotHashKey)
        {
            #region Базовые параметры
            string tplToUrl = string.Empty;
            var mass = new Dictionary<string, string>();
            mass.Add("IP", IP);
            mass.Add("CoreApiUrl", CoreApiUrl);
            mass.Add("HourCacheToUser", conf.HourCacheToUser.ToString());
            mass.Add("WaitUser", conf.WaitUser.ToString());
            mass.Add("AddCodeToHtml", conf.AddCodeToHtml != null ? conf.AddCodeToHtml : string.Empty);
            mass.Add("JsToRewriteUser", JsToRewriteUser(conf.RewriteToOriginalDomain, HostConvert));
            #endregion

            #region Хеш и куки
            switch (tplName)
            {
                case AntiBotType.SignalR:
                    mass.Add("host", HostConvert);
                    mass.Add("AntiBotHashKey", AntiBotHashKey);
                    mass.Add("HashToSignalR", md5.text($"{IP}:{HostConvert}:{conf.HourCacheToUser}:{AntiBotHashKey}:{PasswdTo.salt}"));
                    tplToUrl = File.Exists($"{Folders.Tpl.AntiBot}/{tplName}.html") ? "/statics/tpl/AntiBot/SignalR.html" : "/statics/tpl/AntiBot/default/SignalR.html";
                    break;
                case AntiBotType.reCAPTCHA:
                    mass.Add("reCAPTCHASitekey", reCAPTCHASitekey);
                    mass.Add("HashToreCAPTCHA", md5.text($"{IP}:{conf.HourCacheToUser}:{PasswdTo.salt}"));
                    tplToUrl = File.Exists($"{Folders.Tpl.AntiBot}/{tplName}.html") ? "/statics/tpl/AntiBot/reCAPTCHA.html" : "/statics/tpl/AntiBot/default/reCAPTCHA.html";
                    break;
                case AntiBotType.CookieAndJS:
                    string cookie = GetValidCookie(conf.HourCacheToUser, IP, "js", AntiBotHashKey);
                    mass.Add("ValidCookie", cookie);
                    mass.Add("AntiBotHashKey", AntiBotHashKey);
                    Trigger.OnSetValidCookie((IP, HostConvert, cookie, "js", conf.HourCacheToUser));
                    tplToUrl = File.Exists($"{Folders.Tpl.AntiBot}/{tplName}.html") ? "/statics/tpl/AntiBot/CookieAndJS.html" : "/statics/tpl/AntiBot/default/CookieAndJS.html";
                    break;
            }
            #endregion

            // Сериализуем данные
            string json = JsonConvert.SerializeObject(mass);
            
            #region Создаем кеш
            if (CacheAntiBot != 0)
            {
                //IMemoryCache
                var memoryCache = Service.Get<IMemoryCache>();
                memoryCache.Set(KeyToMemoryCache.AntiBotToCache(IP), (tplToUrl, json, tplName), TimeSpan.FromMilliseconds(CacheAntiBot));
            }
            #endregion

            // Успех
            return Html(tplToUrl, json);
        }
        #endregion

        #region JsToBase64
        /// <summary>
        /// 
        /// </summary>
        /// <param name="RewriteToOriginalDomain"></param>
        public static string JsToBase64(bool RewriteToOriginalDomain)
        {
            if (!RewriteToOriginalDomain)
                return string.Empty;

            return @"
var Base64 = 
{	
	_keyStr : " + "\"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=" + "\"" + @",

	decode : function (input) {
		var output = " + "\"\"" + @";
		var chr1, chr2, chr3;
		var enc1, enc2, enc3, enc4;
		var i = 0;

		input = input.replace(/[^A-Za-z0-9\+\/\=]/g, " + "\"\"" + @");

		while (i < input.length) {

			enc1 = this._keyStr.indexOf(input.charAt(i++));
			enc2 = this._keyStr.indexOf(input.charAt(i++));
			enc3 = this._keyStr.indexOf(input.charAt(i++));
			enc4 = this._keyStr.indexOf(input.charAt(i++));

			chr1 = (enc1 << 2) | (enc2 >> 4);
			chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
			chr3 = ((enc3 & 3) << 6) | enc4;

			output = output + String.fromCharCode(chr1);

			if (enc3 != 64) {
				output = output + String.fromCharCode(chr2);
			}
			if (enc4 != 64) {
				output = output + String.fromCharCode(chr3);
			}

		}

		return Base64._utf8_decode(output);
	},

	_utf8_decode : function (utftext) {
		var string = " + "\"\"" + @";
		var i = 0;
		var c = c1 = c2 = 0;

		while ( i < utftext.length ) {

			c = utftext.charCodeAt(i);

			if (c < 128) {
				string += String.fromCharCode(c);
				i++;
			}
			else if((c > 191) && (c < 224)) {
				c2 = utftext.charCodeAt(i+1);
				string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
				i += 2;
			}
			else {
				c2 = utftext.charCodeAt(i+1);
				c3 = utftext.charCodeAt(i+2);
				string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
				i += 3;
			}

		}

		return string;
	}
}
";
        }
        #endregion

        #region JsToRewriteUser
        /// <summary>
        /// 
        /// </summary>
        /// <param name="RewriteToOriginalDomain"></param>
        /// <param name="HostConvert"></param>
        public static string JsToRewriteUser(bool RewriteToOriginalDomain, string HostConvert)
        {
            if (!RewriteToOriginalDomain)
                return "if (false) { }";

            return @"if (" + $"!\"{HostConvert}\".match(/.isp$/) && " + " location.hostname.replace(/www./gi,'') != Base64.decode('" + base64.Encode(HostConvert) + @"')) { window.location = Base64.decode('" + base64.Encode("http://" + HostConvert) + @"'); }";
        }
        #endregion
    }
}
