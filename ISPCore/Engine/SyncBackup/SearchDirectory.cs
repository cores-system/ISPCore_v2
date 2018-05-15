using System;
using System.Collections.Generic;
using System.IO;

namespace ISPCore.Engine.SyncBackup
{
    public static class SearchDirectory
    {
        /// <summary>
        /// Получить список подпапок
        /// </summary>
        /// <param name="dir">Путь к начальной папке</param>
        public static List<string> Get(string dir)
        {
            List<string> list = new List<string> { };
            list.Add(Tools.ConvertPatchToUnix(dir));
            GetAllDir(dir, ref list);
            return list;
        }


        /// <summary>
        /// Получить список папок
        /// </summary>
        /// <param name="dir">Путь к папке</param>
        /// <param name="list"></param>
        private static void GetAllDir(string dir, ref List<string> list)
        {
            try
            {
                foreach (string intDir in Directory.GetDirectories(dir.Trim(), "*", SearchOption.TopDirectoryOnly))
                {
                    // Правильное имя папки
                    string folder = Tools.ConvertPatchToUnix(intDir);

                    // Идем дальше
                    list.Add(folder);
                    GetAllDir(folder, ref list);
                }
            }
            catch { }
        }
    }
}
