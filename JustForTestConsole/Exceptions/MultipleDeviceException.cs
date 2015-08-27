using System;

namespace JustForTestConsole.Exceptions
{
    public class MultipleDeviceException: Exception
    {
        public MultipleDeviceException()
        {
            
        }

        public MultipleDeviceException(string message): base (message)
        {

        }
    }
}
