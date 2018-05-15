using ISPCore.Engine.Common.Views;
using ISPCore.Models.Databases;
using ISPCore.Models.Databases.json;
using Microsoft.Extensions.Caching.Memory;

namespace ISPCore.Models.Common.Views
{
    public class PageToView<T>
    {
        public PageToView() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="navPage">Навигация</param>
        /// <param name="ajax">ajax запрос</param>
        /// <param name="jsonDB">База Json</param>
        /// <param name="coreDB">База SQL</param>
        /// <param name="memoryCache">Кеш</param>
        public PageToView(NavPage<T> navPage, bool ajax, JsonDB jsonDB, CoreDB coreDB, IMemoryCache memoryCache)
        {
            this.Page = navPage;
            this.ajax = ajax;
            this.jsonDB = jsonDB;
            this.coreDB = coreDB;
            this.memoryCache = memoryCache;
        }

        /// <summary>
        /// Навигация
        /// </summary>
        public NavPage<T> Page { get; }

        /// <summary>
        /// ajax запрос
        /// </summary>
        public bool ajax { get; }

        /// <summary>
        /// База Json
        /// </summary>
        public JsonDB jsonDB { get; }

        /// <summary>
        /// База SQL
        /// </summary>
        public CoreDB coreDB { get; }

        /// <summary>
        /// Кеш
        /// </summary>
        public IMemoryCache memoryCache { get; }
    }
}
