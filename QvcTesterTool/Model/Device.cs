using JustForTestConsole;
using QvcTesterTool.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QvcTesterTool.Model
{
    public class Device : INotifyPropertyChanged
    {
        private string _id;
        private string _model;
        private string _osVersion;
        private string _sdkVersion;
        private string _battery;

        private ObservableCollection<string> _packages;

        public ObservableCollection<string> Packages 
        { 
            get 
            {
                if (_packages == null)
                {
                    _packages = new ObservableCollection<string>() 
                    { 
                        "stub1"
                    };
                }
                return _packages;
            }
            set
            {
                _packages = value;
            }
        }
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public string Model
        {
            get
            {
                return _model;
            }
            set
            {
                _model = value;
                OnPropertyChanged("Model");
            }
        }

        public string OsVersion
        {
            get
            {
                return _osVersion;
            }
            set
            {
                _osVersion = value;
                OnPropertyChanged("OsVersion");
            }
        }

        public string SdkVersion
        {
            get
            {
                return _sdkVersion;
            }
            set
            {
                _sdkVersion = value;
                OnPropertyChanged("SdkVersion");
            }
        }

        public string Battery
        {
            get
            {
                return _battery;
            }
            set
            {
                _battery = value;
                OnPropertyChanged("Battery");
            }
        }

        public Device(string id)
        {
            Initialize(id);
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void UpdatePackagesList()
        {
            AdbShell adb = new AdbShell();
            Packages.Clear();
            var packages = adb.GetAllPackagesList(_id);
            packages.ForEach((p) => Packages.Add(p));
        }

        private void Initialize(string id)
        {
             Id = id;
             Model = GetDeviceModel(id);
             OsVersion = GetOsVersion(id);
             SdkVersion = GetSdkVersion(id);
             Battery = GetBatteryLevel(id);
        }

        private string GetBatteryLevel(string id)
        {
            AdbShell adb = new AdbShell();
            return adb.GetBatteryLevel(id);
        }

        private string GetSdkVersion(string id)
        {
            AdbShell adb = new AdbShell();
            return adb.GetSdkVersion(id);
        }

        private string GetDeviceModel(string id)
        {
            AdbShell adb = new AdbShell();
            return adb.GetDeviceModel(id);      
        }

        private string GetOsVersion(string id)
        {
            AdbShell adb = new AdbShell();
            return adb.GetOsVersion(id);
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
            UpdatePackagesList();
            // Update command execution logic
        }
    }
}
