namespace ISPCore.Models.Databases.json
{
    public class Security
    {
        private int _CountAccess, _BlockingTime;

        /// <summary>
        /// Количество попыток авторизации за 10 минут
        /// </summary>
        public int CountAccess
        {
            get { return _CountAccess == 0 ? 5 : _CountAccess; }
            set { _CountAccess = value; }
        }

        /// <summary>
        /// Время блокировки в минутах
        /// </summary>
        public int BlockingTime
        {
            get { return _BlockingTime == 0 ? 40 : _BlockingTime; }
            set { _BlockingTime = value; }
        }

        /// <summary>
        /// https://www.google.com/recaptcha/admin
        /// </summary>
        public string reCAPTCHASecret { get; set; }

        /// <summary>
        /// https://www.google.com/recaptcha/admin
        /// </summary>
        public string reCAPTCHASitekey { get; set; }
    }
}
