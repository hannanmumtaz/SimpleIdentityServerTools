using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Module;
using SimpleIdentityServer.Parameter.Host.Controllers;
using SimpleIdentityServer.Parameter.Host.Extensions;
using System;
using System.Collections.Generic;

namespace SimpleIdentityServer.Parameter.Host
{
    public class ParameterHostModule : IModule
    {
        public void Configure(IApplicationBuilder applicationBuilder)
        {
        }

        public void Configure(IRouteBuilder routeBuilder)
        {
        }

        public void ConfigureServices(IServiceCollection services, IMvcBuilder mvcBuilder = null, IHostingEnvironment env = null, IDictionary<string, string> options = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (mvcBuilder == null)
            {
                throw new ArgumentNullException(nameof(mvcBuilder));
            }


            var assembly = typeof(ParametersController).Assembly;
            mvcBuilder.AddApplicationPart(assembly);
            services.AddParameterHost();
        }

        public IEnumerable<string> GetOptionKeys()
        {
            return new string[0];
        }
    }
}
