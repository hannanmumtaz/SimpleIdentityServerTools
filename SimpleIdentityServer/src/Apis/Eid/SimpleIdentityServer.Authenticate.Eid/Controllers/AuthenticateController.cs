#region copyright
// Copyright 2015 Habart Thierry
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using SimpleBus.Core;
using SimpleIdentityServer.Authenticate.Eid.Core.Login;
using SimpleIdentityServer.Authenticate.Eid.Extensions;
using SimpleIdentityServer.Authenticate.Eid.ViewModels;
using SimpleIdentityServer.Core;
using SimpleIdentityServer.Core.Common.DTOs;
using SimpleIdentityServer.Core.Exceptions;
using SimpleIdentityServer.Core.Extensions;
using SimpleIdentityServer.Core.Protector;
using SimpleIdentityServer.Core.Services;
using SimpleIdentityServer.Core.Translation;
using SimpleIdentityServer.Core.WebSite.Authenticate;
using SimpleIdentityServer.Core.WebSite.User;
using SimpleIdentityServer.EventStore.Core.Models;
using SimpleIdentityServer.EventStore.Core.Repositories;
using SimpleIdentityServer.Handler.Events;
using SimpleIdentityServer.Host;
using SimpleIdentityServer.Host.Controllers.Website;
using SimpleIdentityServer.Host.Extensions;
using SimpleIdentityServer.Host.ViewModels;
using SimpleIdentityServer.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Authenticate.Eid.Controllers
{
    [Area("EidAuthentication")]
    public class AuthenticateController : BaseController
    {
        private const string ExternalAuthenticateCookieName = "SimpleIdentityServer-{0}";        
        private const string DefaultLanguage = "en";        
        private readonly IAuthenticateActions _authenticateActions;
        private readonly IDataProtector _dataProtector;
        private readonly IEncoder _encoder;
        private readonly ITranslationManager _translationManager;        
        private readonly ISimpleIdentityServerEventSource _simpleIdentityServerEventSource;        
        private readonly IUrlHelper _urlHelper;
        private readonly IEventAggregateRepository _eventAggregateRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IAuthenticationSchemeProvider _authenticationSchemeProvider;
        private readonly IPayloadSerializer _payloadSerializer;
        private readonly IConfigurationService _configurationService;
        private readonly ILoginActions _loginActions;
        private readonly EidAuthenticateOptions _eidAuthenticateOptions;

        public AuthenticateController(
            IAuthenticateActions authenticateActions,
            IDataProtectionProvider dataProtectionProvider,
            IEncoder encoder,
            ITranslationManager translationManager,
            ISimpleIdentityServerEventSource simpleIdentityServerEventSource,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor,
            IEventAggregateRepository eventAggregateRepository,
            IEventPublisher eventPublisher,
            IAuthenticationService authenticationService,
            IAuthenticationSchemeProvider authenticationSchemeProvider,
            IUserActions userActions,
            IPayloadSerializer payloadSerializer,
            AuthenticateOptions authenticateOptions,
            IConfigurationService configurationService,
            ILoginActions loginActions,
            EidAuthenticateOptions eidAuthenticateOptions) : base(authenticationService, userActions, authenticateOptions)
        {
            _authenticateActions = authenticateActions;
            _dataProtector = dataProtectionProvider.CreateProtector("Request");
            _encoder = encoder;
            _translationManager = translationManager;
            _simpleIdentityServerEventSource = simpleIdentityServerEventSource;            
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            _eventAggregateRepository = eventAggregateRepository;
            _eventPublisher = eventPublisher;
            _payloadSerializer = payloadSerializer;
            _authenticationSchemeProvider = authenticationSchemeProvider;
            _configurationService = configurationService;
            _loginActions = loginActions;
            _eidAuthenticateOptions = eidAuthenticateOptions;
        }
        
        #region Public methods
        
        public async Task<ActionResult> Logout()
        {
            HttpContext.Response.Cookies.Delete(SimpleIdentityServer.Core.Constants.SESSION_ID);
            await _authenticationService.SignOutAsync(HttpContext, _authenticateOptions.CookieName, new Microsoft.AspNetCore.Authentication.AuthenticationProperties());
            return RedirectToAction("Index", "Authenticate");
        }
                
        #region Normal authentication process
        
        public async Task<ActionResult> Index(string name)
        {
            var authenticatedUser = await SetUser();
            if (authenticatedUser.Key == null ||
                authenticatedUser.Key.Identity == null ||
                !authenticatedUser.Key.Identity.IsAuthenticated)
            {
                await TranslateView(DefaultLanguage);
                var viewModel = new LoginViewModel();
                await SetIdProviders(viewModel);
                return View(viewModel);
            }

            return RedirectToAction("Index", "User");
        }
        
        [HttpPost]
        public async Task<ActionResult> LocalLogin(LoginViewModel loginViewModel)
        {
            var authenticatedUser = await SetUser();
            if (authenticatedUser.Key != null &&
                authenticatedUser.Key.Identity != null &&
                authenticatedUser.Key.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "User");
            }

            if (loginViewModel == null)
            {
                throw new ArgumentNullException(nameof(loginViewModel));
            }

            if (!ModelState.IsValid)
            {
                await TranslateView(DefaultLanguage);
                await SetIdProviders(loginViewModel);
                return View("Index", loginViewModel);
            }

            try
            {
                var resourceOwner = await _loginActions.LocalAuthenticate(loginViewModel.ToParameter());
                var claims = resourceOwner.Claims;
                claims.Add(new Claim(ClaimTypes.AuthenticationInstant,
                    DateTimeOffset.UtcNow.ConvertToUnixTimestamp().ToString(CultureInfo.InvariantCulture),
                    ClaimValueTypes.Integer));
                var subject = claims.First(c => c.Type == SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Subject).Value;
                await SetLocalCookie(claims, Guid.NewGuid().ToString());
                _simpleIdentityServerEventSource.AuthenticateResourceOwner(subject);
                return RedirectToAction("Index", "User");
            }
            catch (Exception exception)
            {
                _simpleIdentityServerEventSource.Failure(exception.Message);
                await TranslateView("en");
                ModelState.AddModelError("invalid_credentials", exception.Message);
                await SetIdProviders(loginViewModel);
                return View("Index", loginViewModel);
            }
        }
        
        [HttpPost]
        public async Task ExternalLogin(string provider)
        {
            if (string.IsNullOrWhiteSpace(provider))
            {
                throw new ArgumentNullException(nameof(provider));
            }

            var redirectUrl = _urlHelper.AbsoluteAction("LoginCallback", "Authenticate");
            await _authenticationService.ChallengeAsync(HttpContext, provider, new Microsoft.AspNetCore.Authentication.AuthenticationProperties() 
            {
                RedirectUri = redirectUrl
            });
        }
        
        [HttpGet]
        public async Task<ActionResult> LoginCallback(string error)
        {
            if (!string.IsNullOrWhiteSpace(error))
            {
                throw new IdentityServerException(
                    SimpleIdentityServer.Core.Errors.ErrorCodes.UnhandledExceptionCode,
                    string.Format(SimpleIdentityServer.Core.Errors.ErrorDescriptions.AnErrorHasBeenRaisedWhenTryingToAuthenticate, error));
            }

            // 1. Check if the user exists and insert it
            var authenticatedUser = await _authenticationService.GetAuthenticatedUser(this, _authenticateOptions.ExternalCookieName);
            var claims = await _authenticateActions.LoginCallback(authenticatedUser);

            // 2. Set cookie
            await SetLocalCookie(claims, Guid.NewGuid().ToString());
            await _authenticationService.SignOutAsync(HttpContext, _authenticateOptions.ExternalCookieName, new Microsoft.AspNetCore.Authentication.AuthenticationProperties());

            // 3. Redirect to the profile
            return RedirectToAction("Index", "User");
        }
        
        #endregion
        
        #region Authentication process which follows OPENID
        
        [HttpGet]
        public async Task<ActionResult> OpenId(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            var authenticatedUser = await SetUser();
            var request = _dataProtector.Unprotect<AuthorizationRequest>(code);
            var actionResult = await _authenticateActions.AuthenticateResourceOwnerOpenId(
                request.ToParameter(),
                authenticatedUser.Key,
                code);
            var result = this.CreateRedirectionFromActionResult(actionResult,
                request);
            if (result != null)
            {
                await LogAuthenticateUser(actionResult, request.ProcessId);
                return result;
            }

            await TranslateView(request.UiLocales);
            var viewModel = new EidAuthorizeViewModel
            {
                Code = code
            };

            await SetIdProviders(viewModel);
            return View(viewModel);
        }
        
        [HttpPost]
        public async Task<ActionResult> LocalLoginOpenId(EidAuthorizeViewModel authorizeOpenId)
        {
            if (authorizeOpenId == null)
            {
                throw new ArgumentNullException(nameof(authorizeOpenId));
            }

            if (string.IsNullOrWhiteSpace(authorizeOpenId.Code))
            {
                throw new ArgumentNullException(nameof(authorizeOpenId.Code));
            }

            await SetUser();
            var uiLocales = DefaultLanguage;
            try
            {
                // 1. Decrypt the request
                var request = _dataProtector.Unprotect<AuthorizationRequest>(authorizeOpenId.Code);
                
                // 2. Retrieve the default language
                uiLocales = string.IsNullOrWhiteSpace(request.UiLocales) ? DefaultLanguage : request.UiLocales;
                
                // 3. Check the state of the view model
                if (!ModelState.IsValid)
                {
                    await TranslateView(uiLocales);
                    await SetIdProviders(authorizeOpenId);
                    return View("OpenId", authorizeOpenId);
                }

                // 4. Local authentication
                var actionResult = await _loginActions.OpenIdLocalAuthenticate(authorizeOpenId.ToParameter(),
                    request.ToParameter(),
                    authorizeOpenId.Code);
                var subject = actionResult.Claims.First(c => c.Type == SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Subject).Value;

                // 5. Authenticate the user by adding a cookie
                await SetLocalCookie(actionResult.Claims, request.SessionId);
                _simpleIdentityServerEventSource.AuthenticateResourceOwner(subject);

                // 6. Redirect the user agent
                var result = this.CreateRedirectionFromActionResult(actionResult.ActionResult,
                    request);
                if (result != null)
                {
                    await LogAuthenticateUser(actionResult.ActionResult, request.ProcessId);
                    return result;
                }
            }
            catch (Exception ex)
            {
                _simpleIdentityServerEventSource.Failure(ex.Message);
                ModelState.AddModelError("invalid_credentials", ex.Message);
            }

            await TranslateView(uiLocales);
            await SetIdProviders(authorizeOpenId);
            return View("OpenId", authorizeOpenId);
        }
        
        [HttpPost]
        public async Task ExternalLoginOpenId(string provider, string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException("code");
            }

            // 1. Persist the request code into a cookie & fix the space problems
            var cookieValue = Guid.NewGuid().ToString();
            var cookieName = string.Format(ExternalAuthenticateCookieName, cookieValue);
            Response.Cookies.Append(cookieName, code, 
                new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddMinutes(5)
                });

            // 2. Redirect the User agent
            var redirectUrl = _urlHelper.AbsoluteAction("LoginCallbackOpenId", "Authenticate", new { code = cookieValue });
            await _authenticationService.ChallengeAsync(HttpContext, provider, new Microsoft.AspNetCore.Authentication.AuthenticationProperties
            {
                RedirectUri = redirectUrl
            });
        }
        
        [HttpGet]
        public async Task<ActionResult> LoginCallbackOpenId(string code, string error)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException("code");
            }

            // 1 : retrieve the request from the cookie
            var cookieName = string.Format(ExternalAuthenticateCookieName, code);
            var request = Request.Cookies[string.Format(ExternalAuthenticateCookieName, code)];
            if (request == null)
            {
                throw new IdentityServerException(SimpleIdentityServer.Core.Errors.ErrorCodes.UnhandledExceptionCode,
                    SimpleIdentityServer.Core.Errors.ErrorDescriptions.TheRequestCannotBeExtractedFromTheCookie);
            }

            // 2 : remove the cookie
            Response.Cookies.Append(cookieName, string.Empty,
                new CookieOptions{
                    Expires = DateTime.UtcNow.AddDays(-1)
                });

            // 3 : Raise an exception is there's an authentication error
            if (!string.IsNullOrWhiteSpace(error))
            {
                throw new IdentityServerException(
                    SimpleIdentityServer.Core.Errors.ErrorCodes.UnhandledExceptionCode, 
                    string.Format(SimpleIdentityServer.Core.Errors.ErrorDescriptions.AnErrorHasBeenRaisedWhenTryingToAuthenticate, error));
            }            
            
            // 4. Check if the user is authenticated
            var authenticatedUser = await _authenticationService.GetAuthenticatedUser(this, _authenticateOptions.ExternalCookieName);
            if (authenticatedUser == null ||
                !authenticatedUser.Identity.IsAuthenticated ||
                !(authenticatedUser.Identity is ClaimsIdentity)) {
                  throw new IdentityServerException(
                        SimpleIdentityServer.Core.Errors.ErrorCodes.UnhandledExceptionCode,
                        SimpleIdentityServer.Core.Errors.ErrorDescriptions.TheUserNeedsToBeAuthenticated);
            }
            
            // 5. Rerieve the claims
            var claimsIdentity = authenticatedUser.Identity as ClaimsIdentity;
            var claims = claimsIdentity.Claims.ToList();

            // 6. Try to authenticate the resource owner & returns the claims.
            var authorizationRequest = _dataProtector.Unprotect<AuthorizationRequest>(request);
            var actionResult = await _authenticateActions.ExternalOpenIdUserAuthentication(
                claims,
                authorizationRequest.ToParameter(),
                request);

            // 7. Store claims into new cookie
            if (actionResult.ActionResult != null)
            {
                await SetLocalCookie(actionResult.Claims, authorizationRequest.SessionId);
                await _authenticationService.SignOutAsync(HttpContext, _authenticateOptions.ExternalCookieName, new Microsoft.AspNetCore.Authentication.AuthenticationProperties());
                await LogAuthenticateUser(actionResult.ActionResult, authorizationRequest.ProcessId);
                return this.CreateRedirectionFromActionResult(actionResult.ActionResult,
                    authorizationRequest);
            }

            return RedirectToAction("OpenId", "Authenticate", new { code = code });
        }

        #endregion

        #endregion

        #region Private methods

        private async Task LogAuthenticateUser(SimpleIdentityServer.Core.Results.ActionResult act, string processId)
        {
            if (string.IsNullOrWhiteSpace(processId))
            {
                return;
            }

            var evtAggregate = await GetLastEventAggregate(processId);
            if (evtAggregate == null)
            {
                return;
            }

            _eventPublisher.Publish(new ResourceOwnerAuthenticated(Guid.NewGuid().ToString(), processId, _payloadSerializer.GetPayload(act), evtAggregate.Order + 1));
        }

        private async Task<EventAggregate> GetLastEventAggregate(string aggregateId)
        {
            var events = (await _eventAggregateRepository.GetByAggregate(aggregateId)).OrderByDescending(e => e.Order);
            if (events == null || !events.Any())
            {
                return null;
            }

            return events.First();
        }

        private async Task TranslateView(string uiLocales)
        {
            var translations = await _translationManager.GetTranslationsAsync(uiLocales, new List<string>
            {
                SimpleIdentityServer.Core.Constants.StandardTranslationCodes.LoginCode,
                SimpleIdentityServer.Core.Constants.StandardTranslationCodes.UserNameCode,
                SimpleIdentityServer.Core.Constants.StandardTranslationCodes.PasswordCode,
                SimpleIdentityServer.Core.Constants.StandardTranslationCodes.RememberMyLoginCode,
                SimpleIdentityServer.Core.Constants.StandardTranslationCodes.LoginLocalAccount,
                SimpleIdentityServer.Core.Constants.StandardTranslationCodes.LoginExternalAccount,
                SimpleIdentityServer.Core.Constants.StandardTranslationCodes.SendCode,
                SimpleIdentityServer.Core.Constants.StandardTranslationCodes.Code,
                SimpleIdentityServer.Core.Constants.StandardTranslationCodes.ConfirmCode,
                SimpleIdentityServer.Core.Constants.StandardTranslationCodes.SendConfirmationCode
            });

            ViewBag.Translations = translations;
        }

        private async Task SetLocalCookie(IEnumerable<Claim> claims, string sessionId)
        {
            var cls = claims.ToList();
            var tokenValidity = await _configurationService.GetTokenValidityPeriodInSecondsAsync();
            var now = DateTime.UtcNow;
            var expires = now.AddSeconds(tokenValidity);
            HttpContext.Response.Cookies.Append(SimpleIdentityServer.Core.Constants.SESSION_ID, sessionId, new CookieOptions
            {
                HttpOnly = false,
                Expires = expires
            });
            var identity = new ClaimsIdentity(cls, _authenticateOptions.CookieName);
            var principal = new ClaimsPrincipal(identity);
            await _authenticationService.SignInAsync(HttpContext, _authenticateOptions.CookieName, principal, new Microsoft.AspNetCore.Authentication.AuthenticationProperties
            {
                IssuedUtc = now,
                ExpiresUtc = expires,
                AllowRefresh = false,
                IsPersistent = false
            });
        }

        private async Task SetTwoFactorCookie(IEnumerable<Claim> claims)
        {
            var identity = new ClaimsIdentity(claims, Host.Constants.TwoFactorCookieName);
            var principal = new ClaimsPrincipal(identity);
            await _authenticationService.SignInAsync(HttpContext, Host.Constants.TwoFactorCookieName, principal, new Microsoft.AspNetCore.Authentication.AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddMinutes(5),
                IsPersistent = false
            });
        }

        private async Task SetIdProviders(LoginViewModel loginViewModel)
        {
            var schemes = (await _authenticationSchemeProvider.GetAllSchemesAsync()).Where(p => !string.IsNullOrWhiteSpace(p.DisplayName));
            var idProviders = new List<IdProviderViewModel>();
            foreach (var scheme in schemes)
            {
                idProviders.Add(new IdProviderViewModel
                {
                    AuthenticationScheme = scheme.Name,
                    DisplayName = scheme.DisplayName
                });
            }

            loginViewModel.EidUrl = _eidAuthenticateOptions.EidUrl;
            loginViewModel.IdProviders = idProviders;
        }

        private async Task SetIdProviders(EidAuthorizeViewModel authorizeViewModel)
        {
            var schemes = (await _authenticationSchemeProvider.GetAllSchemesAsync()).Where(p => !string.IsNullOrWhiteSpace(p.DisplayName));
            var idProviders = new List<IdProviderViewModel>();
            foreach (var scheme in schemes)
            {
                idProviders.Add(new IdProviderViewModel
                {
                    AuthenticationScheme = scheme.Name,
                    DisplayName = scheme.DisplayName
                });
            }

            authorizeViewModel.EidUrl = _eidAuthenticateOptions.EidUrl;
            authorizeViewModel.IdProviders = idProviders;
        }

        #endregion
    }
}