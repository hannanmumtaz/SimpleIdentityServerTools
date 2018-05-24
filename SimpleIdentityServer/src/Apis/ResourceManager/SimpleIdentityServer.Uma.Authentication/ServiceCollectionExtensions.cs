using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.Core.Jwt;
using SimpleIdentityServer.ResourceManager.Client;
using SimpleIdentityServer.Uma.Client;
using System;

namespace SimpleIdentityServer.Uma.Authentication
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUmaAuthenticationFilter(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddIdServerClient();
            services.AddUmaClient();
            services.AddResourceManagerClient();
            services.AddDataProtection();
            services.AddSimpleIdentityServerJwt();
            return services;
        }
    }
}
