using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.ResourceManager.API.Host.Extensions;
using SimpleIdentityServer.ResourceManager.Common.Responses;

namespace SimpleIdentityServer.ResourceManager.API.Host.Controllers
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
                EndpointsEdp = $"{issuer}/{Constants.RouteNames.EndpointsController}",
                ProfileEdp = $"{issuer}/{Constants.RouteNames.ProfileController}",
                ElfinderEdp = $"{issuer}/{Constants.RouteNames.ElFinterController}",
                ClientsEdp = $"{issuer}/{Constants.RouteNames.ClientsController}",
                ScopesEdp = $"{issuer}/{Constants.RouteNames.ScopesController}",
                ResourceOwnersEdp = $"{issuer}/{Constants.RouteNames.ResourceOwnersController}",
                ClaimsEdp = $"{issuer}/{Constants.RouteNames.ClaimsController}",
                ResourcesEdp = $"{issuer}/{Constants.RouteNames.ResourcesController}",
                AuthPoliciesEdp = $"{issuer}/{Constants.RouteNames.AuthPoliciesController}",
                ScimEdp = $"{issuer}/{Constants.RouteNames.ScimController}",
                HierarchicalresourcesEdp = $"{issuer}/{Constants.RouteNames.HierarchicalResourcesController}",
                ParametersEdp = $"{issuer}/{Constants.RouteNames.ParametersController}"
            };
            return new OkObjectResult(configuration);
        }
    }
}
