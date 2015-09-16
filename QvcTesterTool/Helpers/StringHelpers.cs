using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace QvcTesterTool.Helpers
{
    public static class StringHelpers
    {
        public static List<string> GetPackagesList(string cmdOutput)
        {
            var packages = new List<string>();
            string pattern = "(package:)(.*)$";
            Regex regex = new Regex(pattern, RegexOptions.Multiline | RegexOptions.Compiled);

            foreach (Match device in regex.Matches(cmdOutput))
            {
                var package = device.Groups[2].Value;
                packages.Add(Regex.Replace(package, "\r|\n|\t", ""));
            }
            return packages;
        }

        public static List<string> GetDevicesId(string cmdOutput)
        {
            var devices = new List<string>();
            string pattern = "(.*)(\\t)(device)\r\n";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

            foreach (Match device in regex.Matches(cmdOutput))
            {
                devices.Add(device.Groups[1].Value);
            }
            return devices;
        }
    }


}
