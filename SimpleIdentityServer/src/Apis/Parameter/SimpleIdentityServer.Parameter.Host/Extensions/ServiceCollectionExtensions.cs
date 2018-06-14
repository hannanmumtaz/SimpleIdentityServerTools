using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Parameter.Core;
using System;

namespace SimpleIdentityServer.Parameter.Host.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddParameterHost(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(ISupportRequiredService));
            }

            services.AddParameterCore();
            return services;
        }

        public static AuthorizationOptions AddParameterAuthPolicy(this AuthorizationOptions opts)
        {
            if (opts == null)
            {
                throw new ArgumentNullException(nameof(opts));
            }

            const string authScheme = "OAuth2Introspection";
            opts.AddPolicy("get", policy =>
            {
                policy.AuthenticationSchemes.Add(authScheme);
                policy.RequireClaim("scope", "get_parameters");
            });
            opts.AddPolicy("add", policy =>
            {
                policy.AuthenticationSchemes.Add(authScheme);
                policy.RequireClaim("scope", "add_parameters");
            });
            return opts;
        }
    }
}
