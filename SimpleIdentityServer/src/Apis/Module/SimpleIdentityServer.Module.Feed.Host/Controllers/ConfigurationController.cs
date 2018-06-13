using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.Module.Feed.Common.Responses;
using SimpleIdentityServer.Module.Feed.Host.Extensions;

namespace SimpleIdentityServer.Module.Feed.Host.Controllers
{
    [Route(Constants.Routes.ConfigurationController)]
    public class ConfigurationController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var issuer = Request.GetAbsoluteUriWithVirtualPath();
            var result = new ConfigurationResponse
            {
                ProjectsEndpoint = $"{issuer}/{Constants.Routes.ProjectsController}",
                ConnectorsEndpoint = $"{issuer}/{Constants.Routes.ConnectorsController}"
            };
            return new OkObjectResult(result);
        }
    }
}
