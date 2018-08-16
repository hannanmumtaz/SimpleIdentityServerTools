using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.DocumentManagement.Core.Jwks;
using SimpleIdentityServer.DocumentManagement.Core.Jwks.Actions;
using SimpleIdentityServer.DocumentManagement.Core.OfficeDocuments;
using SimpleIdentityServer.DocumentManagement.Core.OfficeDocuments.Actions;
using System;

namespace SimpleIdentityServer.DocumentManagement.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDocumentManagementCore(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<IOfficeDocumentActions, OfficeDocumentActions>();
            services.AddTransient<IAddOfficeDocumentAction, AddOfficeDocumentAction>();
            services.AddTransient<IGetOfficeDocumentAction, GetOfficeDocumentAction>();
            services.AddTransient<IUpdateOfficeDocumentAction, UpdateOfficeDocumentAction>();
            services.AddTransient<IDecryptOfficeDocumentAction, DecryptOfficeDocumentAction>();
            services.AddTransient<IJwksActions, JwksActions>();
            services.AddTransient<IGetJwksAction, GetJwksAction>();
            services.AddTransient<IJsonWebKeyEnricher, JsonWebKeyEnricher>();
            return services;
        }
    }
}
