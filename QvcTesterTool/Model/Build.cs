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
        private string buildCulture;
        private string buildType;
        private string buildKind;
        private string buildNumber;

        private string _deviceId;
        public string BuildCulture { get; private set; }
        public string BuildType { get; private set; }
        public string BuildKind { get; private set; }
        public string BuildNumber { get; private set; }

        public Build(string packageName, string deviceId)
        {
            Initialize(packageName, _deviceId);
        }

        private void Initialize(string package, string deviceId)
        {
            BuildInitializer(package, out buildCulture, out buildType, out buildKind, out buildNumber);
            _deviceId = deviceId;
        }

        private void BuildInitializer(string package, out string buildCulture, out string buildType, out string buildKind, out string buildNumber)
        {
            buildCulture = RecognizeCulture(package);
            buildType = RecognizeBuildType(package);
            buildKind = RecognizeBuildKind(package);
            buildNumber = RecognizeBuildNumber(package, _deviceId);
        }

        private string RecognizeCulture(string package)
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

        private string RecognizeBuildType(string package)
        {
            if (package.Contains("qa"))
            {
                return "QA";
            }
            if (package.Contains("stage"))
            {
                return "STAGE";
            }
            else
            {
                return "-";
            }
        }

        private string RecognizeBuildKind(string package)
        {
            if (package.Contains("ci"))
            {
                return "Ci";
            }
            else
            if(package.Contains("tabletopt")){
                return "TabletOpt";
            }
            else
            {
                return "-";
            }
        }

        private string RecognizeBuildNumber(string package, string deviceId)
        {
            AdbShell adb = new AdbShell();
            return adb.GetBuildNumber(package, deviceId);
        }
    }
}
