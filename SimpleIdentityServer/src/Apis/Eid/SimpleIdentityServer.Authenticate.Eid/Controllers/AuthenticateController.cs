using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using SimpleBus.Core;
using SimpleIdentityServer.Authenticate.Basic.Controllers;
using SimpleIdentityServer.Authenticate.Eid.Core.Login;
using SimpleIdentityServer.Authenticate.Eid.Extensions;
using SimpleIdentityServer.Authenticate.Eid.ViewModels;
using SimpleIdentityServer.Core;
using SimpleIdentityServer.Core.Api.Profile;
using SimpleIdentityServer.Core.Common.DTOs.Requests;
using SimpleIdentityServer.Core.Exceptions;
using SimpleIdentityServer.Core.Extensions;
using SimpleIdentityServer.Core.Helpers;
using SimpleIdentityServer.Core.Protector;
using SimpleIdentityServer.Core.Services;
using SimpleIdentityServer.Core.Translation;
using SimpleIdentityServer.Core.WebSite.Authenticate;
using SimpleIdentityServer.Core.WebSite.Authenticate.Common;
using SimpleIdentityServer.Core.WebSite.User;
using SimpleIdentityServer.Host;
using SimpleIdentityServer.Host.Extensions;
using SimpleIdentityServer.OpenId.Logging;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Authenticate.Eid.Controllers
{
    [Area(Constants.AMR)]
    public class AuthenticateController : BaseAuthenticateController
    {
        private readonly EidAuthenticateOptions _eidAuthenticateOptions;
        private readonly ILoginActions _loginActions;

        public AuthenticateController(
            IAuthenticateActions authenticateActions,
            IProfileActions profileActions,
            IDataProtectionProvider dataProtectionProvider,
            IEncoder encoder,
            ITranslationManager translationManager,
            IOpenIdEventSource simpleIdentityServerEventSource,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor,
            IEventPublisher eventPublisher,
            IAuthenticationService authenticationService,
            IAuthenticationSchemeProvider authenticationSchemeProvider,
            IUserActions userActions,
            IPayloadSerializer payloadSerializer,
            IConfigurationService configurationService,
            IAuthenticateHelper authenticateHelper,
            IResourceOwnerAuthenticateHelper resourceOwnerAuthenticateHelper,
            ITwoFactorAuthenticationHandler twoFactorAuthenticationHandler,
            ILoginActions loginActions,
            EidAuthenticateOptions basicAuthenticateOptions,
            AuthenticateOptions authenticateOptions) : base(authenticateActions, profileActions, dataProtectionProvider, encoder,
                translationManager, simpleIdentityServerEventSource, urlHelperFactory, actionContextAccessor, eventPublisher,
                authenticationService, authenticationSchemeProvider, userActions, payloadSerializer, configurationService,
                authenticateHelper, twoFactorAuthenticationHandler, basicAuthenticateOptions, authenticateOptions)
        {
            _eidAuthenticateOptions = basicAuthenticateOptions;
            _loginActions = loginActions;
        }
        
        public async Task<ActionResult> Index()
        {
            var authenticatedUser = await SetUser();
            if (authenticatedUser == null ||
                authenticatedUser.Identity == null ||
                !authenticatedUser.Identity.IsAuthenticated)
            {
                await TranslateView(DefaultLanguage);
                var viewModel = new LoginViewModel();
                await SetIdProviders(viewModel);
                return View(viewModel);
            }

            return RedirectToAction("Index", "User", new { area = "UserManagement" });
        }
        
        [HttpPost]
        public async Task<ActionResult> LocalLogin(LoginViewModel loginViewModel)
        {
            var authenticatedUser = await SetUser();
            if (authenticatedUser != null &&
                authenticatedUser.Identity != null &&
                authenticatedUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "User", new { area = "UserManagement" });
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
                var resourceOwner = await _loginActions.LocalAuthenticate(loginViewModel.ToParameter(), _eidAuthenticateOptions.ImagePath, Request.GetAbsoluteUriWithVirtualPath());
                var claims = resourceOwner.Claims;
                claims.Add(new Claim(ClaimTypes.AuthenticationInstant, DateTimeOffset.UtcNow.ConvertToUnixTimestamp().ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer));
                var subject = claims.First(c => c.Type == SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Subject).Value;
                await SetLocalCookie(claims, Guid.NewGuid().ToString());
                _simpleIdentityServerEventSource.AuthenticateResourceOwner(subject);
                return RedirectToAction("Index", "User", new { area = "UserManagement" });
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
        public async Task<ActionResult> LocalLoginOpenId(EidAuthorizeViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            if (string.IsNullOrWhiteSpace(viewModel.Code))
            {
                throw new ArgumentNullException(nameof(viewModel.Code));
            }

            await SetUser();
            var uiLocales = DefaultLanguage;
            try
            {
                // 1. Decrypt the request
                var request = _dataProtector.Unprotect<AuthorizationRequest>(viewModel.Code);

                // 2. Retrieve the default language
                uiLocales = string.IsNullOrWhiteSpace(request.UiLocales) ? DefaultLanguage : request.UiLocales;

                // 3. Check the state of the view model
                if (!ModelState.IsValid)
                {
                    await TranslateView(uiLocales);
                    await SetIdProviders(viewModel);
                    return View("OpenId", viewModel);
                }

                // 4. Local authentication
                var actionResult = await _loginActions.OpenIdLocalAuthenticate(viewModel.ToParameter(), request.ToParameter(), viewModel.Code, _eidAuthenticateOptions.ImagePath, Request.GetAbsoluteUriWithVirtualPath());
                var subject = actionResult.Claims.First(c => c.Type == SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Subject).Value;

                // 5. Two factor authentication.
                if (!string.IsNullOrWhiteSpace(actionResult.TwoFactor))
                {
                    try
                    {
                        await SetTwoFactorCookie(actionResult.Claims);
                        var code = await _authenticateActions.GenerateAndSendCode(subject);
                        _simpleIdentityServerEventSource.GetConfirmationCode(code);
                        return RedirectToAction("SendCode", new { code = viewModel.Code });
                    }
                    catch (ClaimRequiredException)
                    {
                        return RedirectToAction("SendCode", new { code = viewModel.Code });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("invalid_credentials", "Two factor authenticator is not properly configured");
                    }
                }
                else
                {
                    // 6. Authenticate the user by adding a cookie
                    await SetLocalCookie(actionResult.Claims, request.SessionId);
                    _simpleIdentityServerEventSource.AuthenticateResourceOwner(subject);

                    // 7. Redirect the user agent
                    var result = this.CreateRedirectionFromActionResult(actionResult.ActionResult,
                        request);
                    if (result != null)
                    {
                        LogAuthenticateUser(actionResult.ActionResult, request.ProcessId);
                        return result;
                    }
                }
            }
            catch(Exception ex)
            {
                _simpleIdentityServerEventSource.Failure(ex.Message);
                ModelState.AddModelError("invalid_credentials", ex.Message);
            }

            await TranslateView(uiLocales);
            await SetIdProviders(viewModel);
            return View("OpenId", viewModel);
        }
    }
}