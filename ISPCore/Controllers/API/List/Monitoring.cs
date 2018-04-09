using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ISPCore.Models.Databases;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using ISPCore.Models.RequestsFilter.Monitoring;
using ISPCore.Engine.Base.SqlAndCache;

namespace ISPCore.Controllers
{
    public class ApiListMonitoring : ControllerToDB
    {
        public JsonResult StatsMonth() => Json(coreDB.RequestsFilter_NumberOfRequestMonth.AsNoTracking().AsEnumerable().Reverse());


        public JsonResult StatsDay(string SearchHost = null)
        {
            // Данные для вывода статистики
            var DtNumberOfRequestDay = new Dictionary<string, List<NumberOfRequestDay>>();

            #region Локальный метод "AddOrUpdateToNumberOfRequestDay"
            void AddOrUpdateToNumberOfRequestDay(string _host, NumberOfRequestBase dt, DateTime time)
            {
                if (SearchHost != null && SearchHost != _host)
                    return;

                var item = new NumberOfRequestDay()
                {
                    Host = _host,
                    Time = time,
                    Count200 = dt.Count200,
                    Count303 = dt.Count303,
                    Count401 = dt.Count401,
                    Count403 = dt.Count403,
                    Count500 = dt.Count500,
                    Count2FA = dt.Count2FA,
                };

                if (DtNumberOfRequestDay.TryGetValue(_host, out List<NumberOfRequestDay> mass))
                {
                    // Добовляем данные
                    mass.Add(item);

                    // Перезаписываем данные
                    DtNumberOfRequestDay[_host] = mass;
                }
                else
                {
                    // Записываем новые данные
                    DtNumberOfRequestDay.Add(_host, new List<NumberOfRequestDay>() { item });
                }
            }
            #endregion

            #region Статистика из базы за сутки
            foreach (var RequestToHour in coreDB.RequestsFilter_NumberOfRequestDay.AsNoTracking())
            {
                AddOrUpdateToNumberOfRequestDay(RequestToHour.Host, RequestToHour, RequestToHour.Time);
            }
            #endregion

            #region Статистика из кеша за текущий час
            if (memoryCache.TryGetValue(KeyToMemoryCache.IspNumberOfRequestToHour(DateTime.Now), out IDictionary<string, NumberOfRequestHour> DataNumberOfRequestToHour))
            {
                foreach (var item in DataNumberOfRequestToHour)
                {
                    AddOrUpdateToNumberOfRequestDay(item.Key, item.Value, DateTime.Now);
                }
            }
            #endregion

            // Ответ
            return Json(DtNumberOfRequestDay);
        }


        public JsonResult Jurnal(string code, int page = 1, int pageSize = 20, string search = null)
        {
            object result = null;
            switch (code.ToLower())
            {
                case "200":
                    result = coreDB.RequestsFilter_Jurnals200.AsNoTracking().AsEnumerable().Where(i => search == null || i.Host == search).Reverse().Skip((page * pageSize) - pageSize).Take(pageSize);
                    break;
                case "303":
                    result = coreDB.RequestsFilter_Jurnals303.AsNoTracking().AsEnumerable().Where(i => search == null || i.Host == search).Reverse().Skip((page * pageSize) - pageSize).Take(pageSize);
                    break;
                case "403":
                    result = coreDB.RequestsFilter_Jurnals403.AsNoTracking().AsEnumerable().Where(i => search == null || i.Host == search).Reverse().Skip((page * pageSize) - pageSize).Take(pageSize);
                    break;
                case "401":
                    result = coreDB.RequestsFilter_Jurnals401.AsNoTracking().AsEnumerable().Where(i => search == null || i.Host == search).Reverse().Skip((page * pageSize) - pageSize).Take(pageSize);
                    break;
                case "500":
                    result = coreDB.RequestsFilter_Jurnals500.AsNoTracking().AsEnumerable().Where(i => search == null || i.Host == search).Reverse().Skip((page * pageSize) - pageSize).Take(pageSize);
                    break;
                case "2FA":
                    result = coreDB.RequestsFilter_Jurnals2FA.AsNoTracking().AsEnumerable().Where(i => search == null || i.Host == search).Reverse().Skip((page * pageSize) - pageSize).Take(pageSize);
                    break;
            }

            return Json(result);
        }
    }
}
