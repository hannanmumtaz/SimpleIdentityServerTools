using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace SimpleIdentityServer.HierarchicalResource.EF.Sqlite
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHierarchicalResourceSqliteEF(this IServiceCollection serviceCollection, string connectionString)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            serviceCollection.AddHierarchicalResourceRepositories();
            serviceCollection.AddEntityFrameworkSqlite()
                .AddDbContext<HierarchicalResourceDbContext>(options =>
                    options.UseSqlite(connectionString));
            return serviceCollection;
        }
    }
}
