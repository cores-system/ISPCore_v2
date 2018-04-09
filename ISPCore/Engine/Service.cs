using ISPCore.Engine.Base;
using ISPCore.Models.Databases;
using ISPCore.Models.Databases.json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ISPCore.Engine
{
    public class Service
    {
        static dynamic jsonDB;
        static dynamic memoryCache;


        public static void Create(IMemoryCache _memoryCache)
        {
            jsonDB = new JsonDB();
            memoryCache = _memoryCache;
        }


        public static T Get<T>()
        {
            var typeT = typeof(T);

            if (typeT == typeof(IMemoryCache))
                return memoryCache;

            if (typeT == typeof(JsonDB))
                return jsonDB;

            if (typeT == typeof(CoreDB))
            {
                var optionsCoreDB = new DbContextOptionsBuilder<CoreDB>();
                optionsCoreDB.UseSqlite($"DataSource={Folders.File.ISPCoreDB}");
                return (dynamic)new CoreDB(optionsCoreDB.Options);
            }

            return default(T);
        }
    }
}
