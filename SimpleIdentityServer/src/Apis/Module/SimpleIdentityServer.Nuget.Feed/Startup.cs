using System;
using System.Collections.Generic;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using NuGet.Server.Core.Infrastructure;
using NuGet.Server.Core.Logging;
using NuGet.Server.V2;
using Owin;

namespace SimpleIdentityServer.Nuget.Feed
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            appBuilder.Use(typeof(BasicAuthentication));
            var config = new HttpConfiguration();
            appBuilder.UseWebApi(config);
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            NuGetV2WebApiEnabler.UseNuGetV2WebApiFeed(config,
                routeName: "NuGetAdmin",
                routeUrlRoot: "NuGet/admin",
                oDatacontrollerName: "NuGetPrivateOData");
            NuGetV2WebApiEnabler.UseNuGetV2WebApiFeed(config,
                routeName: "NuGetPublic",
                routeUrlRoot: "NuGet/public",
                oDatacontrollerName: "NuGetPublicOData");
            NuGetV2WebApiEnabler.UseNuGetV2WebApiFeed(config,
                routeName: "NuGetVeryPublic",
                routeUrlRoot: "NuGet/verypublic",
                oDatacontrollerName: "NuGetVeryPublicOData");

        }
    }
}