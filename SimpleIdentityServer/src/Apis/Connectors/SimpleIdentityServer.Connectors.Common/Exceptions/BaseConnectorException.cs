using System;

namespace SimpleIdentityServer.Connectors.Common.Exceptions
{
    public class BaseConnectorException : Exception
    {
        public BaseConnectorException(string message) : base(message) { }
    }
}
