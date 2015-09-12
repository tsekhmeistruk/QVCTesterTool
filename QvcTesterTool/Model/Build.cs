using JustForTestConsole;
using QvcTesterTool.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QvcTesterTool.Model
{
    public class Build: INotifyPropertyChanged
    {
        #region Private Fields

        private string _package;
        private string _buildCulture;
        private string _buildType;
        private string _buildKind;
        private string _buildNumber;
        private string _activity;

        private string _deviceId;

        #endregion

        #region Public Properties

        public string PackageName
        {
            get
            {
                return _package;
            }

            set
            {
                _package = value;
                OnPropertyChanged("PackageName");
            }
        }

        public string BuildCulture 
        {
            get
            {
                return _buildCulture;
            }

            set
            {
                _buildCulture = value;
                OnPropertyChanged("BuildCulture");
            }
        }

        public string BuildType
        {
            get
            {
                return _buildType;
            }

            set
            {
                _buildType = value;
                OnPropertyChanged("BuildType");
            }
        }

        public string BuildKind
        {
            get
            {
                return _buildKind;
            }

            set
            {
                _buildKind = value;
                OnPropertyChanged("BuildKind");
            }
        }

        public string BuildNumber
        {
            get
            {
                return _buildNumber;
            }

            set
            {
                _buildNumber = value;
                OnPropertyChanged("BuildNumber");
            }
        }

        public string Activity
        {
            get
            {
                return _activity;
            }

            set
            {
                _activity = value;
                OnPropertyChanged("Activity");
            }
        }

        public string DeviceId
        {
            get
            {
                return _deviceId;
            }

            set
            {
                _deviceId = value;
            }
        }

        #endregion //Public Properties

        #region Constructor

        public Build(string packageName, string deviceId)
        {
            Initialize(packageName, deviceId);
        }

        #endregion //Constructor

        #region Public Methods

        #endregion

        #region Private Methods

        private void Initialize(string package, string deviceId)
        {
            _deviceId = deviceId;
            _package = package;
            BuildInitializer(package);
        }

        private void BuildInitializer(string package)
        {
            _buildCulture = GetBuildCulture(package);
            _buildType = GetBuildType(package);
            _buildKind = GetBuildKind(package);
            _activity = GetActivity(_deviceId, package);
            _buildNumber = GetBuildNumber(_deviceId, package);
        }

        private string GetBuildCulture(string package)
        {
            if (package.Contains("com.qvc."))
            {
                return "US";
            }
            if (package.Contains("com.qvcuk."))
            {
                return "UK";
            }
            if (package.Contains("de.qvc."))
            {
                return "DE";
            }
            else
            {
                return "-";
            }
        }

        private string GetBuildType(string package)
        {
            if (package.Contains("qa"))
            {
                return "Qa";
            }
            if (package.Contains("stage"))
            {
                return "Stage";
            }
            else
            {
                return "-";
            }
        }

        private string GetBuildKind(string package)
        {
            if (package.Contains("ci"))
            {
                return "Ci";
            }
            else
            if(package.Contains("tabletopt")){
                return "-";
            }
            else
            {
                return "-";
            }
        }

        private string GetBuildNumber(string deviceId, string package)
        {
            return AdbShell.GetBuildNumber(deviceId, package);
        }

        private string GetActivity(string deviceId, string package)
        {
            return AdbShell.GetActivity(_deviceId, package);
        }

        #endregion //Private Methods

        #region Commands

        

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
    }
}
