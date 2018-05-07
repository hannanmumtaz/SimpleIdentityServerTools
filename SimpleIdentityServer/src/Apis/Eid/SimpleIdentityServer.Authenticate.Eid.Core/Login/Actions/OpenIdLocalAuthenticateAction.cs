using SimpleIdentityServer.Core.Exceptions;
using SimpleIdentityServer.Core.Extensions;
using SimpleIdentityServer.Core.Parameters;
using SimpleIdentityServer.Core.WebSite.Authenticate.Actions;
using SimpleIdentityServer.Core.WebSite.Authenticate.Common;
using SimpleIdentityServer.Authenticate.Eid.Core.Parameters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Authenticate.Eid.Core.Login.Actions
{
    public interface IOpenIdLocalAuthenticateAction
    {
        Task<LocalOpenIdAuthenticationResult> Execute(LocalAuthenticateParameter localAuthenticateParameter, AuthorizationParameter authorizationParameter, string code);
    }

    internal sealed class OpenIdLocalAuthenticateAction : IOpenIdLocalAuthenticateAction
    {
        private readonly ILocalAuthenticateAction _localAuthenticateAction;
        private readonly IAuthenticateHelper _authenticateHelper;

        public OpenIdLocalAuthenticateAction(ILocalAuthenticateAction localAuthenticateAction, IAuthenticateHelper authenticateHelper)
        {
            _localAuthenticateAction = localAuthenticateAction;
            _authenticateHelper = authenticateHelper;
        }

        public async Task<LocalOpenIdAuthenticationResult> Execute(LocalAuthenticateParameter localAuthenticateParameter, AuthorizationParameter authorizationParameter, string code)
        {
            if (localAuthenticateParameter == null)
            {
                throw new ArgumentNullException(nameof(localAuthenticateParameter));
            }

            if (authorizationParameter == null)
            {
                throw new ArgumentNullException(nameof(authorizationParameter));
            }

            var resourceOwner = await _localAuthenticateAction.Execute(localAuthenticateParameter);
            if (resourceOwner == null)
            {
                throw new IdentityServerAuthenticationException("the resource owner credentials are not correct");
            }
            
            var claims = resourceOwner.Claims == null ? new List<Claim>() : resourceOwner.Claims.ToList();
            claims.Add(new Claim(ClaimTypes.AuthenticationInstant,
                DateTimeOffset.UtcNow.ConvertToUnixTimestamp().ToString(CultureInfo.InvariantCulture),
                ClaimValueTypes.Integer));
            return new LocalOpenIdAuthenticationResult
            {
                ActionResult = await _authenticateHelper.ProcessRedirection(authorizationParameter,
                                code,
                                resourceOwner.Id,
                                claims),
                Claims = claims
            };
        }
    }
}
