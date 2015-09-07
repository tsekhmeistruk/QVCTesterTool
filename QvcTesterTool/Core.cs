using JustForTestConsole;
using QvcTesterTool.Commands;
using QvcTesterTool.Model;
using QvcTesterTool.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace QvcTesterTool
{
    public class Core
    {
        AdbShell adb;
        private Device _selectedDevice;
        private ObservableCollection<Device> _devices;
        private Dispatcher _dispatcher;

        public Device SelectedDevice
        {
            get
            {
                return _selectedDevice;
            }
            set
            {
                if (this._selectedDevice == value || value == null) return;
                _selectedDevice = value;
                _selectedDevice.UpdatePackagesList();
            }
        }

        public ObservableCollection<Device> Devices
        {
            get
            {
                if (_devices == null)
                {
                    _devices = new ObservableCollection<Device>();
                }
                return _devices;
            }
            private set
            {
                _devices = value;
                UpdateDevicesList();
            }
        }

        public Core()
        {
            Initialize();
        }

        public Device this[string index]
        {
            get
            {
                var device = Devices.Where(x => x.Id == index).FirstOrDefault();
                return device;
            }
        }

        private void Initialize()
        {
            adb = new AdbShell();
            Devices = new ObservableCollection<Device>();
            _dispatcher = Dispatcher.CurrentDispatcher;
            SetEvent();
            if (Devices.Count > 0)
            {
                SelectedDevice = Devices[0];
                SelectedDevice.UpdatePackagesList();
            }
        }

        private void SetEvent()
        {
            using (var watcher = new ManagementEventWatcher())
            {
                var query = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");
                watcher.EventArrived += new EventArrivedEventHandler((x, y) => _dispatcher.Invoke(UpdateDevicesList));
                watcher.Query = query;
                watcher.Start();
            }

            using (var watcher = new ManagementEventWatcher())
            {
                var query = new WqlEventQuery("SELECT * FROM __InstanceDeletionEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");
                watcher.EventArrived += new EventArrivedEventHandler((x, y) => _dispatcher.Invoke(UpdateDevicesList));
                watcher.Query = query;
                watcher.Start();
            } 
        }

        public void UpdateDevicesList()
        {
            Devices.Clear();
            var devices = adb.GetDevices().Select((id) => new Device(id)).ToList();
            devices.ForEach((x) => Devices.Add(x));
        }

        private void UpdateBuildsList()
        {
            
        }

        private ICommand _updateCommand;


        public ICommand UpdateCommand
        {
            get
            {
                if (_updateCommand == null)
                {
                    _updateCommand = new RelayCommand(
                        param => this.UpdateObject(),
                        param => this.CanUpdate()
                    );
                }
                return _updateCommand;
            }
        }

        private bool CanUpdate()
        {
            return true;
            // Verify command can be executed here
        }

        private void UpdateObject()
        {
            UpdateDevicesList();
            // Update command execution logic
        }
    }
}