using SimpleIdentityServer.Module.Feed.Core.Models;
using SimpleIdentityServer.Module.Feed.Core.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Module.Feed.Core.Connectors.Actions
{
    public interface IGetAllConnectorsAction
    {
        Task<IEnumerable<ConnectorAggregate>> Execute();
    }

    internal sealed class GetAllConnectorsAction : IGetAllConnectorsAction
    {
        private readonly IConnectorRepository _connectorRepository;

        public GetAllConnectorsAction(IConnectorRepository connectorRepository)
        {
            _connectorRepository = connectorRepository;
        }

        public Task<IEnumerable<ConnectorAggregate>> Execute()
        {
            return _connectorRepository.GetAll();
        }
    }
}
