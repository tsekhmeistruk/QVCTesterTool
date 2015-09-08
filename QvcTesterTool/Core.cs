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
        #region Private Fields

        private Device _selectedDevice;
        private ObservableCollection<Device> _devices;
        private Dispatcher _dispatcher;

        #endregion

        #region Public Properties and Indexers

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

        public Device this[string index]
        {
            get
            {
                var device = Devices.Where(x => x.Id == index).FirstOrDefault();
                return device;
            }
        }

        #endregion

        #region Constructor

        public Core()
        {
            Initialize();
        }

        #endregion

        #region Public Methods

        public void UpdateDevicesList()
        {
            Devices.Clear();
            var devices = AdbShell.GetDevices().Select((id) => new Device(id)).ToList();
            devices.ForEach((x) => Devices.Add(x));
        }

        #endregion

        #region Private Methods

        private void Initialize()
        {
            Devices = new ObservableCollection<Device>();
            _dispatcher = Dispatcher.CurrentDispatcher;
            StartUsbEvent();
            if (Devices.Count > 0)
            {
                SelectedDevice = Devices[0];
                SelectedDevice.UpdatePackagesList();
            }
        }

        private void StartUsbEvent()
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

        #endregion

        #region Commands

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
        }

        private void UpdateObject()
        {
            UpdateDevicesList();
        }

        #endregion //Commands
    }
}