using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.Module.Feed.Client.Projects;

namespace SimpleIdentityServer.Module.Feed.Client
{
    public interface IModuleFeedClientFactory
    {
        IModuleFeedClient BuildModuleFeedClient();
    }

    internal class ModuleFeedClientFactory : IModuleFeedClientFactory
    {
        private readonly ServiceProvider _serviceProvider;

        public ModuleFeedClientFactory()
        {
            var services = BuildServiceCollection();
            _serviceProvider = services.BuildServiceProvider();
        }

        public IModuleFeedClient BuildModuleFeedClient()
        {
            return _serviceProvider.GetService<IModuleFeedClient>();
        }
        
        private static IServiceCollection BuildServiceCollection()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<IModuleFeedClient, ModuleFeedClient>();
            serviceCollection.AddTransient<IProjectClient, ProjectClient>();
            serviceCollection.AddTransient<IConfigurationClient, ConfigurationClient>();
            serviceCollection.AddTransient<IDownloadProjectConfiguration, DownloadProjectConfiguration>();
            serviceCollection.AddTransient<IGetProjectOperation, GetProjectOperation>();
            serviceCollection.AddCommonClient();
            return serviceCollection;
        }
    }
}
