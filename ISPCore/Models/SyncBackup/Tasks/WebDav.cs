using ISPCore.Engine.Databases;
using ISPCore.Models.Databases.Interface;

namespace ISPCore.Models.SyncBackup.Tasks
{
    public class WebDav : IUpdate
    {
        public int Id { get; set; }
        public int TaskId { get; set; }

        /// <summary>
        /// Ссылка на WebDav - "https://webdav.yandex.ua/"
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Passwd { get; set; }

        /// <summary>
        /// Обновить поля
        /// </summary>
        /// <param name="item">Новые данные</param>
        void IUpdate.Update(dynamic item) => CommonModels.Update(this, item);
    }
}
