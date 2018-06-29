using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.HierarchicalResource.Client.Configuration;
using SimpleIdentityServer.HierarchicalResource.Client.HierarchicalResources;
using System;

namespace SimpleIdentityServer.HierarchicalResource.Client
{
    public interface IHierarchicalResourceClientFactory
    {
        IHierarchicalResourceClient GetHierarchicalResourceClient();
    }

    internal sealed class HierarchicalResourceClientFactory : IHierarchicalResourceClientFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public HierarchicalResourceClientFactory()
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
