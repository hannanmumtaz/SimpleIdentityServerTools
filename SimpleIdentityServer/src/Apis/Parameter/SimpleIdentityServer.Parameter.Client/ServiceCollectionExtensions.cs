using Microsoft.Extensions.DependencyInjection;
using System;

namespace SimpleIdentityServer.Parameter.Client
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddParameterClient(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<IParameterClientFactory, ParameterClientFactory>();
            return services;
        }
    }
}
