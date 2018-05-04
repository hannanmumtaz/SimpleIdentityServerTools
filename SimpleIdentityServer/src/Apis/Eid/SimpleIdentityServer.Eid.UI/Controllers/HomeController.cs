using Microsoft.AspNetCore.Mvc;

namespace SimpleIdentityServer.Eid.UI.Controllers
{
    public sealed class HomeController : Controller
    {
        public HomeController() { }

        public IActionResult Index()
        {
            return View();
        }
    }
}
