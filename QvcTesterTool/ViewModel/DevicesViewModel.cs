﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JustForTestConsole;
using JustForTestConsole.Helpers;
using JustForTestConsole.Interfaces;
using System.Management;
using QvcTesterTool.Model;

namespace QvcTesterTool.ViewModel
{
    public class DevicesViewModel
    {
        public ObservableCollection<Device> Devices { get; set; }

        public DevicesViewModel()
        {
            Initialize();
        }

        private void Initialize()
        {
            Devices = new ObservableCollection<Device>();
            foreach (var item in AdbShell.GetDevices())
            {
                Devices.Add(new Device(item));
            }
        }
    }
}
