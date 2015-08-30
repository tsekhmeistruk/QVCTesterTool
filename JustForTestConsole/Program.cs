using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Management;
using System.Net;
using System.Threading;
using JustForTestConsole.Enums;
using JustForTestConsole.Helpers;
using Microsoft.PowerShell.Core.Activities;
using System.Text.RegularExpressions;

namespace JustForTestConsole
{
    internal class Program
    {


        private static void Main()
        {
            //var watcher = new ManagementEventWatcher();
            //var query = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2");
            //watcher.EventArrived += new EventArrivedEventHandler((x, y) => Console.WriteLine("USB"));
            //watcher.Query = query;
            //watcher.Start();
            //Thread.Sleep(50000000);
            

            //RunApp();
            //TestWeb();
            //string input = "all devices are attached\nHJ763764  device\n34djfhJH   device";
            string input = CmdShell.Execute("adb devices");
            string pattern = "(\\w+)(\\t)(device)\r\n";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
            foreach (Match device in regex.Matches(input))
            {
                Console.WriteLine(device.Groups[1].Value);
            }

            //var output = CmdShell.Execute("adb shell \"pm list packages\"");
            //foreach (var pack in StringHelpers.GetPackagesList(output))
            //{
            //    Console.WriteLine(pack);
            //}
            Console.ReadKey();
        }

        

        private static void TestWeb()
        {
            //string build, buildDate;

            //WebPageParser.FetchBuild(out build,out buildDate, BuildType.Qa);
            //Console.WriteLine(build);
            //Console.WriteLine(buildDate);
        }

        private static void RunApp()
        {
            var androidSdk = new Adb();
            while (true)
            {
                Console.WriteLine("Download Qa?");
                Console.ReadKey();
                androidSdk.DownloadNewBuild(BuildType.Qa);

                Console.WriteLine("Download Stage?");
                Console.ReadKey();
                androidSdk.DownloadNewBuild(BuildType.Stage);

                Console.WriteLine("Uninstall All?");
                Console.ReadKey();
                androidSdk.UninstallBuild();

                Console.WriteLine("Install Qa?");
                Console.ReadKey();
                androidSdk.InstallBuild(BuildType.Qa);

                Console.WriteLine("Complete");
            }
        }
    }
}
