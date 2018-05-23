using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;

namespace SimpleIdentityServer.ProtectedWebsite.Mvc.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class UmaFilterAttribute : Attribute, IFilterFactory
    {
        public UmaFilterAttribute(string resourceUrl, string resourceId, params string [] scopes)
        {
            ResourceUrl = resourceUrl;
            ResourceId = resourceId;
            Scopes = scopes;
        }

        public string ResourceUrl { get; private set; }
        public string ResourceId { get; private set; }
        public IEnumerable<string> Scopes { get; private set; }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var filter = (UmaFilter)serviceProvider.GetService(typeof(UmaFilter));
            filter.ResourceId = ResourceId;
            filter.ResourceUrl = ResourceUrl;
            filter.Scopes = Scopes;
            return filter;
        }
    }
}
