using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace SimpleIdentityServer.Module.Feed.EF.InMemory
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddModuleInMemoryEF(this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            serviceCollection.AddModuleFeedRepository();
            serviceCollection.AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<ModuleFeedDbContext>(options => options.UseInMemoryDatabase().ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning)));
            return serviceCollection;
        }
    }
}
