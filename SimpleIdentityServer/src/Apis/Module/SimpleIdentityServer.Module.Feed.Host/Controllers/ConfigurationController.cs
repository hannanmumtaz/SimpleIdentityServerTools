using Microsoft.AspNetCore.Mvc;

namespace SimpleIdentityServer.Module.Feed.Host.Controllers
{
    [Route(Constants.Routes.ConfigurationController)]
    public class ConfigurationController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return new OkResult();
        }
    }
}
