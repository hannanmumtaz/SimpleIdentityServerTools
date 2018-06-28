using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Parameter.Core;
using System;
using System.Linq;

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
                policy.RequireAssertion(p =>
                {
                    if (p.User == null || p.User.Identity == null || !p.User.Identity.IsAuthenticated)
                    {
                        return false;
                    }

                    var claimRole = p.User.Claims.FirstOrDefault(c => c.Type == "role");
                    var claimScope = p.User.Claims.FirstOrDefault(c => c.Type == "scope");
                    if (claimRole == null && claimScope == null)
                    {
                        return false;
                    }

                    return claimRole != null && claimRole.Value == "administrator" || claimScope != null && claimScope.Value == "get_parameters";
                });
            });
            opts.AddPolicy("add", policy =>
            {
                policy.AuthenticationSchemes.Add(authScheme);
                policy.RequireAssertion(p =>
                {
                    if (p.User == null || p.User.Identity == null || !p.User.Identity.IsAuthenticated)
                    {
                        return false;
                    }

                    var claimRole = p.User.Claims.FirstOrDefault(c => c.Type == "role");
                    var claimScope = p.User.Claims.FirstOrDefault(c => c.Type == "scope");
                    if (claimRole == null && claimScope == null)
                    {
                        return false;
                    }

                    return claimRole != null && claimRole.Value == "administrator" || claimScope != null && claimScope.Value == "add_parameters";
                });
            });
            return opts;
        }
    }
}
