namespace ISPCore.Models.SyncBackup.Operation
{
    public class More : Base.Notification.MoreBase
    {
        public More() { }

        /// <summary>
        /// Дополнительная информация
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="value">Данные</param>
        public More(string name, string value) : base(name, value) { }
    }
}
