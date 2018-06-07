using Microsoft.Extensions.DependencyInjection;
using System;

namespace SimpleIdentityServer.DocumentManagement.Api
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDocumentManagementHost(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            return services;
        }
    }
}
