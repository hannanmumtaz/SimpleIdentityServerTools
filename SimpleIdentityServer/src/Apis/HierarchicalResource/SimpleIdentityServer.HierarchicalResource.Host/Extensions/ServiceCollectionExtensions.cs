using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.HierarchicalResource.Core;
using System;

namespace SimpleIdentityServer.HierarchicalResource.Host.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHierarchicalResourceHost(this IServiceCollection services, HierarchicalResourceOptions options)
        {
            if (services== null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            services.AddHierarchicalResourceCore();
            services.AddSingleton(options);
            return services;
        }
    }
}
