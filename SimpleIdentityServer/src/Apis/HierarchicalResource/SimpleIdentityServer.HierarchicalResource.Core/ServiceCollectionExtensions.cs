using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.HierarchicalResource.Core.Api.HierarchicalResources;
using SimpleIdentityServer.HierarchicalResource.Core.Api.HierarchicalResources.Actions;
using System;

namespace SimpleIdentityServer.HierarchicalResource.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHierarchicalResourceCore(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<IHierarchicalResourcesActions, HierarchicalResourcesActions>();
            services.AddTransient<IAddHierarchicalResourcesAction, AddHierarchicalResourcesAction>();
            services.AddTransient<IDeleteHierarchicalResourcesAction, DeleteHierarchicalResourcesAction>();
            services.AddTransient<IGetHierarchicalResourceAction, GetHierarchicalResourceAction>();
            services.AddTransient<ISearchHierarchicalResourcesAction, SearchHierarchicalResourcesAction>();
            return services;
        }
    }
}
