using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleIdentityServer.Module.Loader;
using System;
using System.Collections.Generic;

namespace SimpleIdentityServer.EventStore.Modularized.Startup
{
    public class Startup
    {
        private IModuleLoader _moduleLoader;
        private IHostingEnvironment _env;
        public IConfigurationRoot Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            _env = env;
            var moduleLoaderFactory = new ModuleLoaderFactory();
            _moduleLoader = moduleLoaderFactory.BuidlerModuleLoader(new ModuleLoaderOptions
            {
                NugetSources = new List<string>
                {
                    @"d:\sidfeeds\",
                    "https://api.nuget.org/v3/index.json",
                    "https://www.myget.org/F/advance-ict/api/v3/index.json"
                },
                ModuleFeedUri = new Uri("http://localhost:60008/configuration"),
                ProjectName = "EventStore"
            });
            _moduleLoader.ModuleInstalled += ModuleInstalled;
            _moduleLoader.PackageRestored += PackageRestored;
            _moduleLoader.ModulesLoaded += ModulesLoaded;
            _moduleLoader.ModuleCannotBeInstalled += ModuleCannotBeInstalled;
            _moduleLoader.Initialize();
            _moduleLoader.RestorePackages().Wait();
            _moduleLoader.LoadModules();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
            var mvc = services.AddMvc();
            _moduleLoader.ConfigureServices(services, mvc, _env);
        }

        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            app.UseStatusCodePages();
            app.UseCors("AllowAll");
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            _moduleLoader.Configure(app);
        }

        private static void ModuleCannotBeInstalled(object sender, StrEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"The nuget package {e.Value} cannot be installed");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void ModuleInstalled(object sender, StrEventArgs e)
        {
            Console.WriteLine($"The nuget package {e.Value} is installed");
        }

        private static void PackageRestored(object sender, IntEventArgs e)
        {
            Console.WriteLine($"Finish to restore the packages in {e.Value}");
        }

        private static void ModulesLoaded(object sender, EventArgs e)
        {
            Console.WriteLine("The modules are loaded");
        }
    }
}
