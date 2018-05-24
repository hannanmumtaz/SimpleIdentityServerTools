using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace SimpleIdentityServer.Uma.Authentication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class UmaFilterAttribute : Attribute, IFilterFactory
    {
        public UmaFilterAttribute()
        {
        }

        public string ResourceUrl { get; set; }
        public string ResourceId { get; set; }
        public string Scopes { get; set; }

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
            filter.Scopes = Scopes.Split(' ');
            return filter;
        }
    }
}
