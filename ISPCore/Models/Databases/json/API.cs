namespace ISPCore.Models.Databases.json
{
    public class API
    {
        /// <summary>
        /// Доступ к API открыт
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Логин 
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// IP для которого не нужна авторизация 
        /// </summary>
        public string WhiteIP { get; set; }
    }
}
