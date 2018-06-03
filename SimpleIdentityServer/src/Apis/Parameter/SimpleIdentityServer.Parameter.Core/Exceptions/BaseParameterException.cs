using System;

namespace SimpleIdentityServer.Parameter.Core.Exceptions
{
    public class BaseParameterException : Exception
    {
        public BaseParameterException()
        {

        }

        public BaseParameterException(string message) : base(message)
        {

        }
    }
}
