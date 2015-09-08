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
        #region Private Fields

        private string _id;
        private string _model;
        private string _osVersion;
        private string _sdkVersion;
        private string _battery;

        private Build _selectedPackage;

        private ObservableCollection<Build> _packages;

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
            Initialize(id);
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
            Packages.Clear();
            var packages = AdbShell.GetAllPackagesList(_id);
            var qvcPackages = packages.Where((x) => x.Contains("com.qvc") || x.Contains("com.qvcuk") || x.Contains("de.qvc")).ToList();
            qvcPackages.ForEach((p) => Packages.Add(new Build(p, _id)));
        }

        #endregion //Public Methods

        #region Private Methods

        private void Initialize(string id)
        {
             Id = id;
             Model = AdbShell.GetDeviceModel(id);
             OsVersion = AdbShell.GetOsVersion(id);
             SdkVersion = AdbShell.GetSdkVersion(id);
             Battery = AdbShell.GetBatteryLevel(id);
        }

        #endregion //Private Methods

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
            UpdatePackagesList();
        }

        #endregion //Commands
    }
}
