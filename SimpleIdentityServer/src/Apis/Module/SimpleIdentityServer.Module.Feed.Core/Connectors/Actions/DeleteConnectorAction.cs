using SimpleIdentityServer.Module.Feed.Core.Exceptions;
using SimpleIdentityServer.Module.Feed.Core.Repositories;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Module.Feed.Core.Connectors.Actions
{
    public interface IDeleteConnectorAction
    {
        Task<bool> Execute(string name);
    }

    internal sealed class DeleteConnectorAction : IDeleteConnectorAction
    {
        private readonly IConnectorRepository _connectorRepository;

        public DeleteConnectorAction(IConnectorRepository connectorRepository)
        {
            _connectorRepository = connectorRepository;
        }

        public async Task<bool> Execute(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var record = await _connectorRepository.Get(name);
            if (record == null)
            {
                throw new NoConnectorException();
            }

            return await _connectorRepository.Delete(new[] { name });
        }
    }
}
