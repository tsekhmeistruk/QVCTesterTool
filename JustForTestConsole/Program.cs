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
            string build;
            string date;
            string webadress = DataStrings.webStage.Replace("*culture*", "us");

            WebPageParser.FetchBuild(out build, out date, webadress);
            Console.WriteLine("Biuld: {0} \nDate: {1}", build, date);
            
            
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
