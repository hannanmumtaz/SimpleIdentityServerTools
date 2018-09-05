using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using SimpleIdentityServer.Authenticate.Eid.Controllers;
using SimpleIdentityServer.Authenticate.Eid.Core;
using SimpleIdentityServer.Authenticate.Eid.Services;
using SimpleIdentityServer.Core.Services;
using System;

namespace SimpleIdentityServer.Authenticate.Eid
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEidAuthentication(this IServiceCollection services, IMvcBuilder mvcBuilder, IHostingEnvironment hosting, EidAuthenticateOptions eidOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (mvcBuilder == null)
            {
                throw new ArgumentNullException(nameof(mvcBuilder));
            }

            if (hosting == null)
            {
                throw new ArgumentNullException(nameof(hosting));
            }

            if (eidOptions == null)
            {
                throw new ArgumentNullException(nameof(eidOptions));
            }

            var assembly = typeof(AuthenticateController).Assembly;
            var embeddedFileProvider = new EmbeddedFileProvider(assembly);
            var compositeProvider = new CompositeFileProvider(hosting.ContentRootFileProvider, embeddedFileProvider);
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.FileProviders.Add(compositeProvider);
            });

            services.AddSingleton(eidOptions);
            services.AddEidOpenidCore();
            services.AddTransient<IAuthenticateResourceOwnerService, EidAuthenticateResourceOwnerService>();
            services.AddMvc().AddApplicationPart(assembly);
            return services;
        }
    }
}
