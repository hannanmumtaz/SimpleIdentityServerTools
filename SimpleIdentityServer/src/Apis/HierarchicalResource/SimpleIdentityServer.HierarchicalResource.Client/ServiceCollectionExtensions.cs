using Microsoft.Extensions.DependencyInjection;
using System;

namespace SimpleIdentityServer.HierarchicalResource.Client
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddResourceManagerClient(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<IHierarchicalResourceClientFactory, HierarchicalResourceClientFactory>();
            return services;
        }
    }
}
