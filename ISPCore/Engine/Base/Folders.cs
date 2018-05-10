using System.IO;

namespace ISPCore.Engine.Base
{
    public static class Folders
    {
        public static void CreateDirectory()
        {
            Directory.CreateDirectory(Tmp);
            Directory.CreateDirectory(Temp.AutoUpdate);
            Directory.CreateDirectory(Temp.SyncBackup);
            Directory.CreateDirectory(Passwd);
            Directory.CreateDirectory(Triggers);
            Directory.CreateDirectory(LogRequests);
            Directory.CreateDirectory(ReportsAV);
            Directory.CreateDirectory(ReportSync);
            Directory.CreateDirectory(Sync);
        }

        public static string RootPath { get; private set; }
        public static void SetRootPath(string _RootPath)
        {
            RootPath = _RootPath;
        }

        public static string Tmp => "core/tmp";
        public static string Passwd => "core/passwd";
        public static string Databases => "core/Databases";
        public static string Triggers => "core/Databases/Triggers";
        public static string Log => RootPath + "/core/Log";
        public static string LogRequests => RootPath + "/core/Log/Requests";
        public static string ReportsAV => RootPath + "/wwwroot/reports/av";
        public static string ReportSync => RootPath + "/wwwroot/reports/sync";
        public static string AV => RootPath + "/core/av";
        public static string Style => RootPath + "/wwwroot/style";
        public static string Sync => RootPath + "/core/sync";
       

        public static class File
        {
            public static string SystemErrorLog => $"{Log}/system.error.log";
            public static string TriggerErrorLog => $"{Log}/trigger.error.log";
            public static string ISPCoreDB => $"{RootPath}/core/Databases/ISPCore.db";
        }


        public static class Tpl
        {
            public static string AntiBot => $"{RootPath}/wwwroot/statics/tpl/AntiBot";
            public static string LimitRequest => $"{RootPath}/wwwroot/statics/tpl/LimitRequest";
        }

        public static class Temp
        {
            public static string AutoUpdate => "core/tmp/auto-update";
            public static string SyncBackup => "core/tmp/SyncBackup";
        }
    }
}
