using System;
using JustForTestConsole.Abstracts;
using JustForTestConsole.Data;
using JustForTestConsole.Enums;
using JustForTestConsole.Interfaces;

namespace JustForTestConsole.Builds
{
    public class QaBuild: BaseQvcBuild
    {
        public QaBuild(Culture culture) : base(culture)
        {
        }

        protected override void InstallBuild(string deviceId)
        {
            string path = String.Format("{0}{1}", DataStrings.path, _apk);
            AdbShell.InstallApk(path, deviceId);
        }

        protected override void UnInstallBuild(string deviceId)
        {
            AdbShell.UninstallApk(_packageName, deviceId);
        }

        protected override void DownloadBuild()
        {
            string cmd = String.Format("{0}{1}{2}{3}{4}{5}", DataStrings.pShellDownload, _link, "', '", DataStrings.path, _apk, "')}\"");
            CmdExecute(cmd);
        }

        protected override void ClearDataApp(string deviceId)
        {
            throw new NotImplementedException();
        }

        protected override void ForceStopApp(string deviceId)
        {
            throw new NotImplementedException();
        }

        protected override void CheckLastBuild()
        {
            throw new NotImplementedException();
        }

        protected override void InitialFields()
        {
            _webSite = FormatString(DataStrings.webQa, _culture);
            _link = FormatString(DataStrings.linkQa, _culture);
            _packageName = DataStrings.QaPackageName;
            _apk = FormatString(DataStrings.QaApk, _culture);
        }
    }
}
