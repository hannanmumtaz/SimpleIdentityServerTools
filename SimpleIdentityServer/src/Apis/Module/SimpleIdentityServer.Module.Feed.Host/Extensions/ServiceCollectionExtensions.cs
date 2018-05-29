using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Module.Feed.Core;
using System;

namespace SimpleIdentityServer.Module.Feed.Host.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddModuleFeedHost(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddModuleFeedCore();
            return services;
        }
    }
}
