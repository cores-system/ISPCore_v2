using ISPCore.Models.Base.Notification;
using System;
using System.Collections.Generic;

namespace ISPCore.Models.SyncBackup.Operation
{
    public class Notation : NotationBase
    {
        /// <summary>
        /// Id здания, которое добавило заметку
        /// </summary>
        public int TaskId { get; set; }

        /// <summary>
        /// Дополнительные значения
        /// </summary>
        public List<More> More { get; set; } = new List<More>();
    }
}
