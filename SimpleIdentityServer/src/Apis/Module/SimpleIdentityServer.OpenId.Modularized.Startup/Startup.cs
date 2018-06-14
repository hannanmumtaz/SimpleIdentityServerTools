using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using SimpleIdentityServer.Module.Loader;
using System;
using System.Collections.Generic;

namespace SimpleIdentityServer.OpenId.Modularized.Startup
{
    public class Startup
    {
        private IModuleLoader _moduleLoader;
        private IHostingEnvironment _env;
        private IServiceCollection _services;
        private IApplicationBuilder _app;

        public Startup(IHostingEnvironment env)
        {
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
                ProjectName = "OpenIdProvider",
                NugetNbRetry = 5,
                NugetRetryAfterMs = 1000
            });
            _moduleLoader.ModuleInstalled += ModuleInstalled;
            _moduleLoader.PackageRestored += PackageRestored;
            _moduleLoader.ModulesLoaded += ModulesLoaded;
            _moduleLoader.ConnectorsLoaded += HandleConnectorsLoaded;
            _moduleLoader.ModuleCannotBeInstalled += ModuleCannotBeInstalled;
            _moduleLoader.ConnectorsChanged += HandleConnectorsChanged;
            _moduleLoader.Initialize();
            _moduleLoader.RestorePackages().Wait();
            _moduleLoader.RestoreConnectors().Wait();
            _moduleLoader.LoadModules();
            _moduleLoader.LoadConnectors();
            _moduleLoader.WatchConfigurationFileChanges();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            _services = services;
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
            ConfigureLogging(services);
            services.AddLogging();
            var mvcBuilder = services.AddMvc();
            var externalAuthBuilder = services.AddAuthentication(Constants.ExternalCookieName)
                .AddCookie(Constants.ExternalCookieName);
            var authBuilder = services.AddAuthentication(Constants.CookieName)
                .AddCookie(Constants.CookieName, opts =>
                {
                    opts.LoginPath = "/Authenticate";
                });
            _moduleLoader.ConfigureConnectorAuthentication(externalAuthBuilder);
            _moduleLoader.ConfigureModuleAuthentication(authBuilder);
            services.AddAuthorization(opts =>
            {
                _moduleLoader.ConfigureModuleAuthorization(opts);
            });
            _moduleLoader.ConfigureModuleServices(services, mvcBuilder, _env);
        }

        private void ConfigureLogging(IServiceCollection services)
        {
            Func<LogEvent, bool> serilogFilter = (e) =>
            {
                var ctx = e.Properties["SourceContext"];
                var contextValue = ctx.ToString()
                    .TrimStart('"')
                    .TrimEnd('"');
                return contextValue.StartsWith("SimpleIdentityServer") ||
                    e.Level == LogEventLevel.Error ||
                    e.Level == LogEventLevel.Fatal;
            };
            var logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.ColoredConsole();
            var log = logger.Filter.ByIncludingOnly(serilogFilter)
                .CreateLogger();
            Log.Logger = log;
            services.AddLogging();
            services.AddSingleton<Serilog.ILogger>(log);
        }

        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            _app = app;
            UseSerilogLogging(loggerFactory);
            app.UseAuthentication();
            //1 . Enable CORS.
            app.UseCors("AllowAll");
            // 2. Use static files.
            app.UseStaticFiles();
            // 3. Redirect error to custom pages.
            app.UseStatusCodePagesWithRedirects("~/Error/{0}");
            // 4. Configure the modules.
            _moduleLoader.Configure(app);
            // 5. Configure ASP.NET MVC
            app.UseMvc(routes =>
            {
                routes.MapRoute("Error401Route",
                    "Error/401",
                    new
                    {
                        controller = "Error",
                        action = "Get401"
                    });
                routes.MapRoute("Error404Route",
                    "Error/404",
                    new
                    {
                        controller = "Error",
                        action = "Get404"
                    });
                routes.MapRoute("Error500Route",
                    "Error/500",
                    new
                    {
                        controller = "Error",
                        action = "Get500"
                    });
                _moduleLoader.Configure(routes);
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void UseSerilogLogging(ILoggerFactory logger)
        {
            logger.AddSerilog();
        }

        private void HandleConnectorsChanged(object sender, EventArgs e)
        {
            _moduleLoader.Dispose();
            WebHost.Restart();
        }

        private void HandleConnectorsLoaded(object sender, EventArgs e)
        {
            Console.WriteLine("The connectors are loaded");
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
