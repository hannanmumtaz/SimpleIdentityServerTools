using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.ProtectedWebsite.Mvc.Filters;
using SimpleIdentityServer.Uma.Client;

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
            services.AddLogging();
            services.AddMvc();
            services.AddIdServerClient().AddUmaClient();
            var options = new UmaFilterOptions
            {
                AuthorizationWellKnownConfiguration = "http://localhost:60004/.well-known/uma2-configuration",
                ClientId = "ResourceServer",
                ClientSecret = "LW46am54neU/[=Su"
            };
            services.AddSingleton(options);
            services.AddTransient<UmaFilter>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
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
