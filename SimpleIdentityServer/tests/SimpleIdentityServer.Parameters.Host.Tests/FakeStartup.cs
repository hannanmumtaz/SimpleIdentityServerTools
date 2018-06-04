using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
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
            var mvc = services.AddMvc();
            var parts = mvc.PartManager.ApplicationParts;
            parts.Clear();
            parts.Add(new AssemblyPart(typeof(ParametersController).GetTypeInfo().Assembly));
            return services.BuildServiceProvider();
        }
    }
}