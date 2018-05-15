using System;
using ISPCore.Models.Base;

namespace ISPCore.Engine.Base.SqlAndCache
{
    public class SqlToMode
    {
        private static DateTime LastModifiedMode;
        private static SqlMode _mode = SqlMode.ReadOrWrite;

        public static SqlMode Mode
        {
            get
            {
                // Если прошло больше 120 секунд с переключения в режим Read
                if (DateTime.Now.AddSeconds(-120) > LastModifiedMode)
                    return SqlMode.ReadOrWrite;

                return _mode;
            }
        }


        /// <summary>
        /// Изменить режим доступа к SQL
        /// </summary>
        /// <param name="mode">Режим доступа к SQL</param>
        public static void SetMode(SqlMode mode)
        {
            switch (mode)
            {
                case SqlMode.Read:
                    _mode = mode;
                    LastModifiedMode = DateTime.Now;
                    break;
                case SqlMode.ReadOrWrite:
                    _mode = mode;
                    break;
            }

            //Debug.WriteLine("Debug.WriteLine: " + _mode);
        }
    }
}
