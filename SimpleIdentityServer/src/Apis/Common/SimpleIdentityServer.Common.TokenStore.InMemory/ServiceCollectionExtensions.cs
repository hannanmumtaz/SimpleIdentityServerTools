using Microsoft.Extensions.DependencyInjection;
using System;

namespace SimpleIdentityServer.Common.TokenStore.InMemory
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTokenStoreInMemory(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<ITokenStore, InMemoryTokenStore>();
            return services;
        }
    }
}
