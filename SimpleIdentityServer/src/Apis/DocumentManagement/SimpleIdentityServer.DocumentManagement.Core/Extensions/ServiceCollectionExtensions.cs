using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.DocumentManagement.Core.Jwks;
using SimpleIdentityServer.DocumentManagement.Core.Jwks.Actions;
using SimpleIdentityServer.DocumentManagement.Core.OfficeDocuments;
using SimpleIdentityServer.DocumentManagement.Core.OfficeDocuments.Actions;
using SimpleIdentityServer.DocumentManagement.Core.Validators;
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
            services.AddTransient<IGenerateConfirmationLinkAction, GenerateConfirmationLinkAction>();
            services.AddTransient<IValidateConfirmationLinkAction, ValidateConfirmationLinkAction>();
            services.AddTransient<IGetOfficeDocumentPermissionsAction, GetOfficeDocumentPermissionsAction>();
            services.AddTransient<IGetAllConfirmationLinksAction, GetAllConfirmationLinksAction>();
            services.AddTransient<IJwksActions, JwksActions>();
            services.AddTransient<IGetJwksAction, GetJwksAction>();
            services.AddTransient<IJsonWebKeyEnricher, JsonWebKeyEnricher>();
            services.AddTransient<IGenerateConfirmationLinkParameterValidator, GenerateConfirmationLinkParameterValidator>();
            services.AddTransient<IValidateConfirmationLinkParameterValidator, ValidateConfirmationLinkParameterValidator>();
            services.AddTransient<IAddDocumentParameterValidator, AddDocumentParameterValidator>();
            services.AddTransient<IUpdateOfficeDocumentParameterValidator, UpdateOfficeDocumentParameterValidator>();
            services.AddTransient<IGetAllConfirmationLinksValidator, GetAllConfirmationLinksValidator>();
            services.AddTransient<IDecryptOfficeDocumentParameterValidator, DecryptOfficeDocumentParameterValidator>();
            return services;
        }
    }
}
