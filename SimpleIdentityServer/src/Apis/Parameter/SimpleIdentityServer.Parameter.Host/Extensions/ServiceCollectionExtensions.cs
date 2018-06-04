using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Parameter.Core;
using System;

namespace SimpleIdentityServer.Parameter.Host.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddParameterHost(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(ISupportRequiredService));
            }

            services.AddParameterCore();
            return services;
        }
    }
}
