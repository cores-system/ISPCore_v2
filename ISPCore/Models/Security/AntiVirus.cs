using ISPCore.Engine;
using ISPCore.Engine.Base;
using System.IO;
using System.Text;

namespace ISPCore.Models.Security
{
    public class AntiVirus
    {
        #region Статичиские переменные
        public static bool IsRun(int Id) { return IsRun(Id.ToString()); }
        public static bool IsRun(string Id)
        {
            // Проверяем файл
            if (File.Exists($"{Folders.AV}/progress_id-{Id}.json"))
                return true;

            // Проверяем shell
            Bash bash = new Bash();
            string res = bash.Run($"ps ux | grep \"/av/ai-bolit.php\" | grep \"/progress_id-{Id}.json\" | grep -v \"grep\"");
            return res.Contains("/ai-bolit.php");
        }

        public static string name => "AI-Bolit";
        private static string _versToAV;
        public static string vers
        {
            get
            {
                if (_versToAV == null)
                    _versToAV = File.ReadAllText($"{Folders.AV}/vers.txt", Encoding.UTF8);

                return _versToAV;
            }
        }
        #endregion
       
        private string _php;
        private int _mode, _memory, _size;

        /// <summary>
        /// Путь к PHP
        /// </summary>
        public string php
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_php))
                    return "/usr/bin/php";

                return _php?.Trim();
            }
            set { _php = value; }
        }

        /// <summary>
        /// Просканировать директорию
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// Исключить из сканирования
        /// </summary>
        public string skip { get; set; }

        /// <summary>
        /// Просканировать только определенные расширения
        /// </summary>
        public string scan { get; set; }

        /// <summary>
        /// Режим проверки (обычный = 1 / параноидальный = 2)
        /// </summary>
        public int mode
        {
            get { return _mode <= 1 ? 1 : 2; }
            set { _mode = value; }
        }

        /// <summary>
        /// Размер памяти в MB
        /// </summary>
        public int memory
        {
            get { return _memory == 0 ? 512 : _memory; }
            set { _memory = value; }
        }

        /// <summary>
        /// Максимальный размер проверяемого файла в Kb
        /// </summary>
        public int size
        {
            get { return _size == 0 ? 3000 : _size; }
            set { _size = value; }
        }

        /// <summary>
        /// Делать паузу между файлами при сканировании
        /// </summary>
        public int delay { get; set; } = 0;
    }
}
