using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.Core.WebSite.User;
using SimpleIdentityServer.Host;
using SimpleIdentityServer.Host.Controllers.Website;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Eid.OpenId.Controllers
{
    [Area("EidWebsite")]
    public class ErrorController : BaseController
    {
        public ErrorController(IAuthenticationService authenticationService, IUserActions userActions, AuthenticateOptions authenticateOptions)
            : base(authenticationService, userActions, authenticateOptions)
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