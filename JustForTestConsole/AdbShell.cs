using System;
using System.Collections.Generic;
using JustForTestConsole.Data;
using JustForTestConsole.Helpers;
using JustForTestConsole.Interfaces;

namespace JustForTestConsole
{
    public class AdbShell: IAdbShell
    {
        public string InstallApk(string path, string deviceId)
        {
            string rawCmd = ReplaceMarks(DataStrings.adbInstall, deviceId);
            string cmd = String.Format("{0}{1}", rawCmd, path);
            return CmdExecute(cmd);
        }

        public string UninstallApk(string package, string deviceId)
        {
            string rawCmd = ReplaceMarks(DataStrings.adbUninstall, deviceId);
            string cmd = String.Format("{0}{1}", rawCmd, package);
            return CmdExecute(cmd);
        }

        public string ForceStopApp(string package, string deviceId)
        {
            string rawCmd = ReplaceMarks(DataStrings.adbForceStop, deviceId);
            string cmd = String.Format("{0}{1}", rawCmd, package);
            return CmdExecute(cmd);
        }

        public string ClearDataApp(string package, string deviceId)
        {
            string rawCmd = ReplaceMarks(DataStrings.adbClear, deviceId);
            string cmd = String.Format("{0}{1}", rawCmd, package);
            return CmdExecute(cmd);
        }

        public string RunApp(string package, string deviceId, string activity)
        {
            throw new NotImplementedException();
        }

        public List<string> GetDevices()
        {
            List<string> devicesList = new List<string>();
            var devices = CmdShell.Execute(DataStrings.adbDevices);
            var ids = StringHelpers.GetDevicesId(devices);
            foreach (var item in ids)
            {
                devicesList.Add(item);
            }
            return devicesList;
        }

        public List<string> GetAllPackagesList(string deviceId)
        {
            const string filter = ".";
            string cmd = ReplaceMarks(DataStrings.adbPackagesGrep, deviceId, filter);
            var packages = CmdExecute(cmd);
            List<string> packagesList = StringHelpers.GetPackagesList(packages);
            return packagesList;
        }

        public List<string> GetPackagesList(string filter, string deviceId)
        {
            //for qvc package filter should be 'com.qvc'
            string cmd = ReplaceMarks(DataStrings.adbPackagesGrep, deviceId, filter);
            var packages = CmdExecute(cmd);
            List<string> packagesList = StringHelpers.GetPackagesList(packages);
            return packagesList;
        }

        #region Private Methods

        private string CmdExecute(string command)
        {
            return CmdShell.Execute(command);
        }

        private string ReplaceMarks(string rawCommand, string deviceId)
        {
            return rawCommand.Replace("*deviceId*", deviceId);
        }

        private string ReplaceMarks(string rawCommand, string deviceId, string filter)
        {
            string output = ReplaceMarks(rawCommand, deviceId);
            return output.Replace("*filter*", filter);
        }

        #endregion

    }
}
