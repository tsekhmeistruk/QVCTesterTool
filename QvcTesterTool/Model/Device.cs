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
using System.Windows.Threading;

namespace QvcTesterTool.Model
{
    public class Device : INotifyPropertyChanged
    {
        #region Private Fields

        private string _id;
        private string _model;
        private string _osVersion;
        private string _sdkVersion;
        private string _battery;

        private Build _selectedPackage;

        private ObservableCollection<Build> _packages;
        Dispatcher _dispatcher;

        #endregion

        #region Public Properties

        public Build SelectedPackage
        {
            get
            {
                return _selectedPackage;
            }
            set
            {
                if (this._selectedPackage == value || value == null) return;
                _selectedPackage = value;
                OnPropertyChanged("SelectedPackage");
            }
        }

        public ObservableCollection<Build> Packages
        {
            get
            {
                if (_packages == null)
                {
                    _packages = new ObservableCollection<Build>();
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

        #endregion //Public Properties

        #region Constructor

        public Device(string id)
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
            _id = id;
            Task.Run(new Action(Initialize));
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion // INotifyPropertyChanged Implementation

        #region Public Methods

        public void UpdatePackagesList()
        {
            _dispatcher.Invoke(() => Packages.Clear());
            var packages = AdbShell.GetAllPackagesList(_id);
            var qvcPackages = packages.Where((x) => x.Contains("com.qvc") || x.Contains("com.qvcuk") || x.Contains("de.qvc")).ToList();
            _dispatcher.Invoke(()=>qvcPackages.ForEach((p) => Packages.Add(new Build(p, _id))));
        }

        #endregion //Public Methods

        #region Private Methods

        private void Initialize()
        {
            Model = AdbShell.GetDeviceModel(_id);
            OsVersion = AdbShell.GetOsVersion(_id);
            SdkVersion = AdbShell.GetSdkVersion(_id);
            Battery = AdbShell.GetBatteryLevel(_id);
        }

        #endregion //Private Methods

        #region Update Command

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
            UpdatePackagesList();
        }

        #endregion //Update Command

        #region Uninstall Command

        private ICommand _uninstallCommand;

        public ICommand UninstallCommand
        {
            get
            {
                if (_uninstallCommand == null)
                {
                    _uninstallCommand = new RelayCommand(
                        param => this.UninstallObject(),
                        param => this.CanUninstall()
                    );
                }
                return _uninstallCommand;
            }
        }

        private bool CanUninstall()
        {
            return (_selectedPackage != null && !String.IsNullOrEmpty(_selectedPackage.PackageName));
        }

        private void UninstallObject()
        {
            AdbShell.UninstallApk(SelectedPackage.PackageName, SelectedPackage.DeviceId);
            Packages.Remove(Packages.Where(x => x.PackageName == SelectedPackage.PackageName).FirstOrDefault());
            SelectedPackage = new Build("", Id);
        }

        #endregion // Uninstall Command

        #region ClearData Command

        private ICommand _clearCommand;

        public ICommand ClearCommand
        {
            get
            {
                if (_clearCommand == null)
                {
                    _clearCommand = new RelayCommand(
                        param => this.ClearObject(),
                        param => this.CanClear()
                    );
                }
                return _clearCommand;
            }
        }

        private bool CanClear()
        {
            return (_selectedPackage != null && !String.IsNullOrEmpty(_selectedPackage.PackageName));
        }

        private void ClearObject()
        {
            AdbShell.ClearDataApp(SelectedPackage.PackageName, SelectedPackage.DeviceId);
        }

        #endregion

        #region ForceStop Command

        private ICommand _forceStopCommand;

        public ICommand ForceStopCommand
        {
            get
            {
                if (_forceStopCommand == null)
                {
                    _forceStopCommand = new RelayCommand(
                        param => this.ForceStopObject(),
                        param => this.CanForceStop()
                    );
                }
                return _forceStopCommand;
            }
        }

        private bool CanForceStop()
        {
            return (_selectedPackage != null && !String.IsNullOrEmpty(_selectedPackage.PackageName));
        }

        private void ForceStopObject()
        {
            AdbShell.ForceStopApp(SelectedPackage.PackageName, SelectedPackage.DeviceId);
        }

        #endregion

        #region Open Command

        private ICommand _openCommand;

        public ICommand OpenCommand
        {
            get
            {
                if (_openCommand == null)
                {
                    _openCommand = new RelayCommand(
                        param => this.OpenObject(),
                        param => this.CanOpen()
                    );
                }
                return _openCommand;
            }
        }

        private bool CanOpen()
        {
            return (_selectedPackage != null && !String.IsNullOrEmpty(_selectedPackage.PackageName));
        }

        private void OpenObject()
        {
            AdbShell.RunApp(SelectedPackage.PackageName, SelectedPackage.DeviceId);
        }

        #endregion
    }
}