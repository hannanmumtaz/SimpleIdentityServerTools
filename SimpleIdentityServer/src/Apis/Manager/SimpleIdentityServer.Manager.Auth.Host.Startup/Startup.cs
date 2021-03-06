#region copyright
// Copyright 2015 Habart Thierry
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using SimpleIdentityServer.Manager.Host.Extensions;
using SimpleIdentityServer.OAuth2Introspection;
using SimpleIdentityServer.UserInfoIntrospection;
using System;
using SimpleIdentityServer.EF.SqlServer;
using WebApiContrib.Core.Concurrency;
using WebApiContrib.Core.Storage.InMemory;
using SimpleIdentityServer.AccessToken.Store.InMemory;

namespace SimpleIdentityServer.Manager.Auth.Host.Startup
{

    public class Startup
    {
        private readonly ManagerOptions _options;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
            _options = new ManagerOptions();
        }

        public IConfigurationRoot Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
            services.AddMvc();
            ConfigureOauthRepositorySqlServer(services);
            ConfigureCaching(services);
            ConfigureLogging(services);
            services.AddInMemoryAccessTokenStore();
            // 3. Configure the manager
            services.AddSimpleIdentityServerManager(_options);
            // 4. Configure the authentication.
            services.AddAuthentication(OAuth2IntrospectionOptions.AuthenticationScheme)
		        .AddOAuth2Introspection(opts =>
                {
                    opts.ClientId = Configuration["Auth:ClientId"];
                    opts.ClientSecret = Configuration["Auth:ClientSecret"];
                    opts.WellKnownConfigurationUrl = Configuration["Auth:WellKnownConfiguration"];
                })
		        .AddUserInfoIntrospection(opts =>
                {
                    opts.WellKnownConfigurationUrl = "http://localhost:60000/.well-known/openid-configuration";
                });
        }

        private void ConfigureOauthRepositorySqlServer(IServiceCollection services)
        {
            var connectionString = "Data Source=.;Initial Catalog=SimpleIdServerOauthUma;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            services.AddOAuthSqlServerEF(connectionString, null);
        }

        private void ConfigureCaching(IServiceCollection services)
        {
            services.AddConcurrency(opt => opt.UseInMemory());
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

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();
            app.UseCors("AllowAll");
            app.UseStatusCodePages();
            app.UseManagerApi();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}");
            });
        }
    }
}
