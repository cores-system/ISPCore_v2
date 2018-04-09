namespace ISPCore.Engine.core.Cache.CheckLink
{
    partial class ISPCache
    {
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
            }
            catch { }
        }


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
