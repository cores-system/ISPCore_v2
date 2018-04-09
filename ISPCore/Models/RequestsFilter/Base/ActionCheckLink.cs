namespace ISPCore.Models.RequestsFilter.Base
{
    public enum ActionCheckLink
    {
        allow = 0, // Разрешить все запросы 
        deny = 1,  // Запретить все запросы
        Is2FA = 2  // Разрешить все запросы после прохождения авторизации 2FA
    }
}
