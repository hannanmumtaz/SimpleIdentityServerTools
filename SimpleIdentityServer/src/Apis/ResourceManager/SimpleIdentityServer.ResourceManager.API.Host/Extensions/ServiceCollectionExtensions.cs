using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.License;
using SimpleIdentityServer.License.Exceptions;
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

            var loader = new LicenseLoader();
            var license = loader.TryGetLicense();
            if (license.ExpirationDateTime < DateTime.UtcNow)
            {
                throw new LicenseExpiredException();
            }

            services.AddResourceManager();
            services.AddSingleton(options);
            return services;
        }
    }
}
