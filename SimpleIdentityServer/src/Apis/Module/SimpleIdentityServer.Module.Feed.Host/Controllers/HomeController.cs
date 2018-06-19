using Microsoft.AspNetCore.Mvc;

namespace SimpleIdentityServer.Module.Feed.Host.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
