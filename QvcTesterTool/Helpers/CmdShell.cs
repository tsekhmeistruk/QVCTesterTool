﻿using System.Diagnostics;

namespace QvcTesterTool.Helpers
{
    public class CmdShell
    {
        public static string Execute(string args)
        {
            using (var pProcess = new Process())
            {
                const string strCommand = "cmd.exe";
                string arguments = "/c " + args;

                pProcess.StartInfo.FileName = strCommand;
                pProcess.StartInfo.Arguments = arguments;
                pProcess.StartInfo.UseShellExecute = false;
                pProcess.StartInfo.RedirectStandardOutput = true;
                pProcess.StartInfo.CreateNoWindow = true;

                pProcess.Start();

                string strOutput = pProcess.StandardOutput.ReadToEnd();
                pProcess.WaitForExit();

                return strOutput;    
            }           
        }
    }
}
