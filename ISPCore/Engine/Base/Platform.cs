using System;
using System.IO;
using ISPCore.Models.Base;

namespace ISPCore.Engine.Base
{
    public static class Platform
    {
        private static PlatformOS res = PlatformOS.Unknown;
        public static PlatformOS Get
        {
            get
            {
                if (res != PlatformOS.Unknown)
                    return res;

                if (File.Exists($"{Folders.RootPath}/IsDocker"))
                    return (res = PlatformOS.Docker);

                switch (Environment.OSVersion.Platform)
                {
                    case PlatformID.Win32NT:
                        res = PlatformOS.Windows;
                        break;
                    case PlatformID.Unix:
                        res = PlatformOS.Unix;
                        break;
                    case PlatformID.MacOSX:
                        res = PlatformOS.Mac;
                        break;
                    default:
                        res = PlatformOS.Unknown;
                        break;
                }

                return res;
            }
        }


        /// <summary>
        /// Это демо версия ?
        /// https://195.211.154.91:8793/
        /// </summary>
        public static bool IsDemo => File.Exists($"{Folders.RootPath}/IsDemo");
    }
}
