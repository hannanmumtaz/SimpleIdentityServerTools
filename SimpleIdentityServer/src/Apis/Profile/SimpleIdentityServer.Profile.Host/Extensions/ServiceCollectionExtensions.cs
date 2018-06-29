using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.License;
using SimpleIdentityServer.License.Exceptions;
using SimpleIdentityServer.Profile.Core;
using System;

namespace SimpleIdentityServer.Profile.Host.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProfileHost(this IServiceCollection services)
        {
            if (services== null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var loader = new LicenseLoader();
            var license = loader.TryGetLicense();
            if (license.ExpirationDateTime < DateTime.UtcNow)
            {
                throw new LicenseExpiredException();
            }

            services.AddAuthorization(options =>
            {
                options.AddPolicy("my_profile", policy => policy.RequireAuthenticatedUser());
            });
            services.AddProfileCore();
            return services;
        }
    }
}
