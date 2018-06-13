using SimpleIdentityServer.Module.Feed.Core.Models;
using SimpleIdentityServer.Module.Feed.Core.Repositories;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Module.Feed.Core.Connectors.Actions
{
    public interface IGetConnectorAction
    {
        Task<ConnectorAggregate> Execute(string name);
    }

    internal sealed class GetConnectorAction : IGetConnectorAction
    {
        private readonly IConnectorRepository _connectorRepository;

        public GetConnectorAction(IConnectorRepository connectorRepository)
        {
            _connectorRepository = connectorRepository;
        }

        public Task<ConnectorAggregate> Execute(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return _connectorRepository.Get(name);
        }
    }
}
