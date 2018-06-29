using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.HierarchicalResource.Common.Responses;
using SimpleIdentityServer.HierarchicalResource.Host.Extensions;

namespace SimpleIdentityServer.HierarchicalResource.Host.Controllers
{
    [Route(Constants.RouteNames.ConfigurationController)]
    public class ConfigurationController : Controller
    {
        public ActionResult Get()
        {
            var issuer = Request.GetAbsoluteUriWithVirtualPath();
            var configuration = new ConfigurationResponse
            {
                Version = "2.1",
                HierarchicalresourcesEdp = $"{issuer}/{Constants.RouteNames.HierarchicalResourcesController}",
                ElfinderEdp = $"{issuer}/{Constants.RouteNames.ElFinterController}"
            };
            return new OkObjectResult(configuration);
        }
    }
}
