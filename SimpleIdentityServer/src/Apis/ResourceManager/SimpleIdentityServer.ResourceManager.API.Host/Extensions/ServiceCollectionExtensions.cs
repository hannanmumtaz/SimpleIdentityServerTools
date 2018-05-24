using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.ResourceManager.Core;
using System;

namespace SimpleIdentityServer.ResourceManager.API.Host.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddResourceManagerHost(this IServiceCollection services, ResourceManagerHostOptions options)
        {
            if (services== null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            services.AddResourceManager();
            services.AddSingleton(options);
            return services;
        }
    }
}
