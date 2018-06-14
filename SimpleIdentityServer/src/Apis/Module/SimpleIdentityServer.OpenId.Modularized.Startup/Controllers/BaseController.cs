using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.OpenId.Modularized.Startup.Extensions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleIdentityServer.OpenId.Modularized.Startup
{
    public class BaseController : Controller
    {
        protected readonly IAuthenticationService _authenticationService;

        public BaseController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<ClaimsPrincipal> SetUser()
        {
            var authenticatedUser = await _authenticationService.GetAuthenticatedUser(this, Constants.CookieName);
            var isAuthenticed = authenticatedUser != null && authenticatedUser.Identity != null && authenticatedUser.Identity.IsAuthenticated;
            ViewBag.IsAuthenticated = isAuthenticed;
            if (isAuthenticed)
            {
                ViewBag.Name = GetClaimValue(authenticatedUser, "name");
            }
            else
            {
                ViewBag.Name = "unknown";
            }

            return authenticatedUser;
        }

        private static string GetClaimValue(ClaimsPrincipal principal, string claimName)
        {
            if (principal == null ||
                principal.Identity == null)
            {
                return null;
            }

            var claim = principal.FindFirst(claimName);
            if (claim == null)
            {
                return null;
            }

            return claim.Value;
        }
    }
}
