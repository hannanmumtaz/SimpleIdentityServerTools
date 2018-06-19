using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using SimpleIdentityServer.Module.Feed.Core;
using SimpleIdentityServer.Module.Feed.Host.Controllers;
using System;

namespace SimpleIdentityServer.Module.Feed.Host.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddModuleFeedHost(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }


            var assembly = typeof(HomeController).Assembly;
            var embeddedFileProvider = new EmbeddedFileProvider(assembly);
            services.Configure<RazorViewEngineOptions>(opts =>
            {
                opts.FileProviders.Add(embeddedFileProvider);
            });
            services.AddModuleFeedCore();
            return services;
        }
    }
}