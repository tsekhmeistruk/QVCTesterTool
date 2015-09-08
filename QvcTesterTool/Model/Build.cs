using JustForTestConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QvcTesterTool.Model
{
    public class Build
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

            private set
            {
                _package = value;
            }
        }

        public string BuildCulture 
        {
            get
            {
                return _buildCulture;
            }

            private set
            {
                _buildCulture = value;
            }
        }

        public string BuildType
        {
            get
            {
                return _buildType;
            }

            private set
            {
                _buildType = value;
            }
        }
        public string BuildKind
        {
            get
            {
                return _buildKind;
            }

            private set
            {
                _buildKind = value;
            }
        }

        public string BuildNumber
        {
            get
            {
                return _buildNumber;
            }

            private set
            {
                _buildNumber = value;
            }
        }

        public string Activity
        {
            get
            {
                return _activity;
            }

            private set
            {
                _activity = value;
            }
        }

        #endregion //Public Properties

        #region Constructor

        public Build(string packageName, string deviceId)
        {
            Initialize(packageName, deviceId);
        }

        #endregion //Constructor

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
            _buildNumber = GetBuildNumber(_deviceId, package);
            _activity = GetActivity(_deviceId, package);
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
    }
}
