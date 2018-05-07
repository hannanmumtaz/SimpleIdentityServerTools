using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;

namespace SimpleIdentityServer.Authenticate.Eid
{
    public static class RoutingBuilderExtensions
    {
        public static IRouteBuilder UseEidAuthentication(this IRouteBuilder routeBuilder)
        {
            if (routeBuilder == null)
            {
                throw new ArgumentNullException(nameof(routeBuilder));
            }

            routeBuilder.MapRoute("EidAuthentication",
                "Authenticate/{action}/{id?}",
                new { controller = "Authenticate", action = "Index", area = "EidAuthentication" },
                constraints: new { area = "EidAuthentication" });
            return routeBuilder;
        }
    }
}
