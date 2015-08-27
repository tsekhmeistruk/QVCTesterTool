using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JustForTestConsole.Abstracts;
using JustForTestConsole.Enums;
using JustForTestConsole.Interfaces;

namespace JustForTestConsole.Builds
{
    public class StageBuild : BaseQvcBuild
    {
        public StageBuild(IAdbShell adbShell, Culture culture) : base(adbShell, culture)
        {
        }

        protected override void InstallBuild(string deviceId)
        {
            throw new NotImplementedException();
        }

        protected override void UnInstallBuild(string deviceId)
        {
            throw new NotImplementedException();
        }

        protected override void DownloadBuild()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
