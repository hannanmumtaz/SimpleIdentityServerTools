using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Parameter.Core.Helpers;
using SimpleIdentityServer.Parameter.Host.Controllers;
using SimpleIdentityServer.Parameter.Host.Extensions;

namespace SimpleIdentityServer.Parameters.Host.Tests
{
    public class FakeStartup : IStartup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddParameterHost();
            services.AddTransient<IDirectoryHelper, FakeDirectoryHelper>();
            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("get", p => p.RequireAssertion(c => true));
                opts.AddPolicy("add", p => p.RequireAssertion(c => true));
            });
            var mvc = services.AddMvc();
            var parts = mvc.PartManager.ApplicationParts;
            parts.Clear();
            parts.Add(new AssemblyPart(typeof(ParametersController).GetTypeInfo().Assembly));
            return services.BuildServiceProvider();
        }

        private class FakeDirectoryHelper : IDirectoryHelper
        {
            public string GetCurrentDirectory()
            {
                var result = Directory.GetCurrentDirectory();
                return result;
            }
        }
    }
}