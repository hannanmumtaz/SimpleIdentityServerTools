using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.HierarchicalResource.Core.Repositories;
using SimpleIdentityServer.HierarchicalResource.EF.Repositories;
using System;

namespace SimpleIdentityServer.HierarchicalResource.EF
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHierarchicalResourceRepositories(this IServiceCollection serviceCollection)
        {
			if (serviceCollection == null) 
			{
				throw new ArgumentNullException(nameof(serviceCollection));
			}
			
            serviceCollection.AddTransient<IAssetRepository, AssetRepository>();
			return serviceCollection;
        }
    }
}
