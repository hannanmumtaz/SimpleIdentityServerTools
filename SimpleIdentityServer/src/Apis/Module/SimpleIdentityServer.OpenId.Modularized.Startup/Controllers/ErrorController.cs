using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SimpleIdentityServer.OpenId.Modularized.Startup
{
    public class ErrorController : BaseController
    {
        public ErrorController(IAuthenticationService authenticationService) : base(authenticationService)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Get401()
        {
            await SetUser();
            return View();
        }
        
        public async Task<ActionResult> Get404()
        {
            await SetUser();
            return View();    
        }

        public async Task<ActionResult> Get500()
        {
            await SetUser();
            return View();
        }
    }
}