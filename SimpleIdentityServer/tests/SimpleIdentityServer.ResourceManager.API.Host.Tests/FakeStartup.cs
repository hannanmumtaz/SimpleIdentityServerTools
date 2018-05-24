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
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.ResourceManager.API.Host.Controllers;
using SimpleIdentityServer.ResourceManager.API.Host.Tests.MiddleWares;
using SimpleIdentityServer.ResourceManager.Core;
using SimpleIdentityServer.ResourceManager.EF.InMemory;
using System;
using System.Reflection;
using WebApiContrib.Core.Storage.InMemory;

namespace SimpleIdentityServer.Host.Tests
{
    public class FakeStartup : IStartup
    {
        public const string DefaultSchema = "Cookies";

        public FakeStartup()
        {
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
            services.AddMvc();
            services.AddResourceManagerInMemoryEF();
            RegisterServices(services);
            services.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = DefaultSchema;
                opts.DefaultChallengeScheme = DefaultSchema;
            }).AddFakeCustomAuth(o => { });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("connected", policy => policy.RequireAuthenticatedUser());
            });
            var mvc = services.AddMvc();
            var parts = mvc.PartManager.ApplicationParts;
            parts.Clear();
            parts.Add(new AssemblyPart(typeof(ConfigurationController).GetTypeInfo().Assembly));
            return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void RegisterServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddResourceManager();
            serviceCollection.AddInMemoryTokenStore();
            WebApiContrib.Core.Storage.ServiceCollectionExtensions.AddStorage(serviceCollection, opts => opts.UseInMemory());
            // serviceCollection.AddSingleton<IConfiguration>(Configuration);
        }
    }
}
