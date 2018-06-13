using SimpleIdentityServer.Module.Feed.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Module.Feed.Core.Repositories
{
    public interface IConnectorRepository
    {
        Task<ConnectorAggregate> Get(string name);
        Task<bool> Add(IEnumerable<ConnectorAggregate> connectors);
        Task<bool> Delete(IEnumerable<string> names);
        Task<IEnumerable<ConnectorAggregate>> GetAll();
    }
}
