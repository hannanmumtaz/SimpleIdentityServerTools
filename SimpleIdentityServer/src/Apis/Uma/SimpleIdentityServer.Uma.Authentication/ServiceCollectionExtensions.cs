using Microsoft.Extensions.DependencyInjection;
using System;

namespace SimpleIdentityServer.Uma.Authentication
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUmaFilter(this IServiceCollection services, UmaFilterOptions options)
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
            services.AddTransient<UmaFilter>();
            return services;
        }
    }
}
