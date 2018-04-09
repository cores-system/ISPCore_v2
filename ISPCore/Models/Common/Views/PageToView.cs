using ISPCore.Engine.Common.Views;
using ISPCore.Models.Databases;
using ISPCore.Models.Databases.json;
using Microsoft.Extensions.Caching.Memory;

namespace ISPCore.Models.Common.Views
{
    public class PageToView<T>
    {
        public PageToView() { }
        public PageToView(NavPage<T> navPage, bool ajax, JsonDB jsonDB, CoreDB coreDB, IMemoryCache memoryCache)
        {
            this.Page = navPage;
            this.ajax = ajax;
            this.jsonDB = jsonDB;
            this.coreDB = coreDB;
            this.memoryCache = memoryCache;
        }

        /// <summary>
        /// 
        /// </summary>
        public NavPage<T> Page { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool ajax { get; }

        public JsonDB jsonDB { get; }
        public CoreDB coreDB { get; }
        public IMemoryCache memoryCache { get; }
    }
}
