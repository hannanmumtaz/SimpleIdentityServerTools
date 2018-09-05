using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.Core.WebSite.User;
using SimpleIdentityServer.Host;
using SimpleIdentityServer.Host.Controllers.Website;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Eid.OpenId.Controllers
{
    [Authorize("Connected")]
    public class UserController : BaseController
    {
        public UserController(
            IAuthenticationService authenticationService,
            IUserActions userActions,
            AuthenticateOptions authenticateOptions) : base(authenticationService, authenticateOptions) { }

        public async Task<IActionResult> Index()
        {
            await SetUser();
            return View();
        }
    }
}
