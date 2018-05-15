using Trigger = ISPCore.Models.Triggers.Events.core.CheckRequest;

namespace ISPCore.Engine.core.Cache.CheckLink
{
    partial class ISPCache
    {
        /// <summary>
        /// Удалить кеш домена
        /// </summary>
        /// <param name="Id">Id домена</param>
        public static void RemoveDomain(int Id)
        {
            if (Id == 0)
                return;

            try
            {
                // Пересобираем список доменов
                ReloadDomainToID();

                // Удаляем кеш домена
                MassGetDomain.TryRemove(Id, out var value);
                Trigger.OnDomainCache((Id, IsCreate: false, IsRemove: true));
            }
            catch { }
        }

        /// <summary>
        /// Удалить кеш шаблона
        /// </summary>
        /// <param name="Id">Id шаблона</param>
        public static void RemoveTemplate(int Id)
        {
            if (Id == 0)
                return;

            try
            {
                // Удаляем кеш доменов
                foreach (var item in MassGetDomain)
                {
                    if (item.Value.TemplateIds.Contains(Id))
                    {
                        RemoveDomain(item.Key);
                    }
                }
            }
            catch { }
        }
    }
}
