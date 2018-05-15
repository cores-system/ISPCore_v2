using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ISPCore.Engine.Common.Views
{
    public class NavPage<T> : IDisposable
    {
        public IEnumerable<T> GetItems() => mass.Take(pageSize);

        #region NavPage
        List<T> mass;
        string PagePatch, QueryString;
        int pageSize, page;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db">Записи</param>
        /// <param name="context">Используется для построения навигации</param>
        /// <param name="_pageSize">Количество записей на страницу</param>
        /// <param name="_page">Страница</param>
        /// <param name="reverse">Последние записи выводить первыми</param>
        /// <param name="overrideMass">Используется база с готовой навигацией</param>
        public NavPage(IEnumerable<T> db, HttpContext context, int _pageSize, int _page, bool reverse = true, bool overrideMass = false)
        {
            QueryString = Regex.Replace(context.Request.QueryString.Value.Replace("?", "&"), "(&page=[^&]+|&ajax=[^&]+)", "", RegexOptions.IgnoreCase);
            PagePatch = $"{context.Request.Path.Value}?page";
            pageSize = _pageSize;
            page = _page;
            mass = overrideMass ? db.ToList() : (reverse ? db.AsEnumerable().Reverse() : db).Skip((page * pageSize) - pageSize).Take(NavPageSize(page, pageSize)).ToList();

            #region Локальный метод - NavPageSize
            int NavPageSize(int page, int pageSize)
            {
                int x = (page % 5);
                if (x == 0)
                    return pageSize + 1;

                return (pageSize * (5 - x)) + 1;
            }
            #endregion
        }
        #endregion

        #region Nav
        public string Nav(string args = null)
        {
            // Дополнительные аргументы адресной строки
            if (args != null)
                QueryString += args;

            // Если это первая страница
            // Размер страницы больше чем у меня элементов
            if (page == 1 && pageSize >= mass.Count)
                return string.Empty;

            StringBuilder res = new StringBuilder();

            // Если размер страницы больше чем у меня элементов, то это последняя страница
            // Если элементов больше черм размер страницы, то считаем доступно ли еще +5 страниц
            int MaxPage = pageSize >= mass.Count ? page : (page - 1) + (int)Math.Ceiling(((double)mass.Count / pageSize));

            // Начало навигации
            res.Append("<div class='text-center'>");
            res.Append($" <div class='btn-group'><a class='btn btn-default {(page > 1 ? "" : "btn-disabled")}' style='font-size: 15px;' {(page > 1 ? $"href='{PagePatch}={page - 1}{QueryString}' onclick='return loadPage(this)'" : "")}>«</a></div> ");

            #region 5 кнопок
            res.Append("<div class='btn-group'>");
            int StartPage = 1;
            if (page > 5)
            {
                int x = (page % 5);
                StartPage = x == 0 ? page - 4 : (page - x) + 1;
            }

            for (int pg = StartPage; pg < StartPage + 5; pg++)
            {
                if (pg > MaxPage)
                {
                    res.Append($"<a class='btn btn-default btn-disabled'>{pg}</a>");
                    continue;
                }

                res.Append($"<a class='btn btn-default {(pg == page ? "btn-navpage-active" : "")}' href='{PagePatch}={pg}{QueryString}' onclick='return loadPage(this)'>{pg}</a>");
            }
            res.Append("</div>");
            #endregion

            // Завершаем навигацию
            bool IsLastPage = page >= MaxPage;
            res.Append($" <div class='btn-group'><a class='btn btn-default {(IsLastPage ? "btn-disabled" : "")}' style='font-size: 15px;' {(IsLastPage ? "" : $"href='{PagePatch}={page + 1}{QueryString}' onclick='return loadPage(this)'")} >»</a></div> ");
            res.Append("</div>");

            // Отдаем результат
            return res.ToString();
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            mass.Clear();
        }
        #endregion
    }
}
