using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Module.Feed.Core.Repositories;
using SimpleIdentityServer.Module.Feed.EF.Repositories;
using System;

namespace SimpleIdentityServer.Module.Feed.EF
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddModuleFeedRepository(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<IProjectRepository, ProjectRepository>();
            services.AddTransient<IConnectorRepository, ConnectorRepository>();
            return services;
        }
    }
}
