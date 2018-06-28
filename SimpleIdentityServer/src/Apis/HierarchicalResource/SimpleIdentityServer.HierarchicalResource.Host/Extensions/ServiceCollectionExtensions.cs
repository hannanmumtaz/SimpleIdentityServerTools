using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.HierarchicalResource.Core;
using SimpleIdentityServer.License;
using SimpleIdentityServer.License.Exceptions;
using System;

namespace SimpleIdentityServer.HierarchicalResource.Host.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHierarchicalResourceHost(this IServiceCollection services, HierarchicalResourceOptions options)
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

            services.AddHierarchicalResourceCore();
            services.AddSingleton(options);
            return services;
        }
    }
}
