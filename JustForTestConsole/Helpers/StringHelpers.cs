using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace JustForTestConsole.Helpers
{
    public static class StringHelpers
    {
        public static List<string> GetPackagesList(string cmdOutput)
        {
            List<string> packages = new List<string>();
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
            List<string> devices = new List<string>();
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
