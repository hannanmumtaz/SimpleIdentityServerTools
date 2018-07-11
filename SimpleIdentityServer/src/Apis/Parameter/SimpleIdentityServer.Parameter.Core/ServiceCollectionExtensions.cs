using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Parameter.Core.Common;
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
            services.AddTransient<IGetUnitsAction, GetUnitsAction>();
            services.AddTransient<IGetTwoFactorsAction, GetTwoFactorsAction>();
            services.AddTransient<IUpdateUnitsAction, UpdateUnitsAction>();
            services.AddTransient<IUpdateConnectorsAction, UpdateConnectorsAction>();
            services.AddTransient<IUpdateTwoFactorsAction, UpdateTwoFactorsAction>();
            services.AddTransient<IDirectoryHelper, DirectoryHelper>();
            services.AddTransient<IGetConnectorsAction, GetConnectorsAction>();
            services.AddTransient<IGetProjectConfiguration, GetProjectConfiguration>();
            return services;
        }
    }
}
