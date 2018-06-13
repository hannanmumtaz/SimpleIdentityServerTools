using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Parameter.Core.Helpers;
using SimpleIdentityServer.Parameter.Core.Parameters;
using SimpleIdentityServer.Parameter.Core.Parameters.Actions;
using System;

namespace SimpleIdentityServer.Parameter.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddParameterCore(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<IParameterActions, ParameterActions>();
            services.AddTransient<IGetModulesAction, GetModulesAction>();
            services.AddTransient<IUpdateModuleConfigurationAction, UpdateModuleConfigurationAction>();
            services.AddTransient<IDirectoryHelper, DirectoryHelper>();
            services.AddTransient<IGetConnectorsAction, GetConnectorsAction>();
            services.AddTransient<IUpdateConnectorsAction, UpdateConnectorsAction>();
            return services;
        }
    }
}
