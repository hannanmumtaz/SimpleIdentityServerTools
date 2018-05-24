using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.ResourceManager.Resolver;
using SimpleIdentityServer.Uma.Authentication;
using System.Threading.Tasks;

namespace SimpleIdentityServer.ProtectedWebsite.Mvc.Controllers
{
    [UmaFilter(ResourceUrl = "ProtectedWebsite/Administrator/Index", Scopes =  "read")]
    public class AdministratorController : Controller
    {
        private readonly IResourceManagerResolver _resourceManagerResolver;

        public AdministratorController(IResourceManagerResolver resourceManagerResolver)
        {
            _resourceManagerResolver = resourceManagerResolver;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await _resourceManagerResolver.UpdateViewBag(this);
            return View();
        }
    }
}
