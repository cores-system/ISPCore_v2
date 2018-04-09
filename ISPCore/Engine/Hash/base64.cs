using System;
using System.Text;

namespace ISPCore.Engine.Hash
{
    public class base64
    {
        public static string Encode(string IntText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(IntText);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
