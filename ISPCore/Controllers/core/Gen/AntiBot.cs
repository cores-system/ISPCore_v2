using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Engine;
using ISPCore.Models.Databases.json;
using System.Text.RegularExpressions;
using ISPCore.Engine.core.Cache.CheckLink;
using ISPCore.Models.RequestsFilter.Base.Enums;
using ISPCore.Models.Base;
using ISPCore.Engine.Auth;
using ISPCore.Engine.Hash;
using ISPCore.Engine.Base.SqlAndCache;

namespace ISPCore.Controllers.core.Gen
{
    public class CoreGenToAntiBotController : Controller
    {
        public ActionResult Index()
        {
            #region Проверки
            // Спецальный host
            string HostConvert = Regex.Replace(HttpContext.Request.Host.Host.ToLower().Trim(), "^www\\.", "");

            // Настройки домена
            int DomainID = ISPCache.DomainToID(HostConvert);
            if (DomainID == 0)
                return Content(string.Empty, "application/javascript");

            // IP адрес пользователя
            string IP = HttpContext.Connection.RemoteIpAddress.ToString();

            // Если у пользователя валидные Cookie
            if (Engine.core.AntiBot.IsValidCookie(HttpContext, IP))
                return Content(string.Empty, "application/javascript");

            // Настройки JsonDB
            var jsonDB = Service.Get<JsonDB>();

            // Достаем настройки WhiteList из кеша
            var whiteList = Engine.Base.SqlAndCache.WhiteList.GetCache(jsonDB.WhiteList);

            // IP находится в белом списке
            if (Regex.IsMatch(IP, whiteList.IpRegex) || WhitePtr.IsWhiteIP(IP))
                return Content(string.Empty, "application/javascript");
            #endregion

            // Достаем данные для домена из кеша
            var Domain = ISPCache.GetDomain(DomainID);

            // Достаем настройки AntiBot из кеша
            var antiBotToGlobalConf = Engine.core.AntiBot.GlobalConf(jsonDB.AntiBot);

            // Настройки AntiBot
            var antiBotType = (antiBotToGlobalConf.conf.Enabled || Domain.AntiBot.UseGlobalConf) ? antiBotToGlobalConf.conf.type : Domain.AntiBot.type;

            // Не выбран способ проверки
            if (antiBotType == AntiBotType.Off)
                return Content(string.Empty, "application/javascript");

            // Выбираем настройки какого конфига использовать
            AntiBotBase antiBotConf = (antiBotToGlobalConf.conf.Enabled || Domain.AntiBot.UseGlobalConf) ? (AntiBotBase)antiBotToGlobalConf.conf : (AntiBotBase)Domain.AntiBot;

            // Генерируем код SignalR
            return Content(JsToSignalR(antiBotConf, IP, jsonDB.Base.CoreAPI, HostConvert), "application/javascript");
        }


        #region JsToSignalR
        static string JsToSignalR(AntiBotBase conf, string IP, string CoreApiUrl, string HostConvert)
        {
            return Engine.core.AntiBot.JsToBase64(conf.RewriteToOriginalDomain) + Engine.core.AntiBot.JsToRewriteUser(conf.RewriteToOriginalDomain, HostConvert) + @"
{
    setTimeout(function()
    {
	    HubConnection = new signalR.HttpConnection('" + CoreApiUrl + @"/AntiBotHub');
	    Hub = new signalR.HubConnection(HubConnection);

	    Hub.on('OnCookie', function(cookie, HourToCookie) 
	    {
		    var date = new Date(new Date().getTime() + (60 * 1000) * 60 * HourToCookie);
		    document.cookie = 'isp.ValidCookie='+cookie+'; path=/; expires=' + date.toUTCString();
	    })

	    Hub.start().then(function () {
		    Hub.invoke('GetValidCookie', '" + IP + "', '" + conf.HourCacheToUser + "', '" + md5.text($"{IP}:{conf.HourCacheToUser}:{PasswdTo.salt}") + @"');
	    })

    }," + conf.WaitUser + @");
}
";
        }
        #endregion
    }
}
