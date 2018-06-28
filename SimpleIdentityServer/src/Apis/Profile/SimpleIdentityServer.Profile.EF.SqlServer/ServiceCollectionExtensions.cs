using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace SimpleIdentityServer.Profile.EF.SqlServer
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProfileSqlServerEF(this IServiceCollection serviceCollection, string connectionString)
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
            serviceCollection.AddEntityFrameworkSqlServer()
                .AddDbContext<ProfileDbContext>(options =>
                    options.UseSqlServer(connectionString));
            return serviceCollection;
        }
    }
}
