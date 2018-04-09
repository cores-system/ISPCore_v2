using ISPCore.Engine.Databases;
using ISPCore.Models.Databases.Interface;

namespace ISPCore.Models.SyncBackup.Tasks
{
    public class OneDrive : IUpdate
    {
        public int Id { get; set; }
        public int TaskId { get; set; }

        /// <summary>
        /// ID приложения
        /// </summary>
        public string ApplicationId { get; set; } = "2c2720ef-7b64-4938-9b68-2a88bdd2a456";

        /// <summary>
        /// Токен для авторизации и работы с OneDrive
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Обновить поля
        /// </summary>
        /// <param name="item">Новые данные</param>
        void IUpdate.Update(dynamic item) => CommonModels.Update(this, item);
    }
}
