using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.DocumentManagement.Core.Repositories;
using SimpleIdentityServer.DocumentManagement.EF.Repositories;
using System;

namespace SimpleIdentityServer.DocumentManagement.EF
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDocumentManagementRepository(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<IOfficeDocumentRepository, OfficeDocumentRepository>();
            return services;
        }
    }
}
