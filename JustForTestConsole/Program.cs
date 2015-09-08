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

            string f = "eifje__{1}__ifjeifj{0}";
            string h = String.Format(f, "x2", "x1");

            Console.WriteLine(h);
            
            Console.ReadKey();
        }

        

        private static void TestWeb()
        {
            //string build, buildDate;

            //WebPageParser.FetchBuild(out build,out buildDate, BuildType.Qa);
            //Console.WriteLine(build);
            //Console.WriteLine(buildDate);
        }
    }
}
