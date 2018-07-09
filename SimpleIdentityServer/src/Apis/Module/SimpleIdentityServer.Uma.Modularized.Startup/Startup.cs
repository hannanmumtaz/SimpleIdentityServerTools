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
using SimpleIdentityServer.Module.Loader;
using SimpleIdentityServer.OAuth2Introspection;
using SimpleIdentityServer.UserInfoIntrospection;
using System;

namespace SimpleIdentityServer.Uma.Startup
{
    public class Startup
    {
        private IModuleLoader _moduleLoader;
        private IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            _env = env;
            var moduleLoaderFactory = new ModuleLoaderFactory();
            _moduleLoader = moduleLoaderFactory.BuidlerModuleLoader(new ModuleLoaderOptions
            {
                ProjectName = "UmaProvider",
                Version = "3.0.0-rc8"
            });
            _moduleLoader.UnitsLoaded += HandleUnitsLoaded;
            _moduleLoader.Initialize();
            _moduleLoader.LoadUnits();
        }

        public IConfigurationRoot Configuration { get; set; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(OAuth2IntrospectionOptions.AuthenticationScheme)
                .AddOAuth2Introspection(opts =>
                {
                    opts.ClientId = "uma";
                    opts.ClientSecret = "uma";
                    opts.WellKnownConfigurationUrl = "http://localhost:60004/.well-known/uma2-configuration";
                })
                .AddUserInfoIntrospection(opts =>
                {
                    opts.WellKnownConfigurationUrl = "http://localhost:60000/.well-known/openid-configuration";
                });
            services.AddLogging();
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
            var mvc = services.AddMvc();
            _moduleLoader.ConfigureUnitsServices(services, mvc, _env);
            services.AddAuthorization(opts =>
            {
                _moduleLoader.ConfigureUnitsAuthorization(opts);
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            app.UseAuthentication();
            app.UseCors("AllowAll");
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}");
            });
        }

        private void HandleUnitsLoaded(object sender, EventArgs e)
        {
            Console.WriteLine("the units are loaded");
        }
    }
}
