using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.ProtectedWebsite.Mvc.Filters;

namespace SimpleIdentityServer.ProtectedWebsite.Mvc.Controllers
{
    [UmaFilter("", "1", "read")]
    public class AdministratorController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
