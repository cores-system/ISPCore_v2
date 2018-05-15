namespace ISPCore.Models.Notification
{
    public class More : Base.Notification.MoreBase
    {
        public More() { }

        /// <summary>
        /// Дополнительные данные
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="value">Данные</param>
        public More(string name, string value) : base(name, value) { }
    }
}
