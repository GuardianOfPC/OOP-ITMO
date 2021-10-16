using System;

namespace Isu.Tools
{
    public class IsuException : Exception
    {
        public IsuException()
            : base("Isu is down")
        { }

        public IsuException(string message)
            : base(message) // base Exception class constructor
        { }

        public IsuException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}