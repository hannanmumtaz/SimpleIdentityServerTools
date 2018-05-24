using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.ResourceManager.Client.Configuration;
using SimpleIdentityServer.ResourceManager.Client.HierarchicalResources;
using System;

namespace SimpleIdentityServer.ResourceManager.Client
{
    public interface IResourceManagerClientFactory
    {
        IHierarchicalResourceClient GetHierarchicalResourceClient();
    }

    internal sealed class ResourceManagerClientFactory : IResourceManagerClientFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ResourceManagerClientFactory()
        {
            var services = new ServiceCollection();
            RegisterDependencies(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        public IHierarchicalResourceClient GetHierarchicalResourceClient()
        {
            var hierarchicalResourceClient = (IHierarchicalResourceClient)_serviceProvider.GetService(typeof(IHierarchicalResourceClient));
            return hierarchicalResourceClient;
        }

        private static void RegisterDependencies(IServiceCollection services)
        {
            services.AddTransient<IHierarchicalResourceClient, HierarchicalResourceClient>();
            services.AddTransient<IGetHierarchicalResourceOperation, GetHierarchicalResourceOperation>();
            services.AddTransient<IConfigurationClient, ConfigurationClient>();
            services.AddTransient<IGetConfigurationOperation, GetConfigurationOperation>();
            services.AddCommonClient();
        }
    }
}
