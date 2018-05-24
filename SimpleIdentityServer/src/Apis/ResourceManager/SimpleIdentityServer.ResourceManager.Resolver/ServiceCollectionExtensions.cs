using Microsoft.Extensions.DependencyInjection;
using System;

namespace SimpleIdentityServer.ResourceManager.Resolver
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddResourceManagerResolver(this IServiceCollection services, ResourceManagerResolverOptions options)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            services.AddSingleton(options);
            services.AddTransient<IResourceManagerResolver, ResourceManagerResolver>();
            return services;
        }
    }
}