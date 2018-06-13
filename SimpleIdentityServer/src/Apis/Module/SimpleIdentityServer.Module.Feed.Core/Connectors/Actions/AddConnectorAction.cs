using SimpleIdentityServer.Module.Feed.Core.Exceptions;
using SimpleIdentityServer.Module.Feed.Core.Models;
using SimpleIdentityServer.Module.Feed.Core.Repositories;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Module.Feed.Core.Connectors.Actions
{
    public interface IAddConnectorAction
    {
        Task<bool> Execute(ConnectorAggregate connector);
    }

    internal sealed class AddConnectorAction : IAddConnectorAction
    {
        private readonly IConnectorRepository _connectorRepository;

        public AddConnectorAction(IConnectorRepository connectorRepository)
        {
            _connectorRepository = connectorRepository;
        }

        public async Task<bool> Execute(ConnectorAggregate connector)
        {
            Check(connector);
            var record = await _connectorRepository.Get(connector.Name);
            if (record != null)
            {
                throw new ModuleFeedInternalException("internal", "connector_already_exists");
            }

            return await _connectorRepository.Add(new[] { connector });
        }

        private void Check(ConnectorAggregate connector)
        {
            if (connector == null)
            {
                throw new ArgumentNullException(nameof(connector));
            }

            if (string.IsNullOrWhiteSpace(connector.Name))
            {
                throw new ArgumentNullException(nameof(connector.Name));
            }

            if (string.IsNullOrWhiteSpace(connector.Library))
            {
                throw new ArgumentNullException(nameof(connector.Library));
            }

            if (string.IsNullOrWhiteSpace(connector.Version))
            {
                throw new ArgumentNullException(nameof(connector.Version));
            }
        }
    }
}
