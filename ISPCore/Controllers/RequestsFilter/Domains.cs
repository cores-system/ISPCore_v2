using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Models.Databases;
using ISPCore.Engine.Common.Views;
using ISPCore.Models.RequestsFilter.Domains;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ISPCore.Engine.Base.SqlAndCache;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using System.Text;
using System.Text.RegularExpressions;
using ISPCore.Models.RequestsFilter.Monitoring;

namespace ISPCore.Controllers
{
    public class RequestsFilterToDomainsController : ControllerToDB
    {
        [HttpGet]
        public IActionResult Index(bool ajax, int page = 1, string search = null, string sort = null)
        {
            int pageSize = 12;

            #region Локальный метод - NavPageSize
            int NavPageSize()
            {
                int x = (page % 5);
                if (x == 0)
                    return pageSize + 1;

                return (pageSize * (5 - x)) + 1;
            }
            #endregion

            #region Статистика запросов за прошлую минуту
            Dictionary<int, ulong> numberOfRequestsPerMinute = new Dictionary<int, ulong>();
            {
                if (memoryCache.TryGetValue(KeyToMemoryCache.IspNumberOfRequestToMinutes(DateTime.Now.AddMinutes(-1)), out IDictionary<string, NumberOfRequestMinute> data))
                {
                    foreach (var dt in data)
                    {
                        if (numberOfRequestsPerMinute.TryGetValue(dt.Value.DomainID, out ulong item))
                        {
                            numberOfRequestsPerMinute[dt.Value.DomainID] = item + dt.Value.NumberOfRequest;
                        }
                        else
                        {
                            numberOfRequestsPerMinute.Add(dt.Value.DomainID, dt.Value.NumberOfRequest);
                        }
                    }

                    #region Сортируем массив
                    if (sort == "req")
                    {
                        numberOfRequestsPerMinute = numberOfRequestsPerMinute.OrderByDescending(i => i.Value).Skip((page * pageSize) - pageSize).Take(NavPageSize()).ToDictionary(i => i.Key, i => i.Value);
                    }
                    else
                    {
                        numberOfRequestsPerMinute = numberOfRequestsPerMinute.OrderByDescending(i => i.Value).ToDictionary(i => i.Key, i => i.Value);
                    }
                    #endregion
                }
            }
            #endregion

            // Поиск / Сортировка
            Func<Domain, bool> predicat = i => search == null || i.host.Contains(search);
            if (sort == "req")
                predicat = i => numberOfRequestsPerMinute.ContainsKey(i.Id);

            // Список доменов
            List<DomainView> domains = new List<DomainView>();

            // Domain To DomainView
            foreach (var domain in coreDB.RequestsFilter_Domains.AsNoTracking().Include(t => t.Templates).AsEnumerable().Where(predicat).Reverse().Skip((page * pageSize) - pageSize).Take(NavPageSize()))
            {
                var model = new DomainView()
                {
                    Id = domain.Id,
                    host = domain.host,
                    Protect = domain.Protect
                };

                /// Количество запросов за прошлую минуту
                if (numberOfRequestsPerMinute.TryGetValue(domain.Id, out ulong ReqToMinute))
                    model.ReqToMinute = ReqToMinute;

                #region Имена шаблонов
                StringBuilder TemplateName = new StringBuilder();
                foreach (var tpl in domain.Templates)
                {
                    TemplateName.Append(coreDB.RequestsFilter_Templates.GetTemplateName(tpl.Template, "") + ", ");
                }
                model.Templates = Regex.Replace(TemplateName.ToString(), ",([ ]+)?$", "");
                #endregion

                // Модель
                domains.Add(model);
            }

            // Дополнительная информация
            ViewBag.Info = page == 1 && search == null && sort == null ? "У вас еще нет добавленных доменов" : "Нет данных для вывода";

            // Сортировка по "req/s"
            if (sort == "req")
                return View("~/Views/RequestsFilter/Domains.cshtml", new NavPage<DomainView>(domains.OrderByDescending(i => i.ReqToMinute).ToList(), HttpContext, pageSize, page, overrideMass: true), ajax);

            // Базовая сортировка
            return View("~/Views/RequestsFilter/Domains.cshtml", new NavPage<DomainView>(domains, HttpContext, pageSize, page, overrideMass: true), ajax);
        }
    }
}
