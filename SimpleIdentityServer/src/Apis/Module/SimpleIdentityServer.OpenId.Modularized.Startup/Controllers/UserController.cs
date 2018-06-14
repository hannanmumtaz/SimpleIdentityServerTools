using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SimpleIdentityServer.OpenId.Modularized.Startup
{
    [Authorize("Connected")]
    public class UserController : BaseController
    {
        public UserController(IAuthenticationService authenticationService) : base(authenticationService)
        {

        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await SetUser();
            return View();
        }
    }
}
