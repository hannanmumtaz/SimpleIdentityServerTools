using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.ProtectedWebsite.Mvc.ViewModels;
using SimpleIdentityServer.ResourceManager.Client;
using SimpleIdentityServer.ResourceManager.Common.Responses;
using SimpleIdentityServer.Uma.Common.DTOs;
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
        private readonly IResourceManagerClientFactory _resourceManagerClientFactory;
        private readonly IIdentityServerUmaClientFactory _identityServerUmaClientFactory;

        public HomeController(IIdentityServerClientFactory identityServerClientFactory, IAuthenticationService authenticationService,
            IResourceManagerClientFactory resourceManagerClientFactory, IIdentityServerUmaClientFactory identityServerUmaClientFactory)
        {
            _identityServerClientFactory = identityServerClientFactory;
            _authenticationService = authenticationService;
            _resourceManagerClientFactory = resourceManagerClientFactory;
            _identityServerUmaClientFactory = identityServerUmaClientFactory;
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

            HttpContext.Response.Cookies.Delete(Uma.Authentication.Constants.DEFAULT_COOKIE_NAME);
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

            // 1. Get the resources.
            var getHierarchicalResource = await _resourceManagerClientFactory.GetHierarchicalResourceClient()
                .Get(new Uri("http://localhost:60005/configuration"), "ProtectedWebsite", true);
            var resources = getHierarchicalResource.Content.Where(r => !string.IsNullOrWhiteSpace(r.ResourceId));
            var resourcePathLst = getHierarchicalResource.Content.Where(r => string.IsNullOrWhiteSpace(r.ResourceId)).Select(r => r.Path).ToList();
            var grantedToken = await _identityServerClientFactory.CreateAuthSelector()
                .UseClientSecretPostAuth("ProtectedWebsite", "ProtectedWebsite")
                .UseClientCredentials("uma_protection")
                .ResolveAsync("http://localhost:60004/.well-known/uma2-configuration");
            List<Task<string>> tasks = new List<Task<string>>(); 
            foreach(var resource in resources)
            {
                tasks.Add(ResolveUrl(resource, grantedToken.AccessToken, result.IdToken));
            }

            var grantedPathLst = (await Task.WhenAll(tasks)).Where(p => !string.IsNullOrWhiteSpace(p));
            resourcePathLst.AddRange(grantedPathLst);
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

        private async Task<string> ResolveUrl(AssetResponse asset, string accessToken, string idToken)
        {
            var permissionResponse = await _identityServerUmaClientFactory.GetPermissionClient()
                .AddByResolution(new PostPermission
                {
                    ResourceSetId = asset.ResourceId,
                    Scopes = new[]
                    {
                        "read"
                    },
                }, "http://localhost:60004/.well-known/uma2-configuration", accessToken);
            var umaGrantedToken = await _identityServerClientFactory.CreateAuthSelector()
                .UseClientSecretPostAuth("ProtectedWebsite", "ProtectedWebsite")
                .UseTicketId(permissionResponse.TicketId, idToken)
                .ResolveAsync("http://localhost:60004/.well-known/uma2-configuration");
            if (umaGrantedToken == null || string.IsNullOrWhiteSpace(umaGrantedToken.AccessToken))
            {
                return null;
            }

            return asset.Path;
        }
    }
}