using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using SimpleIdentityServer.EF;
using SimpleIdentityServer.Module.Loader;
using System;

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
                ProjectName = "OpenIdProvider",
                Version = "3.0.0-rc8"
            });
            _moduleLoader.UnitsLoaded += HandleUnitsLoaded;
            _moduleLoader.Initialize();
            _moduleLoader.LoadUnits();
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
            var twoFactorAuthBuilder = services.AddAuthentication(Constants.TwoFactorCookieName)
                .AddCookie(Constants.TwoFactorCookieName);
            var authBuilder = services.AddAuthentication(Constants.CookieName)
                .AddCookie(Constants.CookieName, opts =>
                {
                    opts.LoginPath = "/Authenticate";
                });
            _moduleLoader.ConfigureUnitsServices(services, mvcBuilder, _env);
            services.AddAuthorization(opts =>
            {
                _moduleLoader.ConfigureUnitsAuthorization(opts);
            });
            // _moduleLoader.ConfigureConnectorAuthentication(externalAuthBuilder);
            // _moduleLoader.ConfigureModuleAuthentication(authBuilder);
            // services.AddAuthorization(opts =>
            // {
            //     _moduleLoader.ConfigureModuleAuthorization(opts);
            // });
            // _moduleLoader.ConfigureModuleServices(services, mvcBuilder, _env);
            // _moduleLoader.ConfigureTwoFactors(services, mvcBuilder, _env);
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
            //1 . Enable CORS.
            app.UseCors("AllowAll");
            // 2. Configure the application builder.
            _moduleLoader.ConfigureApplicationBuilder(app);
            // 3. Redirect error to custom pages.
            app.UseStatusCodePagesWithRedirects("~/Error/{0}");
            // 4. Configure ASP.NET MVC
            app.UseMvc(routes =>
            {
                _moduleLoader.ConfigureRouter(routes);
            });

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var simpleIdentityServerContext = serviceScope.ServiceProvider.GetService<SimpleIdentityServerContext>();
                simpleIdentityServerContext.Database.EnsureCreated();
                simpleIdentityServerContext.EnsureSeedData();
            }
        }

        private void UseSerilogLogging(ILoggerFactory logger)
        {
            logger.AddSerilog();
        }

        private void HandleUnitsLoaded(object sender, EventArgs e)
        {
            Console.WriteLine("the units are loaded");
        }
    }
}
