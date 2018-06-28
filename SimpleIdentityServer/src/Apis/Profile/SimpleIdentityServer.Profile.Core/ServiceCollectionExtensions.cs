using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Profile.Core.Api.Profile;
using SimpleIdentityServer.Profile.Core.Api.Profile.Actions;
using System;

namespace SimpleIdentityServer.Profile.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProfileCore(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            
            services.AddTransient<IProfileActions, ProfileActions>();
            services.AddTransient<IGetProfileAction, GetProfileAction>();
            services.AddTransient<IUpdateProfileAction, UpdateProfileAction>();
            return services;
        }
    }
}
