using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;

namespace SimpleIdentityServer.Connectors.Common
{
    public interface IConnector
    {
        void Configure(AuthenticationBuilder authBuilder, IDictionary<string, string> options);
        IEnumerable<string> GetParameters();
    }
}