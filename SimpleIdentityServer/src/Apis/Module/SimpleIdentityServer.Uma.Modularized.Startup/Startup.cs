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
using System;
using System.Collections.Generic;

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
                NugetSources = new List<string>
                {
                    @"d:\sidfeeds\",
                    "https://api.nuget.org/v3/index.json",
                    "https://www.myget.org/F/advance-ict/api/v3/index.json"
                },
                ModuleFeedUri = new Uri("http://localhost:60008/configuration"),
                ProjectName = "UmaProvider"
            });
            _moduleLoader.ModuleInstalled += ModuleInstalled;
            _moduleLoader.PackageRestored += PackageRestored;
            _moduleLoader.ModulesLoaded += ModulesLoaded;
            _moduleLoader.ModuleCannotBeInstalled += ModuleCannotBeInstalled;
            _moduleLoader.Initialize();
            _moduleLoader.RestorePackages().Wait();
            _moduleLoader.LoadModules();
        }

        public IConfigurationRoot Configuration { get; set; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
            var mvc = services.AddMvc();
            var authBuilder = services.AddAuthentication();
            _moduleLoader.ConfigureModuleServices(services, mvc, _env);
            _moduleLoader.ConfigureModuleAuthentication(authBuilder);
            services.AddAuthorization(opts =>
            {
                _moduleLoader.ConfigureModuleAuthorization(opts);
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            app.UseAuthentication();
            app.UseCors("AllowAll");
            _moduleLoader.Configure(app);
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}");
            });
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
