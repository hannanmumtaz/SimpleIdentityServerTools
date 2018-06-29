using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace SimpleIdentityServer.HierarchicalResource.EF.Postgre
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHierarchicalResourcePostgreEF(this IServiceCollection serviceCollection, string connectionString)
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
            serviceCollection.AddEntityFrameworkNpgsql()
                .AddDbContext<HierarchicalResourceDbContext>(options =>
                    options.UseNpgsql(connectionString));
            return serviceCollection;
        }
    }
}
