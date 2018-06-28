using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace SimpleIdentityServer.Profile.EF.InMemory
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProfileInMemoryEF(this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            serviceCollection.AddProfileRepositories();
            serviceCollection.AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<ProfileDbContext>(options => options.UseInMemoryDatabase().ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning)));
            return serviceCollection;
        }
    }
}
