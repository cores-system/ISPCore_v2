using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ISPCore.Models.Databases;
using ISPCore.Models.RequestsFilter.Monitoring;
using System.Text;
using ISPCore.Engine.Base.SqlAndCache;

namespace ISPCore.Controllers
{
    public class RequestsFilterToMonitoringController : ControllerToDB
    {
        [HttpGet]
        public IActionResult Index(bool ajax, bool IsJurnal200, bool IsJurnal303, bool IsJurnal403, bool IsJurnal401, bool IsJurnal500, bool IsJurnal2FA, int page = 1, string ShowHost = null)
        {
            // Форматируем домен поиска
            if (string.IsNullOrWhiteSpace(ShowHost))
                ShowHost = null;

            // Статистика запросов за сутки
            ViewBag.StatReguestToHours = StatReguestToHours(ShowHost);

            // Остальные параметры
            ViewData["ajax"] = ajax;
            ViewData["page"] = page;
            ViewBag.ShowHost = ShowHost;
            ViewData["IsJurnal500"] = IsJurnal500;
            ViewData["IsJurnal200"] = IsJurnal200;
            ViewData["IsJurnal303"] = IsJurnal303;
            ViewData["IsJurnal403"] = IsJurnal403;
            ViewData["IsJurnal401"] = IsJurnal401;
            ViewData["IsJurnal2FA"] = IsJurnal2FA;

            // Отдаем контент
            return View("~/Views/RequestsFilter/Monitoring.cshtml", coreDB);
        }

        #region StatReguestToHours
        private string StatReguestToHours(string ShowHost = null)
        {
            // Данные для вывода статистики
            var DtNumberOfRequestDay = new Dictionary<int, NumberOfRequestBase>();

            #region Локальный метод - "AddToNumberOfRequestDay"
            void AddOrUdpateNumberOfRequestDay(string host, NumberOfRequestBase dt)
            {
                // Статистика не для текущего дня
                // Поиск по домену
                if (dt.Time.Day != DateTime.Now.Day || ( ShowHost != null && ShowHost != host))
                    return;

                if (DtNumberOfRequestDay.TryGetValue(dt.Time.Hour, out NumberOfRequestBase item))
                {
                    // Добовляем данные к статистике
                    item.Count200 += dt.Count200;
                    item.Count303 += dt.Count303;
                    item.Count403 += dt.Count403;
                    item.Count401 += dt.Count401;
                    item.Count500 += dt.Count500;
                    item.Count2FA += dt.Count2FA;
                }
                else
                {
                    // Записываем данные
                    DtNumberOfRequestDay.TryAdd(dt.Time.Hour, new NumberOfRequestBase()
                    {
                        Time = dt.Time,
                        Count200 = dt.Count200,
                        Count303 = dt.Count303,
                        Count403 = dt.Count403,
                        Count401 = dt.Count401,
                        Count500 = dt.Count500,
                        Count2FA = dt.Count2FA
                    });
                }
            }
            #endregion

            #region Статистика из базы за сутки
            foreach (var RequestToHour in coreDB.RequestsFilter_NumberOfRequestDay.AsNoTracking())
            {
                AddOrUdpateNumberOfRequestDay(RequestToHour.Host, RequestToHour);
            }
            #endregion

            #region Статистика из кеша за текущий час
            if (memoryCache.TryGetValue(KeyToMemoryCache.IspNumberOfRequestToHour(DateTime.Now), out IDictionary<string, NumberOfRequestHour> DataNumberOfRequestToHour))
            {
                foreach (var item in DataNumberOfRequestToHour)
                {
                    AddOrUdpateNumberOfRequestDay(item.Key, item.Value);
                }
            }
            #endregion

            #region Переменные для статистики
            string tmpBase = "{x:'0:00',y:0},{x:'1:00',y:0},{x:'2:00',y:0},{x:'3:00',y:0},{x:'4:00',y:0},{x:'5:00',y:0},{x:'6:00',y:0},{x:'7:00',y:0},{x:'8:00',y:0},{x:'9:00',y:0},{x:'10:00',y:0},{x:'11:00',y:0},{x:'12:00',y:0},{x:'13:00',y:0},{x:'14:00',y:0},{x:'15:00',y:0},{x:'16:00',y:0},{x:'17:00',y:0},{x:'18:00',y:0},{x:'19:00',y:0},{x:'20:00',y:0},{x:'21:00',y:0},{x:'22:00',y:0},{x:'23:00',y:0}";
            string tmp200 = tmpBase;
            string tmp303 = tmpBase;
            string tmp403 = tmpBase;
            string tmp401 = tmpBase;
            string tmp500 = tmpBase;
            string tmp2FA = tmpBase;
            #endregion

            #region Обновляем переменные
            foreach (var dt in DtNumberOfRequestDay)
            {
                #region Локальный метод - "GeReplacet"
                string GeReplace(string s, long value)
                {
                    return s.Replace("{x:'" + dt.Value.Time.Hour + ":00',y:0}", "{x:'" + dt.Value.Time.Hour + ":00',y:" + value + "}");
                }
                #endregion

                tmp200 = GeReplace(tmp200, dt.Value.Count200);
                tmp303 = GeReplace(tmp303, dt.Value.Count303);
                tmp403 = GeReplace(tmp403, dt.Value.Count403);
                tmp401 = GeReplace(tmp401, dt.Value.Count401);
                tmp500 = GeReplace(tmp500, dt.Value.Count500);
                tmp2FA = GeReplace(tmp2FA, dt.Value.Count2FA);
            }
            #endregion

            #region Собираем json
            StringBuilder json = new StringBuilder();
            json.Append("{key: '200', nonStackable: false, shifting: false, values: [ " + tmp200 + " ] },");
            json.Append("{key: '303', nonStackable: false, shifting: false, values: [ " + tmp303 + " ] },");
            json.Append("{key: '403', nonStackable: false, shifting: false, values: [ " + tmp403 + " ] },");
            json.Append("{key: '401', nonStackable: false, shifting: false, values: [ " + tmp401 + " ] },");
            json.Append("{key: '500', nonStackable: false, shifting: false, values: [ " + tmp500 + " ] },");
            json.Append("{key: '2FA', nonStackable: false, shifting: false, values: [ " + tmp2FA + " ] }");
            #endregion
            
            // Успех
            return json.ToString();
        }
        #endregion
    }
}
