using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.DocumentManagement.Api.Extensions;
using SimpleIdentityServer.DocumentManagement.Core.Extensions;
using System;
using SimpleIdentityServer.License;
using SimpleIdentityServer.License.Exceptions;

namespace SimpleIdentityServer.DocumentManagement.Api
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDocumentManagementHost(this IServiceCollection services, DocumentManagementApiOptions options)
        {
            if (services == null)
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
			
            services.AddSingleton(options);
            services.AddDocumentManagementCore();
            return services;
        }
    }
}
