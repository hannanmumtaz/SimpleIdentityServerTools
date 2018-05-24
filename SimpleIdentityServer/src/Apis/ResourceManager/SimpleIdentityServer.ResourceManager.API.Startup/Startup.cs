#region copyright
// Copyright 2018 Habart Thierry
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
using SimpleIdentityServer.ResourceManager.API.Host.Extensions;
using SimpleIdentityServer.ResourceManager.API.Startup.Extensions;
using SimpleIdentityServer.ResourceManager.Core;
using SimpleIdentityServer.ResourceManager.EF;
using SimpleIdentityServer.ResourceManager.EF.InMemory;
using SimpleIdentityServer.UserInfoIntrospection;
using System;
using WebApiContrib.Core.Storage.InMemory;

namespace SimpleIdentityServer.ResourceManager.API.Startup
{

    public class Startup
    {
        private ResourceManagerHostOptions _options;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
            _options = new ResourceManagerHostOptions
            {
                AuthClientId = Configuration["Auth:ClientId"],
                AuthClientSecret = Configuration["Auth:ClientSecret"],
                AuthWellKnownConfiguration = Configuration["Auth:WellKnownConfiguration"]
            };
        }

        public IConfigurationRoot Configuration { get; set; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            RegisterTokenStore(services);
            RegisterCaching(services);
            RegisterDatabase(services);
            RegisterHost(services);
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
            services.AddMvc();
            services.AddAuthentication(UserInfoIntrospectionOptions.AuthenticationScheme)
                .AddUserInfoIntrospection(opts =>
                {
                    opts.WellKnownConfigurationUrl = "http://localhost:60000/.well-known/openid-configuration";
                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("connected", policy => policy.RequireAuthenticatedUser());
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors("AllowAll");
            loggerFactory.AddConsole();
            app.UseStatusCodePages();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var resourceManagerContext = serviceScope.ServiceProvider.GetService<ResourceManagerDbContext>();
                try
                {
                    resourceManagerContext.Database.EnsureCreated();
                }
                catch (Exception) { }
                resourceManagerContext.EnsureSeedData();
            }
        }

        private static void RegisterTokenStore(IServiceCollection serviceCollection)
        {
            serviceCollection.AddInMemoryTokenStore();
        }

        private static void RegisterCaching(IServiceCollection serviceCollection)
        {
            WebApiContrib.Core.Storage.ServiceCollectionExtensions.AddStorage(serviceCollection, opts => opts.UseInMemory());
        }

        private void RegisterHost(IServiceCollection serviceCollection)
        {
            serviceCollection.AddResourceManagerHost(_options);
        }

        private void RegisterDatabase(IServiceCollection serviceCollection)
        {
            serviceCollection.AddResourceManagerInMemoryEF();
        }
    }
}
