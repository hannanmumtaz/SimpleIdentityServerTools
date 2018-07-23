using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.ProtectedWebsite.Mvc.ViewModels;
using SimpleIdentityServer.ResourceManager.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleIdentityServer.ProtectedWebsite.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IIdentityServerClientFactory _identityServerClientFactory;
        private readonly IAuthenticationService _authenticationService;
        private readonly IResourceManagerResolver _resourceManagerResolver;
        private readonly IDataProtector _dataProtector;

        public HomeController(IIdentityServerClientFactory identityServerClientFactory, IAuthenticationService authenticationService,
            IResourceManagerResolver resourceManagerResolver, IDataProtectionProvider dataProtectionProvider)
        {
            _identityServerClientFactory = identityServerClientFactory;
            _authenticationService = authenticationService;
            _resourceManagerResolver = resourceManagerResolver;
            _dataProtector = dataProtectionProvider.CreateProtector(ResourceManager.Resolver.Constants.ProtectorName);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await _resourceManagerResolver.UpdateViewBag(this);
            var user = User;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Authenticate()
        {
            await _resourceManagerResolver.UpdateViewBag(this);
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Logout()
        {
            var identity = User.Identity;
            if (!identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            HttpContext.Response.Cookies.Delete(ResourceManager.Resolver.Constants.DefaultCookieName);
            HttpContext.Response.Cookies.Delete(Uma.Authentication.Constants.DEFAULT_COOKIE_NAME);
            await _authenticationService.SignOutAsync(HttpContext, Constants.CookieName, new Microsoft.AspNetCore.Authentication.AuthenticationProperties());
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate(AuthenticateViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            var result = await _identityServerClientFactory.CreateAuthSelector()
                .UseClientSecretPostAuth("ProtectedWebsite", "ProtectedWebsite")
                .UsePassword(viewModel.Login, viewModel.Password, "openid", "profile")
                .ResolveAsync(Constants.OpenIdUrl);
            var userInfo = await _identityServerClientFactory.CreateUserInfoClient()
                .Resolve(Constants.OpenIdUrl, result.Content.AccessToken).ConfigureAwait(false);
            var claims = new List<Claim>();
            claims.Add(new Claim("sub", userInfo.Content["sub"].ToString()));
            claims.Add(new Claim("id_token", result.Content.IdToken));
            await SetLocalCookie(claims);
            var accessibleResources = await _resourceManagerResolver.ResolveAccessibleResources(result.Content.IdToken);
            this.PersistAccessibleResources(accessibleResources.ToList(), _dataProtector);
            return RedirectToAction("Index", "Home");
        }

        private async Task SetLocalCookie(IEnumerable<Claim> claims)
        {
            var cls = claims.ToList();
            var now = DateTime.UtcNow;
            var expires = now.AddSeconds(3600);
            var identity = new ClaimsIdentity(cls, Constants.CookieName);
            var principal = new ClaimsPrincipal(identity);
            await _authenticationService.SignInAsync(HttpContext, Constants.CookieName, principal, new AuthenticationProperties
            {
                IssuedUtc = now,
                ExpiresUtc = expires,
                AllowRefresh = false,
                IsPersistent = false
            });
        }
    }
}