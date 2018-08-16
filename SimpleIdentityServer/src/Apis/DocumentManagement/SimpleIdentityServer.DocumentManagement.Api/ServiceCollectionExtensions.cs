using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.DocumentManagement.Api.Extensions;
using SimpleIdentityServer.DocumentManagement.Core.Extensions;
using System;

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

            services.AddSingleton(options);
            services.AddDocumentManagementCore();
            return services;
        }
    }
}
