using ISPCore.Models.Databases;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using ISPCore.Models.RequestsFilter.Domains;
using ISPCore.Models.Base;
using ISPCore.Engine.Base.SqlAndCache;

namespace ISPCore.Engine.core.Cache.CheckLink
{
    partial class ISPCache
    {
        /// <summary>
        /// Список доменов
        /// </summary>
        private static ConcurrentDictionary<string, int> MassDomainToID = null;
        

        /// <summary>
        /// Обновляет список доменов в кеше
        /// </summary>
        private static void ReloadDomainToID()
        {
            SqlToMode.SetMode(SqlMode.Read);
            using (var coreDB = Service.Get<CoreDB>())
            {
                ConcurrentDictionary<string, int> tmp = new ConcurrentDictionary<string, int>();
                foreach (var domain in coreDB.RequestsFilter_Domains.AsNoTracking().Include(a => a.Aliases))
                {
                    // Пропускаем сайты которые отключены
                    if (domain.Protect == Protection.off || domain.Id == 0)
                        continue;

                    foreach (var alias in domain.Aliases)
                    {
                        // Алиасы
                        tmp.AddOrUpdate(alias.host, domain.Id, (s, i) => domain.Id);
                    }

                    // Основной домен
                    tmp.AddOrUpdate(domain.host, domain.Id, (s, i) => domain.Id);
                }

                // Очищаем текущую базу
                if (MassDomainToID != null)
                    MassDomainToID.Clear();

                MassDomainToID = tmp;
            }
            SqlToMode.SetMode(SqlMode.ReadOrWrite);
        }


        /// <summary>
        /// Получить Id домена в кеше по имени домена
        /// </summary>
        /// <param name="host">Имя домена</param>
        /// <returns>Id</returns>
        public static int DomainToID(string host)
        {
            if (host == string.Empty)
                return 0;

            if (MassDomainToID == null)
                ReloadDomainToID();

            MassDomainToID.TryGetValue(host, out int Id);
            return Id;
        }
    }
}
