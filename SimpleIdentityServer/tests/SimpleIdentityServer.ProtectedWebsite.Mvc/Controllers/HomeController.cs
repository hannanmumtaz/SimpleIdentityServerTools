using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.ProtectedWebsite.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApiContrib.Core.Storage;

namespace SimpleIdentityServer.ProtectedWebsite.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IIdentityServerClientFactory _identityServerClientFactory;
        private readonly IAuthenticationService _authenticationService;
        private readonly IStorage _storage;

        public HomeController(IIdentityServerClientFactory identityServerClientFactory, IAuthenticationService authenticationService,
            IStorage storage)
        {
            _identityServerClientFactory = identityServerClientFactory;
            _authenticationService = authenticationService;
            _storage = storage;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var user = User;
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

            var claim = ((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "sub");
            await _storage.RemoveAsync(claim.Value);
            await _authenticationService.SignOutAsync(HttpContext, Constants.CookieName, new Microsoft.AspNetCore.Authentication.AuthenticationProperties());
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Authenticate()
        {
            return View();
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
                .ResolveAsync(SimpleIdentityServer.ProtectedWebsite.Mvc.Constants.OpenIdUrl);
            var userInfo = await _identityServerClientFactory.CreateUserInfoClient()
                .Resolve(SimpleIdentityServer.ProtectedWebsite.Mvc.Constants.OpenIdUrl, result.AccessToken).ConfigureAwait(false);
            var claims = new List<Claim>();
            claims.Add(new Claim("sub", userInfo["sub"].ToString()));
            claims.Add(new Claim("id_token", result.IdToken));
            await SetLocalCookie(claims);
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