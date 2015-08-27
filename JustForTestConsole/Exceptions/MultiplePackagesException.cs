using System;

namespace JustForTestConsole.Exceptions
{
    public class MultiplePackagesException : Exception
    {
        public MultiplePackagesException()
        {
        }

        public MultiplePackagesException(string message) : base(message)
        {
        }
    }
}
