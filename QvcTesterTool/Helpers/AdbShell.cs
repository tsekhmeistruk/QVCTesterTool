﻿using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using QvcTesterTool.Data;

namespace QvcTesterTool.Helpers
{
    public static class AdbShell
    {
        #region Public Methods

        public static async Task<string> InstallApkAsync(string path, string deviceId)
        {
            string rawCmd = ReplaceMarks(DataStrings.adbInstall, deviceId);
            string cmd = String.Format("{0}{1}", rawCmd, path);
            return await CmdExecuteTask(cmd);
        }

        public static string UninstallApk(string package, string deviceId)
        {
            string rawCmd = ReplaceMarks(DataStrings.adbUninstall, deviceId);
            string cmd = String.Format("{0}{1}", rawCmd, package);
            return CmdExecute(cmd);
        }

        public static string ForceStopApp(string package, string deviceId)
        {
            string rawCmd = ReplaceMarks(DataStrings.adbForceStop, deviceId);
            string cmd = String.Format("{0}{1}", rawCmd, package);
            return CmdExecute(cmd);
        }

        public static string ClearDataApp(string package, string deviceId)
        {
            string rawCmd = ReplaceMarks(DataStrings.adbClear, deviceId);
            string cmd = String.Format("{0}{1}", rawCmd, package);
            return CmdExecute(cmd);
        }

        public static string RunApp(string package, string deviceId)
        {
            string cmd = String.Format("adb -s {0} shell am start -n {1}/{2}",deviceId, package, DataStrings.activity);
            return CmdExecute(cmd);
        }

        public static List<string> GetDevices()
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

        public static List<string> GetAllPackagesList(string deviceId)
        {
            string cmd = ReplaceMarks(DataStrings.adbPackages, deviceId);
            var packages = CmdExecute(cmd);
            List<string> packagesList = StringHelpers.GetPackagesList(packages);
            return packagesList;
        }

        public static List<string> GetPackagesList(string deviceId, string filter)
        {
            string cmd = ReplaceMarks(DataStrings.adbPackagesGrep, deviceId, filter);
            var packages = CmdExecute(cmd);
            List<string> packagesList = StringHelpers.GetPackagesList(packages);
            return packagesList;
        }

        public static string GetBuildNumber(string deviceId, string package)
        {
            string cmd = String.Format("adb -s {0} shell \"dumpsys package {1}\"", deviceId, package);
            string output = CmdExecute(cmd);

            string pattern = "(versionName=)(.*)\r\r\n";
            Regex regex = new Regex(pattern, RegexOptions.Multiline | RegexOptions.Compiled);
            var versionNumber = regex.Match(output).Groups[2].Value;
            return versionNumber;
        }

        public static string GetDeviceModel(string deviceId)
        {
            string cmd = String.Format("adb -s {0} shell \"getprop ro.product.model\"", deviceId);
            string output = CmdExecute(cmd);
            return Regex.Replace(output, "\r|\n|\t", "");
        }

        public static string GetOsVersion(string deviceId)
        {
            string cmd = String.Format("adb -s {0} shell \"getprop ro.build.version.release\"", deviceId);
            string output = CmdExecute(cmd);
            return Regex.Replace(output, "\r|\n|\t", "");
        }

        public static string GetSdkVersion(string deviceId)
        {
            string cmd = String.Format("adb -s {0} shell \"getprop ro.build.version.sdk\"", deviceId);
            string output = CmdExecute(cmd);
            return Regex.Replace(output, "\r|\n|\t", "");
        }

        public static string GetBatteryLevel(string deviceId)
        {
            string cmd = String.Format("adb -s {0} shell cat /sys/class/power_supply/battery/*", deviceId);
            string output = CmdExecute(cmd);
            string pattern = "(POWER_SUPPLY_CAPACITY=)(\\d+)";
            Regex regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.Multiline);
            output = regex.Match(output).Groups[2].Value;
            return Regex.Replace(output, "\r|\n|\t", "");
        }

        public static string GetActivity(string deviceId, string package)
        {
            string cmd = String.Format("adb -s {0} shell \"dumpsys package {1} | grep EULA\"", deviceId, package);
            string output = CmdExecute(cmd);
            string pattern = "(.*)(/)(.*)(\\s)(\\w+)(\\s)(\\w+)";
            Regex regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.Multiline);
            output = regex.Match(output).Groups[3].Value;
            return Regex.Replace(output, "\r|\n|\t", "");
        }

        public static void ResetDevice(string deviceId)
        {
            string cmd = String.Format("adb -s {0} reboot", deviceId);
            var output = CmdExecute(cmd);
        }

        public static string MakeScreenshot(string deviceId)
        {
            string date = DateTime.Now.ToString("yyyy-M-d_hh.mm.ss");
            string cmd = String.Format("adb -s {0} shell screencap -p /sdcard/{1}.png", deviceId, date);
            CmdExecute(cmd);
            return date;
        }

        public static void TakeScreenshot(string deviceId, string screenshotName, string path)
        {
            string cmd = String.Format("adb -s {0} pull /sdcard/{1}.png {2}", deviceId, screenshotName, path);
            CmdExecute(cmd);
            RemoveScreenshot(deviceId, screenshotName);
        }

        public static void RemoveScreenshot(string deviceId, string screenshotName, string path = "/sdcard/")
        {
            string cmd = String.Format("adb -s {0} shell rm {1}{2}.png", deviceId, path, screenshotName);
            CmdExecute(cmd);
        }

        public static void StartLog(string deviceId)
        {
            string cmd = String.Format("adb -s {0} logcat -c", deviceId);
            CmdExecute(cmd);
        }

        public static void StopAndGetLog(string deviceId, string path)
        {
            string cmd = String.Format("adb -s {0} logcat -d {1}", deviceId, path);
            CmdExecute(cmd);
        }

        #endregion //Public Methods

        #region Private Methods

        private static Task<string> CmdExecuteTask(string command)
        {
            return Task<string>.Run(() => CmdShell.Execute(command));
            
        }

        private static string CmdExecute(string command)
        {
            return CmdShell.Execute(command);

        }

        private static string ReplaceMarks(string rawCommand, string deviceId)
        {
            return rawCommand.Replace("*deviceId*", deviceId);
        }

        private static string ReplaceMarks(string rawCommand, string deviceId, string filter)
        {
            string output = ReplaceMarks(rawCommand, deviceId);
            return output.Replace("*filter*", filter);
        }

        #endregion //Private Methods
    }
}
