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

            AdbShell adb = new AdbShell();
            var t = adb.GetDevices();
            var m = adb.GetDeviceModel(t[0]);

            string lineSeparator = ((char)0x2028).ToString();
            string paragraphSeparator = ((char)0x2029).ToString();

            string luxm = m.Replace("\r\n", string.Empty).Replace("\n", string.Empty).Replace("\f", string.Empty).Replace("\r", string.Empty).Replace(lineSeparator, string.Empty).Replace(paragraphSeparator, string.Empty);
            Console.Write(luxm + 88);
            
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
