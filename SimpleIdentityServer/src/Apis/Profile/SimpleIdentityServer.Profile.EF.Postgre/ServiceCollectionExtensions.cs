using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace SimpleIdentityServer.Profile.EF.Postgre
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProfilePostgreEF(this IServiceCollection serviceCollection, string connectionString)
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
            serviceCollection.AddEntityFrameworkNpgsql()
                .AddDbContext<ProfileDbContext>(options =>
                    options.UseNpgsql(connectionString));
            return serviceCollection;
        }
    }
}
