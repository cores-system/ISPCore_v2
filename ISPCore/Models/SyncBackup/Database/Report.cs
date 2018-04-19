using ISPCore.Models.Base.Notification;

namespace ISPCore.Models.SyncBackup.Database
{
    public class Report : NotationBase
    {
        /// <summary>
        /// Id здания, которое добавило заметку
        /// </summary>
        public int TaskId { get; set; }

        /// <summary>
        /// Состояние
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Текст ошибки
        /// </summary>
        public string ErrorMsg { get; set; }
    }
}
