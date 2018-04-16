using System;

namespace ISPCore.Models.Auth
{
    public class AuthSession
    {
        public int Id { get; set; }

        /// <summary>
        /// Сессия
        /// </summary>
        public string Session { get; set; }

        /// <summary>
        /// IP-адрес
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// Авторизация 2FA
        /// </summary>
        public bool Confirm2FA { get; set; }

        /// <summary>
        /// Хеш пароля в SHA256
        /// </summary>
        public string HashPasswdToRoot { get; set; }

        /// <summary>
        /// До какого числа сессия считается активной 
        /// </summary>
        public DateTime Expires { get; set; }

        /// <summary>
        /// Время создания сессии
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
