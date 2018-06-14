using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SimpleIdentityServer.OpenId.Modularized.Startup.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IAuthenticationService authenticationService) : base(authenticationService)
        {

        }

        public async Task<IActionResult> Index()
        {
            await SetUser();
            return View();
        }
    }
}
