using ISPCore.Engine.Hash;
using ISPCore.Models.Base.Notification;
using System.Collections.Generic;
using System.Text;

namespace ISPCore.Models.Notification
{
    public class Notation : NotationBase
    {
        #region CreateHashData
        public static string CreateHashData(Notation note)
        {
            StringBuilder mr = new StringBuilder();
            foreach (var item in note.More)
                mr.Append(item.Name + item.Value);

            return md5.text(note.Category + note.Msg + mr.ToString());
        }
        #endregion

        /// <summary>
        /// md5 сообщения
        /// </summary>
        public string HashData { get; set; }

        /// <summary>
        /// Дополнительные значения
        /// </summary>
        public List<More> More { get; set; } = new List<More>();
    }
}
