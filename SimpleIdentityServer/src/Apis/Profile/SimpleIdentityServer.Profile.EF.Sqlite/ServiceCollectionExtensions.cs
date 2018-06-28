using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace SimpleIdentityServer.Profile.EF.Sqlite
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProfileSqliteEF(this IServiceCollection serviceCollection, string connectionString)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            serviceCollection.AddProfileRepositories();
            serviceCollection.AddEntityFrameworkSqlite()
                .AddDbContext<ProfileDbContext>(options =>
                    options.UseSqlite(connectionString));
            return serviceCollection;
        }
    }
}
