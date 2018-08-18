using Microsoft.Extensions.DependencyInjection;
using System;

namespace SimpleIdentityServer.DocumentManagement.Store.InMemory
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDocumentManagementInMemoryStore(this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            serviceCollection.AddSingleton(typeof(IOfficeDocumentConfirmationLinkStore), typeof(OfficeDocumentConfirmationLinkStore));
            return serviceCollection;
        }
    }
}
