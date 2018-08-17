﻿using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.DocumentManagement.Client.Jwks;
using SimpleIdentityServer.DocumentManagement.Client.OfficeDocuments;

namespace SimpleIdentityServer.DocumentManagement.Client
{
    public interface IDocumentManagementFactory
    {
        IOfficeDocumentClient GetOfficeDocumentClient();
        IJwksClient GetJwksClient();
    }

    public sealed class DocumentManagementFactory : IDocumentManagementFactory
    {
        private readonly ServiceProvider _servicesProvider;

        public DocumentManagementFactory()
        {
            var services = BuildServiceCollection();
            _servicesProvider =  services.BuildServiceProvider();
        }

        public DocumentManagementFactory(IHttpClientFactory httpClientFactory)
        {
            var services = BuildServiceCollection(httpClientFactory);
            _servicesProvider = services.BuildServiceProvider();
        }

        public IOfficeDocumentClient GetOfficeDocumentClient()
        {
            return _servicesProvider.GetService<IOfficeDocumentClient>();
        }

        public IJwksClient GetJwksClient()
        {
            return _servicesProvider.GetService<IJwksClient>();
        }

        private IServiceCollection BuildServiceCollection(IHttpClientFactory httpClientFactory = null)
        {
            var serviceCollection = new ServiceCollection();
            if (httpClientFactory != null)
            {
                serviceCollection.AddSingleton(httpClientFactory);
            }
            else
            {
                serviceCollection.AddCommonClient();
            }

            serviceCollection.AddTransient<IOfficeDocumentClient, OfficeDocumentClient>();
            serviceCollection.AddTransient<IUpdateOfficeDocumentOperation, UpdateOfficeDocumentOperation>();
            serviceCollection.AddTransient<IGetOfficeDocumentOperation, GetOfficeDocumentOperation>();
            serviceCollection.AddTransient<IAddOfficeDocumentOperation, AddOfficeDocumentOperation>();
            serviceCollection.AddTransient<IDecryptOfficeDocumentOperation, DecryptOfficeDocumentOperation>();
            serviceCollection.AddTransient<IGetPermissionsOperation, GetPermissionsOperation>();
            serviceCollection.AddTransient<IJwksClient, JwksClient>();
            serviceCollection.AddTransient<IGetJwksOperation, GetJwksOperation>();
            serviceCollection.AddTransient<IGetConfigurationOperation, GetConfigurationOperation>();
            return serviceCollection;
        }
    }
}
