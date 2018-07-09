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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleIdentityServer.Module.Loader;
using System;

namespace SimpleIdentityServer.ScimProvider.Modularized.Startup
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
                ProjectName = "ScimProvider",
                Version = "3.0.0-rc8"
            });
            _moduleLoader.UnitsLoaded += HandleUnitsLoaded;
            _moduleLoader.Initialize();
            _moduleLoader.LoadUnits();
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
            var mvc = services.AddMvc();
            var authBuilder = services.AddAuthentication();
            _moduleLoader.ConfigureUnitsServices(services, mvc, _env);
            // _moduleLoader.ConfigureModuleAuthentication(authBuilder);
            // services.AddAuthorization(opts =>
            // {
            //     _moduleLoader.ConfigureModuleAuthorization(opts);
            // });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            app.UseAuthentication();
            app.UseStatusCodePages();
            app.UseCors("AllowAll");
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void HandleUnitsLoaded(object sender, EventArgs e)
        {
            Console.WriteLine("the units are loaded");
        }
    }
}