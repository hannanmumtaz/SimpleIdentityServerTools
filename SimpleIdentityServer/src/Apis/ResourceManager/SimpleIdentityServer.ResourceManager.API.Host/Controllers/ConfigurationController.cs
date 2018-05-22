using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SimpleIdentityServer.ResourceManager.API.Host.Extensions;

namespace SimpleIdentityServer.ResourceManager.API.Host.Controllers
{
    [Route(Constants.RouteNames.ConfigurationController)]
    public class ConfigurationController : Controller
    {
        public ActionResult Get()
        {
            var issuer = Request.GetAbsoluteUriWithVirtualPath();
            var json = new JObject();
            json.Add("version", "2.1");
            json.Add("endpoints_endpoint", Constants.RouteNames.EndpointsController);
            json.Add("profile_endpoint", Constants.RouteNames.ProfileController);
            json.Add("elfinder_endpoint", Constants.RouteNames.ElFinterController);
            json.Add("clients_endpoint", Constants.RouteNames.ClientsController);
            json.Add("scopes_endpoint", Constants.RouteNames.ScopesController);
            json.Add("resourceowners_endpoint", Constants.RouteNames.ResourceOwnersController);
            json.Add("claims_endpoint", Constants.RouteNames.ClaimsController);
            json.Add("resources_endpoint", Constants.RouteNames.ResourcesController);
            json.Add("authpolicies_endpoint", Constants.RouteNames.ResourcesController);
            json.Add("scim_endpoint", Constants.RouteNames.ScimController);
            return new OkObjectResult(json);
        }
    }
}
