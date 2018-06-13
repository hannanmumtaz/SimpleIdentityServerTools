using SimpleIdentityServer.Module.Feed.Client.Projects;

namespace SimpleIdentityServer.Module.Feed.Client
{
    public interface IModuleFeedClient
    {
        IProjectClient GetProjectClient();
        IConfigurationClient GetConfigurationClient();
    }

    internal sealed class ModuleFeedClient : IModuleFeedClient
    {
        private readonly IProjectClient _projectClient;
        private readonly IConfigurationClient _configurationClient;

        public ModuleFeedClient(IProjectClient projectClient, IConfigurationClient configurationClient)
        {
            _projectClient = projectClient;
            _configurationClient = configurationClient;
        }

        public IProjectClient GetProjectClient()
        {
            return _projectClient;
        }

        public IConfigurationClient GetConfigurationClient()
        {
            return _configurationClient;
        }
    }
}
