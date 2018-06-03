using Microsoft.Extensions.DependencyInjection;
using System;

namespace SimpleIdentityServer.Configuration.Host.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfigurationHost(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(ISupportRequiredService));
            }

            return services;
        }
    }
}
