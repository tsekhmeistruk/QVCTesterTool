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
using JustForTestConsole.Data;

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

            
            
                //var query = new WqlEventQuery();
                //query.EventClassName = "__InstanceCreationEvent";
                //query.WithinInterval = new TimeSpan(5000000);
                //query.Condition = @"TargetInstance ISA 'Win32_PnPEntity'";
                //var watcher = new ManagementEventWatcher(query);
                //watcher.EventArrived += new EventArrivedEventHandler((x, y) => Console.WriteLine("USB"));
                //watcher.Start();

            DateTimeConverter g = new DateTimeConverter();

            var t2 = DateTime.Parse("Friday September 11, 2015 11:18AM");
            var t = DateTime.Parse("Thursday August 20, 2015 11:36AM");
            var result = t.Subtract(t2);

            Console.WriteLine(t);
            var t1 = t.AddHours(7);
            Console.WriteLine(t1);
            Console.WriteLine(result);
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
