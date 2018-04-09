using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using ISPCore.Models.SyncBackup;

namespace ISPCore.Engine.SyncBackup
{
    public class CryptoBox
    {
        string PasswdAES;
        public CryptoBox(string _PasswdAES) {
            PasswdAES = _PasswdAES;
        }

        #region GetAES
        Aes GetAES()
        {
            // Данные для ключа
            byte[] IVa = new byte[] { 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x11, 0x11, 0x12, 0x13, 0x14, 0x0e, 0x16, 0x17 };
            byte[] key = Encoding.UTF8.GetBytes(PasswdAES);

            // Создаем ключ
            Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(Encoding.UTF8.GetString(IVa, 0, IVa.Length), key);
            Aes _aes = Aes.Create();
            _aes.KeySize = 256;
            _aes.BlockSize = 128;
            _aes.Key = deriveBytes.GetBytes(16); //128bits
            _aes.IV = _aes.Key;
            return _aes;
        }
        #endregion

        #region EncryptSize
        /// <summary>
        /// Получает размер файла
        /// </summary>
        /// <param name="LocalFile">Путь к локальному файлу</param>
        private long EncryptSize(string LocalFile)
        {
            try
            {
                // Получаем поток для шифрования
                ICryptoTransform EncryptoTransform = GetAES().CreateEncryptor();

                // Локальный файл
                var IntStream = File.OpenRead(LocalFile);

                // Подключаем поток для расшифровки данных
                var cry = new CryptoStream(IntStream, EncryptoTransform, CryptoStreamMode.Read);

                long size = 0;
                int Coutread = 0;
                byte[] buffer = new byte[81920];
                while((Coutread = cry.Read(buffer, 0 , buffer.Length)) != 0)
                {
                    size += Coutread;
                }

                cry.Dispose();
                IntStream.Dispose();
                EncryptoTransform.Dispose();

                return size;
            }
            catch { return 0; }
        }
        #endregion

        #region Encrypt
        /// <summary>
        /// Шифрует файл
        /// </summary>
        /// <param name="IntStream">Исходный поток</param>
        /// <param name="size">Размер файла после шифрования</param>
        /// <param name="error">Данные ошибки</param>
        /// <returns>Зашифрованный поток</returns>
        public EncryptStream Encrypt(Stream IntStream, string LocalFile, ref long size, out string error)
        {
            error = null;
            try
            {
                // Узнаем размер файла в зашифрованном виде
                size = EncryptSize(LocalFile);

                // Получаем поток для шифрования
                ICryptoTransform EncryptoTransform = GetAES().CreateEncryptor();

                //Подключаем поток для расшифровки данных
                var stream = new EncryptStream(IntStream, EncryptoTransform, CryptoStreamMode.Read);
                stream.SetLength(size);
                return stream;
            }
            catch (Exception ex)
            {
                error = ex.ToString();
                return null;
            }
        }
        #endregion

        #region Decrypt
        /// <summary>
        /// Расшировка файла
        /// </summary>
        /// <param name="IntStream">Зашифрованный поток</param>
        /// <param name="OutStream">Исходный поток</param>
        /// <param name="FileSize">Размер файла</param>
        /// <param name="error">Данные ошибки</param>
        public bool Decrypt(Stream IntStream, Stream OutStream, long FileSize, out string error)
        {
            try
            {
                // Получаем поток для дешифровки
                ICryptoTransform DecryptoTransform = GetAES().CreateDecryptor();

                //Подключаем поток для расшифровки данных
                CryptoStream crypt = new CryptoStream(IntStream, DecryptoTransform, CryptoStreamMode.Read);

                int read = 0;
                do
                {
                    // Считываем поток
                    byte[] buffer = new byte[FileSize > 81920 ? 81920 : FileSize];
                    read = crypt.Read(buffer, 0, buffer.Length);
                    OutStream.Write(buffer, 0, read);
                    FileSize -= read;
                }
                while (read != 0 && FileSize > 0);

                // Закрываем потоки
                crypt.Close(); crypt.Dispose();
                DecryptoTransform.Dispose();
                error = null;
                return true;
            }
            catch (Exception ex)
            {
                error = ex.ToString();
                return false;
            }
        }
        #endregion
    }
}
