using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.Uma.Authentication;

namespace SimpleIdentityServer.ProtectedWebsite.Mvc.Controllers
{
    [UmaFilter(ResourceUrl = "ProtectedWebsite/Administrator/Index", Scopes =  "read")]
    public class AdministratorController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
