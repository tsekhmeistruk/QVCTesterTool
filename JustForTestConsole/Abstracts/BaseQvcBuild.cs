using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;
using JustForTestConsole.Data;
using JustForTestConsole.Enums;
using JustForTestConsole.Interfaces;

namespace JustForTestConsole.Abstracts
{
    abstract public class BaseQvcBuild: IBuild
    {
        #region Protected Fields

        protected string _apk;
        protected string _webSite;
        protected string _link;
        protected string _packageName;
        protected string _activity;
        protected readonly Culture _culture;

        #endregion

        #region Constructors

        protected BaseQvcBuild(Culture culture)
        {
            _culture = culture;
            Initialize(culture);
        }

        #endregion

        #region Public Methods

        public void Install(string deviceId)
        {
            InstallBuild(deviceId);
        }

        public void Uninstall(string deviceId)
        {
            UnInstallBuild(deviceId);
        }

        public void Download()
        {
            DownloadBuild();
        }

        public void ClearData(string deviceId)
        {
            ClearDataApp(deviceId);
        }

        public void ForceStop(string deviceId)
        {
            ForceStopApp(deviceId);
        }

        public void CheckNewBuild()
        {
            CheckLastBuild();
        }

        #endregion

        #region Protected Abstract Methods

        protected abstract void InstallBuild(string deviceId);
        protected abstract void UnInstallBuild(string deviceId);
        protected abstract void DownloadBuild();
        protected abstract void ClearDataApp(string deviceId);
        protected abstract void ForceStopApp(string deviceId);
        protected abstract void CheckLastBuild();

        protected abstract void InitialFields();

        #endregion

        protected string FormatString(string command, Culture culture)
        {
            string output = command.Replace("*culture*", culture.ToString());
            return output;
        }

        protected string FormatString(string command, Culture culture, string deviceId)
        {
            string output = FormatString(command, culture);
            output = output.Replace("*deviceId*", deviceId);
            return output;
        }

        private void Initialize(Culture culture)
        {
            _activity = DataStrings.activity;
            InitialFields();
        }

        protected void CmdExecute(string command)
        {
            CmdShell.Execute(command);
        }
    }
}
