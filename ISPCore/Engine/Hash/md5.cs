using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace ISPCore.Engine.Hash
{
    public class md5
    {
        public static string text(string IntText)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(IntText));
                return BitConverter.ToString(result).Replace("-", "").ToLower();
            }
        }
    }
}
