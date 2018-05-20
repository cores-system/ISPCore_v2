using ISPCore.Engine.Hash;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ISPCore.Engine.Base
{
    public class Generate
    {
        #region Passwd
        static Random random = new Random();
        static string ArrayList => "qwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNM1234567890";
        static string ArrayListToNumber => "1234567890";

        public static string Passwd(int size = 8, bool IsNumberCode = false)
        {
            StringBuilder array = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                array.Append(IsNumberCode ? ArrayListToNumber[random.Next(0, 9)] : ArrayList[random.Next(0, 61)]);
            }

            return array.ToString();
        }
        #endregion

        #region Style
        public static string Style(ActionStyle action)
        {
            switch (action)
            {
                case ActionStyle.css:
                    {
                        var res = GetHashAndFileName($"{Folders.Style}/old/css");
                        string FileName = $"{res.hash}.css";
                        string FilePath = $"{Folders.Style}/generate/{FileName}";
                        if (!File.Exists(FilePath))
                            GenerateToFile(FilePath, res.files, $"*.css");

                        return FileName;
                    }
                case ActionStyle.js:
                    {
                        var res = GetHashAndFileName($"{Folders.Style}/old/js");
                        string FileName = $"{res.hash}.other.js";
                        string FilePath = $"{Folders.Style}/generate/{FileName}";
                        if (!File.Exists(FilePath))
                            GenerateToFile(FilePath, res.files, $"*.other.js");

                        return FileName;
                    }
                case ActionStyle.jsLib:
                    {
                        var res = GetHashAndFileName($"{Folders.Style}/old/jsLib");
                        string FileName = $"{res.hash}.lib.js";
                        string FilePath = $"{Folders.Style}/generate/{FileName}";
                        if (!File.Exists(FilePath))
                            GenerateToFile(FilePath, res.files, $"*.lib.js");

                        return FileName;
                    }
                case ActionStyle.blueprint:
                    {
                        var res = GetHashAndFileName($"{Folders.Style}/blueprint/js");
                        string FileName = $"{res.hash}.blueprint.js";
                        string FilePath = $"{Folders.Style}/generate/{FileName}";
                        if (!File.Exists(FilePath)) {
                            GenerateToFile(FilePath, res.files, $"*.blueprint.js");
                            File.WriteAllText($"{Folders.Style}/generate/blueprint.js", File.ReadAllText(FilePath));
                        }

                        return FileName;
                    }
            }

            return "";


            #region GetHashAndFileName
            (string hash, List<string> files) GetHashAndFileName(string path)
            {
                StringBuilder tmpHash = new StringBuilder();
                List<string> tmp_Files = new List<string>() { Capacity = 60 };

                foreach (var IntFile in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories))
                {
                    tmp_Files.Add(IntFile);
                    tmpHash.Append(IntFile + File.GetLastWriteTime(IntFile).ToBinary().ToString());
                }
                
                return (md5.text(tmpHash.ToString()), tmp_Files.OrderBy(i => i).ToList());
            }
            #endregion

            #region GenerateToFile
            void GenerateToFile(string FilePath, List<string> files, string RemovePath)
            {
                #region Создаем новый файл
                StringBuilder data = new StringBuilder();
                foreach (var IntFile in files) {
                    data.Append(File.ReadAllText(IntFile) + "\n");
                }

                File.WriteAllText(FilePath, data.ToString());
                #endregion

                #region Удаляем старый файл
                foreach (var IntFile in Directory.GetFiles($"{Folders.Style}/generate/", RemovePath, SearchOption.TopDirectoryOnly))
                {
                    if (IntFile != FilePath)
                        File.Delete(IntFile);
                }
                #endregion  
            }
            #endregion
        }
        #endregion
    }


    public enum ActionStyle
    {
        css,
        js,
        jsLib,
        blueprint
    }
}
