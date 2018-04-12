using ISPCore.Engine.Base;
using ISPCore.Engine.Hash;
using System;
using System.IO;
using System.Text;

namespace ISPCore.Engine.Auth
{
    public static class PasswdTo
    {
        private static string PasswdRoot, Passwd2FA, _salt;
        private static DateTime LastWriteRoot, LastWrite2FA;

        private static string ReadPasswd(ref string passwd, ref DateTime LastWrite, string path)
        {
            // Кэшированный пароль
            if (passwd != null && File.GetLastWriteTime(path) == LastWrite)
                return passwd;

            // Создаем кеш
            LastWrite = File.GetLastWriteTime(path);
            passwd = File.ReadAllText(path, Encoding.UTF8).Trim(); // В файле пароль под SHA256

            // Отдаем хеш пароля
            return passwd;
        }


        public static string salt
        {
            get
            {
                if (_salt != null)
                    return _salt;

                if (File.Exists($"{Folders.Passwd}/salt")) {
                    _salt = File.ReadAllText($"{Folders.Passwd}/salt");
                }
                else
                {
                    _salt = Generate.Passwd(30);
                    File.WriteAllText($"{Folders.Passwd}/salt", _salt);
                }

                return _salt;
            }
            set
            {
                _salt = value;
                File.WriteAllText($"{Folders.Passwd}/salt", _salt);
            }
        }


        public static string Root => ReadPasswd(ref PasswdRoot, ref LastWriteRoot, $"{Folders.Passwd}/root");
        public static string FA => ReadPasswd(ref Passwd2FA, ref LastWrite2FA, $"{Folders.Passwd}/2fa");
    }
}
