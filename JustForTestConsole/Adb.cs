using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using JustForTestConsole.Enums;
using JustForTestConsole.Exceptions;

namespace JustForTestConsole
{
    public class Adb
    {
        private CmdShell _shell;

        private CmdShell Shell
        {
            get { return _shell ?? (_shell = new CmdShell()); }
        }

        public void DownloadNewBuilds()
        {
            CmdShell.Execute(
                       "powershell -command \"& { (New-Object Net.WebClient).DownloadFile('https://dl.dropbox.com/u/25719532/apps/android_us_tabletopt_qa/QVC_US_TabletOpt_QA.apk', 'd:\\QVC_US_TabletOpt_QA.apk')}\"");
            CmdShell.Execute(
                       "powershell -command \"& { (New-Object Net.WebClient).DownloadFile('https://dl.dropbox.com/u/25719532/apps/android_us_tabletopt_stage/QVC_US_TabletOpt_Stage.apk', 'd:\\QVC_US_TabletOpt_Stage.apk')}\"");
        }

        public void DownloadNewBuild(BuildType buildType)
        {
            switch (buildType)
            {
                case BuildType.Qa:
                    CmdShell.Execute(
                        "powershell -command \"& { (New-Object Net.WebClient).DownloadFile('https://dl.dropbox.com/u/25719532/apps/android_us_tabletopt_qa/QVC_US_TabletOpt_QA.apk', 'd:\\QVC_US_TabletOpt_QA.apk')}\"");
                    break;
                case BuildType.Stage:
                    CmdShell.Execute(
                        "powershell -command \"& { (New-Object Net.WebClient).DownloadFile('https://dl.dropbox.com/u/25719532/apps/android_us_tabletopt_stage/QVC_US_TabletOpt_Stage.apk', 'd:\\QVC_US_TabletOpt_Stage.apk')}\"");
                    break;
            }
        }

        public void InstallBuild(BuildType buildType)
        {
            switch (buildType)
            {
                case BuildType.Qa:
                    CmdShell.Execute("adb install d:\\QVC_US_TabletOpt_QA.apk");
                    break;
                case BuildType.Stage:
                    CmdShell.Execute("adb install d:\\QVC_US_TabletOpt_Stage.apk");
                    break;
                default:
                    return;
            }
        }

        public void UninstallBuild()
        {
            try
            {
                var builtType = GetCurrentPackage();
                switch (builtType)
                {
                    case BuildType.Qa:
                        CmdShell.Execute("adb uninstall com.qvc.tabletopt.qa");
                        break;
                    case BuildType.Stage:
                        CmdShell.Execute("adb uninstall com.qvc.tabletopt.stage");
                        break;  
                }
            }
            catch (MultiplePackagesException)
            {
                CmdShell.Execute("adb uninstall com.qvc.tabletopt.qa");
                Thread.Sleep(5000);
                CmdShell.Execute("adb uninstall com.qvc.tabletopt.stage");
            }
        }

        public BuildType GetCurrentPackage()
        {
            const string args = "adb shell \"pm list packages | grep com.qvc.tabletopt\"";

            string strOutput = CmdShell.Execute(args);

            string packageName = GetClearPackageName(strOutput);

            switch (packageName)
            {
                case "com.qvc.tabletopt.qa":
                    return BuildType.Qa;
                case "com.qvc.tabletopt.stage":
                    return BuildType.Stage;
                default:
                    throw new MultiplePackagesException();
            }
        }

        public int GetDevicesCount()
        {
            string adbGetDevices = "adb devices";
            var output = CmdShell.Execute(adbGetDevices);

            string searched = "device";
            return GetCountOfStringMatches(output, searched) - 1;
        }

        public List<string> GetDevicesNameList()
        {
            return null;
        }

        private string GetClearPackageName(string input)
        {
            string pattern = "package:";
            int packagesCount = GetCountOfStringMatches(input, pattern);

            if (packagesCount > 1)
            {
                throw new MultipleDeviceException("Multiple device mode exception.");
            }

            else if(packagesCount == 0)
            {
                throw new NoPackagesInstalledException("Any builds are not installed yet.");
            }

            int ix = input.IndexOf(pattern, StringComparison.Ordinal);

            if (ix != -1)
            {
                string code = input.Substring(ix + pattern.Length);
                string resultString = Regex.Replace(code, @"(?:(?:\r?\n)+ +){2,}", @"\n");
                resultString = resultString.Trim();
                return resultString;
            }
            return String.Empty;
        }

        private int GetCountOfStringMatches(string source, string pattern)
        {
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matchCollection = regex.Matches(source);
            return matchCollection.Count;
        }

        
    }
}
