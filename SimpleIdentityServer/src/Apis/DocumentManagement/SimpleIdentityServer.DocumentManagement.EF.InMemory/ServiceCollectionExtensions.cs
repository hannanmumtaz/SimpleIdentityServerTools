using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace SimpleIdentityServer.DocumentManagement.EF.InMemory
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDocumentManagementEFInMemory(this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            serviceCollection.AddDocumentManagementRepository();
            serviceCollection.AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<DocumentManagementDbContext>(options => options.UseInMemoryDatabase().ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning)));
            return serviceCollection;
        }
    }
}
