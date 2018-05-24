using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.Uma.Authentication;
using SimpleIdentityServer.Uma.Client;
using WebApiContrib.Core.Storage;
using WebApiContrib.Core.Storage.InMemory;

namespace SimpleIdentityServer.ProtectedWebsite.Mvc
{
    public class Startup
    {
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
            ConfigureInMemoryStorage(services);
            services.AddLogging();
            services.AddMvc();
            services.AddIdServerClient().AddUmaClient();
            services.AddAuthentication(Constants.CookieName)
                .AddCookie(Constants.CookieName);
            var options = new UmaFilterAuthorizationOptions
            {
                AuthorizationWellKnownConfiguration = "http://localhost:60004/.well-known/uma2-configuration",
                ClientId = "ProtectedWebsite",
                ClientSecret = "ProtectedWebsite"
            };
            services.AddSingleton(new UmaFilterOptions
            {
                Authorization = options
            });
            var s = new ServiceCollection();
            services.AddTransient<UmaFilter>();
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

        private void ConfigureInMemoryStorage(IServiceCollection services)
        {
            services.AddStorage(opt => opt.UseInMemory());
        }
    }
}
