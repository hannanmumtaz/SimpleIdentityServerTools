using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleIdentityServer.Module.Feed.EF;
using SimpleIdentityServer.Module.Feed.EF.InMemory;
using SimpleIdentityServer.Module.Feed.Host.Extensions;
using SimpleIdentityServer.Module.Feed.Startup.Extensions;

namespace SimpleIdentityServer.Module.Feed.Startup
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
            RegisterDatabase(services);
            RegisterHost(services);
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors("AllowAll");
            loggerFactory.AddConsole();
            app.UseStatusCodePages();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var resourceManagerContext = serviceScope.ServiceProvider.GetService<ModuleFeedDbContext>();
                resourceManagerContext.EnsureSeedData();
            }
        }

        private void RegisterDatabase(IServiceCollection serviceCollection)
        {
            serviceCollection.AddModuleInMemoryEF();
        }

        private void RegisterHost(IServiceCollection serviceCollection)
        {
            serviceCollection.AddModuleFeedHost();
        }
    }
}
