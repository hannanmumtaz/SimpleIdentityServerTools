using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace SimpleIdentityServer.Connectors.Common
{
    public interface IConnector
    {
        void Configure(IServiceCollection services, IDictionary<string, string> options);
        IEnumerable<string> GetParameters();
    }
}