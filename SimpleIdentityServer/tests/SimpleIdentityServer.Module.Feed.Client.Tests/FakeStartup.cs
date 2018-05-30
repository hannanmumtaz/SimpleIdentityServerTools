using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Module.Feed.Client.Tests.Extensions;
using SimpleIdentityServer.Module.Feed.EF;
using SimpleIdentityServer.Module.Feed.EF.InMemory;
using SimpleIdentityServer.Module.Feed.Host.Controllers;
using SimpleIdentityServer.Module.Feed.Host.Extensions;
using System;
using System.Reflection;

namespace SimpleIdentityServer.Module.Feed.Client.Tests
{
    public class FakeStartup : IStartup
    {
        public FakeStartup() { }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var mvc = services.AddMvc();
            services.AddModuleInMemoryEF();
            services.AddModuleFeedHost();
            var parts = mvc.PartManager.ApplicationParts;
            parts.Clear();
            parts.Add(new AssemblyPart(typeof(ConfigurationController).GetTypeInfo().Assembly));
            return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app)
        {
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
    }
}