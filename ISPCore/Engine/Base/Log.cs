using System.IO;

namespace ISPCore.Engine.Base
{
    public static class Log
    {
        public static void Write(string file, string msg)
        {
            try
            {
                File.AppendAllText(file, msg + "\n\n=======================================================================\n\n");
            }
            catch { }
        }
    }
}
