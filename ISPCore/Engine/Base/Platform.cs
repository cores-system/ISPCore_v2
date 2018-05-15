using System;
using ISPCore.Models.Base;

namespace ISPCore.Engine.Base
{
    public static class Platform
    {
        private static PlatformOS res = PlatformOS.Unknown;
        private static bool _IsDemo, _IsDebug;


        /// <summary>
        /// Указать платформу
        /// </summary>
        /// <param name="IsDocker">Docker</param>
        /// <param name="IsDemo">Demo</param>
        /// <param name="IsDebug">Dev</param>
        public static void Set(bool IsDocker = false, bool IsDemo = false, bool IsDebug = false)
        {
            if (IsDocker)
                res = PlatformOS.Docker;

            _IsDemo = IsDemo;
            _IsDebug = IsDebug;
        }

        /// <summary>
        /// Платформа
        /// </summary>
        public static PlatformOS Get
        {
            get
            {
                if (res != PlatformOS.Unknown)
                    return res;

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
        /// https://isp.demo.core-system.org/
        /// </summary>
        public static bool IsDemo => _IsDemo;

        /// <summary>
        /// Для разработки
        /// </summary>
        public static bool IsDebug => _IsDebug;
    }
}
