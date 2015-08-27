using System;

namespace JustForTestConsole.Exceptions
{
    public class NoPackagesInstalledException: Exception
    {
        public NoPackagesInstalledException()
        {
            
        }

        public NoPackagesInstalledException(string message): base(message)
        {

        }
    }
}
