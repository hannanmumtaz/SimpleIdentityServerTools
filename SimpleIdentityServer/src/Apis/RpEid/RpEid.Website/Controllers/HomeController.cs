using Microsoft.AspNetCore.Mvc;

namespace RpEid.Website.Controllers
{
    public class HomeController : Controller
    {
        public HomeController() { }

        public IActionResult Index()
        {
            return View();
        }
    }
}
