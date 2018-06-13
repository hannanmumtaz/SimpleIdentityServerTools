using Microsoft.Extensions.DependencyInjection;
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
            return services;
        }
    }
}
