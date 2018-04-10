using System;
using System.Security.Cryptography;
using System.Text;

namespace ISPCore.Engine.Hash
{
    public static class SHA256
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IntText"></param>
        public static string Text(string IntText)
        {
            using (SHA256Managed sha = new SHA256Managed())
            {
                var result = sha.ComputeHash(Encoding.UTF8.GetBytes(IntText));
                return BitConverter.ToString(result).Replace("-", "").ToUpper();
            }
        }
    }
}
