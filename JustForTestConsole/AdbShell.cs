using System;
using System.Collections.Generic;
using JustForTestConsole.Data;
using JustForTestConsole.Helpers;
using JustForTestConsole.Interfaces;
using System.Text.RegularExpressions;

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
            string cmd = ReplaceMarks(DataStrings.adbPackages, deviceId);
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

        public string GetBuildNumber(string package, string deviceId)
        {
            string cmd = String.Format("adb -s {1} shell \"dumpsys package {0} | grep versionName\"", package, deviceId);
            return CmdExecute(cmd);
        }

        public string GetDeviceModel(string deviceId)
        {
            string cmd = String.Format("adb -s {0} shell \"getprop ro.product.model\"", deviceId);
            string output = CmdExecute(cmd);
            return Regex.Replace(output, "\r|\n|\t", "");
        }

        public string GetOsVersion(string deviceId)
        {
            string cmd = String.Format("adb -s {0} shell \"getprop ro.build.version.release\"", deviceId);
            string output = CmdExecute(cmd);
            return Regex.Replace(output, "\r|\n|\t", "");
        }

        public string GetSdkVersion(string deviceId)
        {
            string cmd = String.Format("adb -s {0} shell \"getprop ro.build.version.sdk\"", deviceId);
            string output = CmdExecute(cmd);
            return Regex.Replace(output, "\r|\n|\t", "");
        }

        public string GetBatteryLevel(string deviceId)
        {
            string cmd = String.Format("adb -s {0} shell cat /sys/class/power_supply/battery/*", deviceId);
            string output = CmdExecute(cmd);
            string pattern = "(POWER_SUPPLY_CAPACITY=)(\\d+)";
            Regex regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.Multiline);
            output = regex.Match(output).Groups[2].Value;
            return Regex.Replace(output, "\r|\n|\t", "");
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
