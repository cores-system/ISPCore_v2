using System;

namespace ISPCore.Engine.Triggers
{
    public class Invoke
    {
        /// <summary>
        /// Выполнить Bash команду
        /// </summary>
        /// <param name="s">Команда</param>
        public string Bash(string s)
        {
            return new Bash().Run(s);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="args"></param>
        public string Browser(string method, string url, params string[] args)
        {
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        public bool API(string method, params string[] args)
        {
            return false;
        }
    }
}
