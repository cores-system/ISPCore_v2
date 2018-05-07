using ISPCore.Engine.Auth;
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
using ISPCore.Models.Security;
using ISPCore.Models.RequestsFilter.Monitoring;
using ISPCore.Models.Databases;
using ISPCore.Models.RequestsFilter.Domains;
using ISPCore.Models.RequestsFilter.Domains.Log;
using System.Linq;
using ISPCore.Models.Base;
using ISPCore.Engine.Base.SqlAndCache;
using System.IO;
using Newtonsoft.Json;
using System.Text;

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
        /// 
        /// </summary>
        /// <param name="antiBotType"></param>
        /// <param name="HostConvert"></param>
        /// <param name="method"></param>
        /// <param name="HttpContext"></param>
        /// <param name="domain"></param>
        /// <param name="outHtml"></param>
        public static bool ValidRequest(string IP, AntiBotType antiBotType, string HostConvert, string method, string uri, HttpContext HttpContext, Models.core.Cache.CheckLink.Domain domain, out string outHtml)
        {
            // По умолчанию null
            outHtml = null;

            // Не выбран способ проверки
            if (antiBotType == AntiBotType.Off)
                return true;

            // Проверка Cookie
            if (IsValidCookie(HttpContext, IP))
                return true;

            // IMemoryCache
            var memoryCache = Service.Get<IMemoryCache>();

            // База
            var jsonDB = Service.Get<JsonDB>();

            #region Отдаем данные с кеша
            if (jsonDB.Cache.AntiBot != 0 && memoryCache.TryGetValue(KeyToMemoryCache.AntiBotToCache(IP), out (string tplToUrl, string json) _cache))
            {
                outHtml = Html(_cache.tplToUrl, _cache.json);
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
                                    WhitePtr.Add(IP, host.HostName, DateTime.Now.AddHours(IsGlobalConf() ? antiBotToGlobalConf.conf.HourCacheToBot : domain.AntiBot.HourCacheToBot));
                                    return true;
                                }
                            }
                        }
                        catch { }

                        // Записываем IP в кеш IPtables и журнал
                        Check.Request.SetBlockedToIPtables(domain, IP, HostConvert, "AntiBot", DateTime.Now.AddMinutes(40), uri, userAgent, ptr);

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
            outHtml = Html(tplName, antiBotConf, jsonDB.Base.CoreAPI, IP, HostConvert, jsonDB.Security.reCAPTCHASitekey, jsonDB.Cache.AntiBot);
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
        /// 
        /// </summary>
        /// <param name="expired"></param>
        /// <param name="IP"></param>
        /// <param name="key"></param>
        private static bool ValidCookie(string expired, string IP, string key)
        {
            return DateTime.FromBinary(long.Parse(expired)) > DateTime.Now && key == md5.text($"{expired}:{IP}:{PasswdTo.salt}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="HourCacheToUser"></param>
        /// <param name="IP"></param>
        public static string GetValidCookie(int HourCacheToUser, string IP)
        {
            // Когда куки станут недействительны
            string expired = DateTime.Now.AddHours(HourCacheToUser).ToBinary().ToString();

            // Ключ  для проверки
            string key = md5.text($"{expired}:{IP}:{PasswdTo.salt}");

            // Результат
            return $"{expired}:{key}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="HttpContext"></param>
        /// <param name="IP"></param>
        /// <returns></returns>
        public static bool IsValidCookie(HttpContext HttpContext, string IP)
        {
            if (HttpContext.Request.Cookies.TryGetValue("isp.ValidCookie", out var cookie))
            {
                // Получаем время и ключ
                var g = new Regex("(-[0-9]+):([a-z0-9]+)").Match(cookie.ToString()).Groups;

                // Проверяем на пустоту
                if (!string.IsNullOrWhiteSpace(g[1].Value) && !string.IsNullOrWhiteSpace(g[2].Value))
                {
                    // Cookie еще активны
                    // Ключ валидный
                    if (ValidCookie(g[1].Value, IP, g[2].Value))
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
        /// <param name="tplToUrl"></param>
        /// <param name="json"></param>
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
        /// 
        /// </summary>
        /// <param name="tplName"></param>
        /// <param name="conf"></param>
        /// <param name="CoreApiUrl"></param>
        /// <param name="IP"></param>
        /// <param name="HostConvert"></param>
        /// <param name="reCAPTCHASitekey"></param>
        static string Html(AntiBotType tplName, AntiBotBase conf, string CoreApiUrl, string IP, string HostConvert, string reCAPTCHASitekey, int CacheAntiBot)
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
                    mass.Add("HashToSignalR", md5.text($"{IP}:{conf.HourCacheToUser}:{PasswdTo.salt}"));
                    tplToUrl = File.Exists($"{Folders.Tpl.AntiBot}/{tplName}.html") ? "/statics/tpl/AntiBot/SignalR.html" : "/statics/tpl/AntiBot/default/SignalR.html";
                    break;
                case AntiBotType.reCAPTCHA:
                    mass.Add("reCAPTCHASitekey", reCAPTCHASitekey);
                    mass.Add("HashToreCAPTCHA", md5.text($"{IP}:{conf.HourCacheToUser}:{PasswdTo.salt}"));
                    tplToUrl = File.Exists($"{Folders.Tpl.AntiBot}/{tplName}.html") ? "/statics/tpl/AntiBot/reCAPTCHA.html" : "/statics/tpl/AntiBot/default/reCAPTCHA.html";
                    break;
                case AntiBotType.CookieAndJS:
                    mass.Add("ValidCookie", GetValidCookie(conf.HourCacheToUser, IP));
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
                memoryCache.Set(KeyToMemoryCache.AntiBotToCache(IP), (tplToUrl, json), TimeSpan.FromMilliseconds(CacheAntiBot));
            }
            #endregion

            // Успех
            return Html(tplToUrl, json);
        }
        #endregion

        #region JsToBase64
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
        public static string JsToRewriteUser(bool RewriteToOriginalDomain, string HostConvert)
        {
            if (!RewriteToOriginalDomain)
            {
                return "if (false) { }";
            }

            return @"if (" + $"!\"{HostConvert}\".match(/.isp$/) && " + " location.hostname.replace(/www./gi,'') != Base64.decode('" + base64.Encode(HostConvert) + @"')) { window.location = Base64.decode('" + base64.Encode("http://" + HostConvert) + @"'); }";
        }
        #endregion
    }
}
