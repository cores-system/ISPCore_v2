using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using ISPCore.Engine.Base;
using ISPCore.Engine.Hash;
using ISPCore.Models.SyncBackup;

namespace ISPCore.Engine.SyncBackup
{
    public class CryptoBox : IDisposable
    {
        string PasswdAES;
        string LocalFile, tmpFile;
        public CryptoBox(string _PasswdAES, string _localFile, string _tmpFile = null) {
            PasswdAES = _PasswdAES;
            LocalFile = _localFile;
            tmpFile = _tmpFile;
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

        #region OpenRead
        /// <summary>
        /// Отдает файл зашифрованный в AES256 и сжатый в GZip
        /// </summary>
        /// <param name="error">Данные ошибки</param>
        /// <returns>Временный файл</returns>
        public FileStream OpenRead(out string error)
        {
            error = null;
            try
            {
                // Временный файл
                tmpFile = $"{Folders.Temp.SyncBackup}/{md5.text(LocalFile)}";
                
                using (ICryptoTransform EncryptoTransform = GetAES().CreateEncryptor())
                {
                    // Исходный файл
                    using (FileStream sourceStream = File.OpenRead(LocalFile))
                    {
                        // Временный файл
                        using (FileStream targetStream = new FileStream(tmpFile, FileMode.Create, FileAccess.Write))
                        {
                            // Поток архивации и шифрования
                            using (GZipStream compressionStream = new GZipStream(new CryptoStream(targetStream, EncryptoTransform, CryptoStreamMode.Write), CompressionMode.Compress))
                            {
                                sourceStream.CopyTo(compressionStream);
                            }
                        }
                    }
                }

                // Отдаем временный файл
                return File.OpenRead(tmpFile);
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
        /// 
        /// </summary>
        /// <param name="error">Данные ошибки</param>
        public bool Decrypt(out string error)
        {
            try
            {
                using (ICryptoTransform DecryptoTransform = GetAES().CreateDecryptor())
                {
                    // Поток для расшифровки файла 
                    using (CryptoStream crypt = new CryptoStream(File.OpenRead(tmpFile), DecryptoTransform, CryptoStreamMode.Read))
                    {
                        // Поток для распаковки файла 
                        using (GZipStream decompresionStream = new GZipStream(crypt, CompressionMode.Decompress))
                        {
                            using (FileStream targetStream = File.OpenWrite(LocalFile))
                            {
                                // Распаковка и расшифровка файла
                                decompresionStream.CopyTo(targetStream);
                            }

                            // Успех
                            error = null;
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.ToString();
                return false;
            }
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            try
            {
                // Удаляем временный файл
                if (tmpFile != null)
                    File.Delete(tmpFile);
            }
            catch { }
        }
        #endregion
    }
}
