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
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleIdentityServer.EF.SqlServer;
using SimpleIdentityServer.HierarchicalResource.EF.InMemory;
using SimpleIdentityServer.HierarchicalResource.Host.Controllers;
using SimpleIdentityServer.HierarchicalResource.Host.Extensions;
using SimpleIdentityServer.Manager.Host.Controllers;
using SimpleIdentityServer.Manager.Host.Extensions;
using SimpleIdentityServer.Merged.API.Startup.Extensions;
using SimpleIdentityServer.OAuth2Introspection;
using SimpleIdentityServer.Profile.EF;
using SimpleIdentityServer.AccessToken.Store.InMemory;
using SimpleIdentityServer.Profile.EF.InMemory;
using SimpleIdentityServer.Profile.Host.Controllers;
using SimpleIdentityServer.Profile.Host.Extensions;
using SimpleIdentityServer.UserInfoIntrospection;
using System;
using WebApiContrib.Core.Concurrency;
using WebApiContrib.Core.Storage.InMemory;

namespace SimpleIdentityServer.Merged.API.Startup
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

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // 1. Profile API.
            app.UseBranchWithServices("/profile", services =>
            {
                services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()));
                services.AddProfileHost();
                services.AddProfileInMemoryEF();
                services.AddAuthentication(UserInfoIntrospectionOptions.AuthenticationScheme)
                    .AddUserInfoIntrospection(opts =>
                    {
                        opts.WellKnownConfigurationUrl = "http://localhost:60000/.well-known/openid-configuration";
                    });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("connected", policy => policy.RequireAuthenticatedUser());
                });
                services.AddMvc().ConfigureApplicationPartManager(manager =>
                {
                    manager.ApplicationParts.Clear();
                    var assembly = typeof(ProfilesController).Assembly;
                    manager.ApplicationParts.Add(new AssemblyPart(assembly));
                });
            }, appBuilder =>
            {
                appBuilder.UseCors("AllowAll");
                appBuilder.UseStatusCodePages();
                appBuilder.UseAuthentication();
                appBuilder.UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
                });
                using (var serviceScope = appBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var resourceManagerContext = serviceScope.ServiceProvider.GetService<ProfileDbContext>();
                    try
                    {
                        resourceManagerContext.Database.EnsureCreated();
                    }
                    catch (Exception) { }
                    resourceManagerContext.EnsureSeedData();
                }
            });
            // 2. Resource API
            app.UseBranchWithServices("/resources", services =>
            {
                services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()));
                app.UseStatusCodePages();
                services.AddHierarchicalResourceInMemoryEF();
                services.AddHierarchicalResourceHost(new HierarchicalResourceOptions());
                services.AddMvc().ConfigureApplicationPartManager(manager =>
                {
                    manager.ApplicationParts.Clear();
                    var assembly = typeof(ElFinderController).Assembly;
                    manager.ApplicationParts.Add(new AssemblyPart(assembly));
                });
            }, appBuilder =>
            {
                appBuilder.UseCors("AllowAll");
                appBuilder.UseStatusCodePages();
                appBuilder.UseAuthentication();
                appBuilder.UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
                });
            });
            // 3. OAUTH2.0 API.
            app.UseBranchWithServices("/oauthmanager", services =>
            {
                services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()));
                services.AddMvc().ConfigureApplicationPartManager(manager =>
                {
                    manager.ApplicationParts.Clear();
                    var assembly = typeof(ClaimsController).Assembly;
                    manager.ApplicationParts.Add(new AssemblyPart(assembly));
                });
                services.AddInMemoryAccessTokenStore();
                services.AddAuthentication(OAuth2IntrospectionOptions.AuthenticationScheme)
                    .AddOAuth2Introspection(opts =>
                    {
                        opts.ClientId = Configuration["OAuthManager:ClientId"];
                        opts.ClientSecret = Configuration["OAuthManager:ClientSecret"];
                        opts.WellKnownConfigurationUrl = Configuration["OAuthManager:WellKnownConfiguration"];
                    })
                    .AddUserInfoIntrospection(opts =>
                    {
                        opts.WellKnownConfigurationUrl = "http://localhost:60000/.well-known/openid-configuration";
                    });
                var connectionString = "Data Source=.;Initial Catalog=SimpleIdServerOauthUma;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                services.AddOAuthSqlServerEF(connectionString, null);
                services.AddConcurrency(opt => opt.UseInMemory());
                services.AddLogging();
                services.AddSimpleIdentityServerManager(new ManagerOptions());
            }, appBuilder =>
            {
                appBuilder.UseCors("AllowAll");
                appBuilder.UseStatusCodePages();
                appBuilder.UseAuthentication();
                appBuilder.UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller}/{action}/{id?}");
                });
            });
            // 4. OPENID API.
            app.UseBranchWithServices("/openidmanager", services =>
            {
                services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()));
                services.AddMvc().ConfigureApplicationPartManager(manager =>
                {
                    manager.ApplicationParts.Clear();
                    var assembly = typeof(ClaimsController).Assembly;
                    manager.ApplicationParts.Add(new AssemblyPart(assembly));
                });
                services.AddInMemoryAccessTokenStore();
                services.AddAuthentication(OAuth2IntrospectionOptions.AuthenticationScheme)
                    .AddOAuth2Introspection(opts =>
                    {
                        opts.ClientId = Configuration["OpenIdManager:ClientId"];
                        opts.ClientSecret = Configuration["OpenIdManager:ClientSecret"];
                        opts.WellKnownConfigurationUrl = Configuration["OpenIdManager:WellKnownConfiguration"];
                    })
                    .AddUserInfoIntrospection(opts =>
                    {
                        opts.WellKnownConfigurationUrl = "http://localhost:60000/.well-known/openid-configuration";
                    });
                var connectionString = "Data Source=.;Initial Catalog=SimpleIdentityServer;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                services.AddOAuthSqlServerEF(connectionString, null);
                services.AddConcurrency(opt => opt.UseInMemory());
                services.AddLogging();
                services.AddSimpleIdentityServerManager(new ManagerOptions());
            }, appBuilder =>
            {
                appBuilder.UseCors("AllowAll");
                appBuilder.UseStatusCodePages();
                appBuilder.UseAuthentication();
                appBuilder.UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller}/{action}/{id?}");
                });
            });
        }
    }
}
