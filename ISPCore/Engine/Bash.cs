using System;
using System.Diagnostics;

namespace ISPCore.Engine
{
    public class Bash
    {
        public string Run(string arguments)
        {
            try
            {
                var processInfo = new ProcessStartInfo();
                processInfo.UseShellExecute = false;
                processInfo.RedirectStandardOutput = true;
                processInfo.FileName = "/bin/bash";
                processInfo.Arguments = $" -c \"{arguments}\"";

                var process = Process.Start(processInfo);
                var outPut = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                return outPut;
            }
            catch
            {
                return "";
            }
        }
    }
}
