using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace SimpleIdentityServer.HierarchicalResource.EF.SqlServer
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHierarchicalResourceSqlServerEF(this IServiceCollection serviceCollection, string connectionString)
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
            serviceCollection.AddEntityFrameworkSqlServer()
                .AddDbContext<HierarchicalResourceDbContext>(options =>
                    options.UseSqlServer(connectionString));
            return serviceCollection;
        }
    }
}
