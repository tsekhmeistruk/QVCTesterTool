using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JustForTestConsole;
using JustForTestConsole.Helpers;
using JustForTestConsole.Interfaces;
using System.Management;

namespace QvcTesterTool.ViewModel
{
    public class DevicesViewModel
    {
        private IAdbShell _adb;
        public ObservableCollection<Device> Devices { get; set; }

        public DevicesViewModel()
        {
            _adb = new AdbShell();
            Initialize();
        }

        private void Initialize()
        {
            Devices = new ObservableCollection<Device>();
            foreach (var item in _adb.GetDevices())
            {
                Devices.Add(new Device(item));
            }
        }
    }
}
