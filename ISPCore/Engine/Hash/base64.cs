using System;
using System.Text;

namespace ISPCore.Engine.Hash
{
    public class base64
    {
        /// <summary>
        /// Хеш Base64
        /// </summary>
        /// <param name="IntText">Исходный текст</param>
        public static string Encode(string IntText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(IntText);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
