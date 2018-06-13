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

            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("get", policy =>
                {
                    policy.RequireAssertion(a =>
                    {
                        var user = a.User;
                        return true;
                    });
                    // policy.RequireClaim("scope", "get_parameters");
                });
                opts.AddPolicy("add", policy => policy.RequireClaim("scope", "add_parameters"));
            });
            services.AddParameterCore();
            return services;
        }
    }
}
