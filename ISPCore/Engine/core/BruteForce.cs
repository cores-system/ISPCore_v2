using ISPCore.Models.RequestsFilter.Base.Enums;
using System.Text.RegularExpressions;

namespace ISPCore.Engine.core
{
    public static class BruteForce
    {
        /// <summary>
        /// Попытка авторизации или авторизация выполнена
        /// </summary>
        /// <param name="bruteForceType">CMS для защиты от Brute Force</param>
        /// <param name="method">Метод запроса "POST/GET"</param>
        /// <param name="uri">url запроса</param>
        /// <param name="FormData">Данные POST запроса</param>
        public static bool IsLogin(BruteForceType bruteForceType, string method, string uri, string FormData)
        {
            RequestMethod Method = method == "POST" ? RequestMethod.POST : RequestMethod.GET;

            // Правила только для POST запросов
            if (Method != RequestMethod.POST)
                return false;

            // Переводим в нижний регистр
            uri = uri.ToLower();

            // Проверяем
            switch (bruteForceType)
            {
                case BruteForceType.DLE:
                    return FormData.Contains("login=submit") && FormData.Contains("login_name=") && FormData.Contains("login_password=");
                case BruteForceType.OpenCart:
                    return Regex.IsMatch(uri, @"^/index.php\?route=(account/login|checkout/login/save)$");
                case BruteForceType.WordPress:
                    {
                        if (uri.Contains("/wp-login.php") || (uri.Contains("/wp-admin/admin-ajax.php") && FormData.Contains("username=") && FormData.Contains("password=")))
                            return true;

                        return false;
                    }
                default:
                    return false;
            }
        }
    }
}
