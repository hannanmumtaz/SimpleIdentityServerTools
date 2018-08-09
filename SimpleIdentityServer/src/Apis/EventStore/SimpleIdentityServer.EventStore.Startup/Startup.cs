using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleBus.Core;
using SimpleBus.InMemory;
using SimpleIdentityServer.EventStore.Core.Repositories;
using SimpleIdentityServer.EventStore.EF;
using SimpleIdentityServer.EventStore.Host.Extensions;
using SimpleIdentityServer.EventStore.InMemory;
using SimpleIdentityServer.EventStore.Startup.Handlers;
using System.Collections.Generic;

namespace SimpleIdentityServer.EventStore.Startup
{
    public class Startup
    {
        private InMemoryEventSubscriber _eventSubscriber;

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
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
            services.AddEventStoreInMemoryEF();
            services.AddEventStoreHost();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var repository = (IEventAggregateRepository)app.ApplicationServices.GetService(typeof(IEventAggregateRepository));
            applicationLifetime.ApplicationStarted.Register(() => OnStarted(repository));
            applicationLifetime.ApplicationStopping.Register(OnShutDown);
            loggerFactory.AddConsole();
            app.UseStatusCodePages();
            app.UseCors("AllowAll");
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var scimDbContext = serviceScope.ServiceProvider.GetService<EventStoreContext>();
                scimDbContext.Database.EnsureCreated();
            }
        }

        private void OnStarted(IEventAggregateRepository repository)
        {
            _eventSubscriber = new InMemoryEventSubscriber(new InMemoryOptions(), new List<IEventHandler>
            {
                new OauthHandler(repository),
                new OpenidHandler(repository),
                new ScimHandler(repository)
            });
            _eventSubscriber.Listen();
        }

        private void OnShutDown()
        {
            _eventSubscriber.Dispose();
        }
    }
}
