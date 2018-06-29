using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.Core.Jwt;
using SimpleIdentityServer.HierarchicalResource.Client;
using SimpleIdentityServer.ResourceManager.Resolver;
using SimpleIdentityServer.Uma.Authentication;
using SimpleIdentityServer.Uma.Client;

namespace SimpleIdentityServer.ProtectedWebsite.Mvc
{
    public class Startup
    {
        private ResourceManagerResolverOptions _resourceManagerResolverOptions;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddMvc();
            services.AddIdServerClient();
            services.AddUmaClient();
            services.AddResourceManagerClient();
            services.AddDataProtection();
            services.AddSimpleIdentityServerJwt();
            _resourceManagerResolverOptions = new ResourceManagerResolverOptions
            {
                Authorization = new ResourceManagerResolverAuthorizationOptions
                {
                    AuthorizationWellKnownConfiguration = "http://localhost:60004/.well-known/uma2-configuration",
                    ClientId = "ProtectedWebsite",
                    ClientSecret = "ProtectedWebsite"
                },
                ResourceManagerUrl = "http://localhost:60005/configuration",
                Url = "ProtectedWebsite"
            };
            var umaFilterOptions = new UmaFilterOptions
            {
                Authorization = new UmaFilterAuthorizationOptions
                {
                    AuthorizationWellKnownConfiguration = "http://localhost:60004/.well-known/uma2-configuration",
                    ClientId = "ProtectedWebsite",
                    ClientSecret = "ProtectedWebsite"
                },
                ResourceManager = new UmaFilterResourceManagerAuthorizationOptions
                {
                    ConfigurationUrl = "http://localhost:60005/configuration",
                    ClientId = "tmp",
                    ClientSecret = "tmp"
                }
            };
            services.AddHierarchicalResourceResolver(_resourceManagerResolverOptions);
            services.AddUmaFilter(umaFilterOptions);
            services.AddAuthentication(Constants.CookieName)
                .AddCookie(Constants.CookieName);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            app.UseAuthentication();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
