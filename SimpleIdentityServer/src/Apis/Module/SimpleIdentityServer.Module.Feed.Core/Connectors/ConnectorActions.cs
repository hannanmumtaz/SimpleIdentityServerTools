using SimpleIdentityServer.Module.Feed.Core.Connectors.Actions;
using SimpleIdentityServer.Module.Feed.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Module.Feed.Core.Connectors
{
    public interface IConnectorActions
    {
        Task<bool> Add(ConnectorAggregate connector);
        Task<bool> Delete(string name);
        Task<IEnumerable<ConnectorAggregate>> GetAll();
        Task<ConnectorAggregate> Get(string name);
    }

    internal sealed class ConnectorActions : IConnectorActions
    {
        private readonly IAddConnectorAction _addConnectorAction;
        private readonly IDeleteConnectorAction _deleteConnectorAction;
        private readonly IGetAllConnectorsAction _getAllConnectorsAction;
        private readonly IGetConnectorAction _getConnectorAction;

        public ConnectorActions(IAddConnectorAction addConnectorAction, IDeleteConnectorAction deleteConnectorAction, IGetAllConnectorsAction getAllConnectorsAction, IGetConnectorAction getConnectorAction)
        {
            _addConnectorAction = addConnectorAction;
            _deleteConnectorAction = deleteConnectorAction;
            _getAllConnectorsAction = getAllConnectorsAction;
            _getConnectorAction = getConnectorAction;
        }

        public Task<bool> Add(ConnectorAggregate connector)
        {
            return _addConnectorAction.Execute(connector);
        }

        public Task<bool> Delete(string name)
        {
            return _deleteConnectorAction.Execute(name);
        }

        public Task<IEnumerable<ConnectorAggregate>> GetAll()
        {
            return _getAllConnectorsAction.Execute();
        }

        public Task<ConnectorAggregate> Get(string name)
        {
            return _getConnectorAction.Execute(name);
        }
    }
}
