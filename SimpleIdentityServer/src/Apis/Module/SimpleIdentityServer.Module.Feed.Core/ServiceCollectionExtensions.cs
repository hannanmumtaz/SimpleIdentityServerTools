using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Module.Feed.Core.Connectors;
using SimpleIdentityServer.Module.Feed.Core.Connectors.Actions;
using SimpleIdentityServer.Module.Feed.Core.Projects;
using SimpleIdentityServer.Module.Feed.Core.Projects.Actions;
using System;

namespace SimpleIdentityServer.Module.Feed.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddModuleFeedCore(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<IProjectActions, ProjectActions>();
            services.AddTransient<IGetProjectAction, GetProjectAction>();
            services.AddTransient<IGetAllProjectsAction, GetAllProjectsAction>();
            services.AddTransient<IConnectorActions, ConnectorActions>();
            services.AddTransient<IAddConnectorAction, AddConnectorAction>();
            services.AddTransient<IDeleteConnectorAction, DeleteConnectorAction>();
            services.AddTransient<IGetAllConnectorsAction, GetAllConnectorsAction>();
            services.AddTransient<IGetConnectorAction, GetConnectorAction>();
            return services;
        }
    }
}
