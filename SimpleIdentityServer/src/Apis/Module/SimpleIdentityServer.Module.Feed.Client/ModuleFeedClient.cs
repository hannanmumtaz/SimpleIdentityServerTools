using SimpleIdentityServer.Module.Feed.Client.Connectors;
using SimpleIdentityServer.Module.Feed.Client.Projects;

namespace SimpleIdentityServer.Module.Feed.Client
{
    public interface IModuleFeedClient
    {
        IProjectClient GetProjectClient();
        IConfigurationClient GetConfigurationClient();
        IConnectorClient GetConnectorClient();
    }

    internal sealed class ModuleFeedClient : IModuleFeedClient
    {
        private readonly IProjectClient _projectClient;
        private readonly IConfigurationClient _configurationClient;
        private readonly IConnectorClient _connectorClient;

        public ModuleFeedClient(IProjectClient projectClient, IConfigurationClient configurationClient,
            IConnectorClient connectorClient)
        {
            _projectClient = projectClient;
            _configurationClient = configurationClient;
            _connectorClient = connectorClient;
        }

        public IProjectClient GetProjectClient()
        {
            return _projectClient;
        }

        public IConfigurationClient GetConfigurationClient()
        {
            return _configurationClient;
        }

        public IConnectorClient GetConnectorClient()
        {
            return _connectorClient;
        }
    }
}
