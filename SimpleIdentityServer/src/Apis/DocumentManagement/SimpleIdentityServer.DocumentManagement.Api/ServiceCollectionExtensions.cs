using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.DocumentManagement.Api.Extensions;
using SimpleIdentityServer.DocumentManagement.Core.Extensions;
using SimpleIdentityServer.Uma.Client;
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

            services.AddIdServerClient();
            services.AddUmaClient();
            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("connected", policy => policy.RequireAuthenticatedUser());
            });
            services.AddSingleton(options.OAuth);
            services.AddDocumentManagementCore();
            return services;
        }
    }
}
