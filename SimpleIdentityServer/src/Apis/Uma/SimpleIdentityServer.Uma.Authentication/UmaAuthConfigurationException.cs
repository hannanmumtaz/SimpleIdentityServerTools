using System;

namespace SimpleIdentityServer.Uma.Authentication
{
    internal class UmaAuthConfigurationException : Exception
    {
        public UmaAuthConfigurationException(string message) : base(message) { }
    }
}
