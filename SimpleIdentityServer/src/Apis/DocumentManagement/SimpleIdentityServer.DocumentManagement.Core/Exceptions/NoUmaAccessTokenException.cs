using System;

namespace SimpleIdentityServer.DocumentManagement.Core.Exceptions
{
    public class NoUmaAccessTokenException : NotAuthorizedException
    {
        public NoUmaAccessTokenException(string umaResourceId, string wellKnownConfiguration)
        {
            UmaResourceId = umaResourceId;
            WellKnownConfiguration = wellKnownConfiguration;
        }

        public string UmaResourceId { get; private set; }
        public string WellKnownConfiguration { get; private set; }
    }
}
