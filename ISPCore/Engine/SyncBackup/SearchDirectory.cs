using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ISPCore.Engine.SyncBackup
{
    public static class SearchDirectory
    {
        public static List<string> Get(string dir)
        {
            List<string> list = new List<string> { };
            list.Add(Tools.ConvertPatchToUnix(dir));
            GetAllDir(dir, ref list);
            return list;
        }


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
