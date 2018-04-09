namespace ISPCore.Models.RequestsFilter.Domains
{
    public enum Protection
    {
        on = 1,    // Включена
        off = 0,   // Выключена
        error = 2  // Включена но есть ошибки в "работе/настройке"
    }
}
