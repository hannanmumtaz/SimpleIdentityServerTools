using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Profile.Core.Repositories;
using SimpleIdentityServer.Profile.EF.Repositories;
using System;

namespace SimpleIdentityServer.Profile.EF
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProfileRepositories(this IServiceCollection serviceCollection)
        {
			if (serviceCollection == null) 
			{
				throw new ArgumentNullException(nameof(serviceCollection));
			}

            serviceCollection.AddTransient<IEndpointRepository, EndpointRepository>();
            serviceCollection.AddTransient<IProfileRepository, ProfileRepository>();
            return serviceCollection;
        }
    }
}
