using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustForTestConsole.Interfaces
{
    public interface IAdbShell
    {
        string InstallApk(string path, string deviceId);
        string UninstallApk(string package, string deviceId);
        string ForceStopApp(string package, string deviceId);
        string ClearDataApp(string package, string deviceId);
        string RunApp(string package, string deviceId, string activity);

        List<string> GetDevices();

        List<string> GetAllPackagesList(string deviceId);
        List<string> GetPackagesList(string filter, string deviceId);


    }
}
