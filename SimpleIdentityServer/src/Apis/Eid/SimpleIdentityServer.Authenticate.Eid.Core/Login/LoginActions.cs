using SimpleIdentityServer.Core.Common.Models;
using SimpleIdentityServer.Core.Parameters;
using SimpleIdentityServer.Core.WebSite.Authenticate.Actions;
using SimpleIdentityServer.Authenticate.Eid.Core.Login.Actions;
using SimpleIdentityServer.Authenticate.Eid.Core.Parameters;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Authenticate.Eid.Core.Login
{
    public interface ILoginActions
    {
        Task<ResourceOwner> LocalAuthenticate(LocalAuthenticateParameter parameter);
        Task<LocalOpenIdAuthenticationResult> OpenIdLocalAuthenticate(LocalAuthenticateParameter localAuthenticateParameter, AuthorizationParameter authorizationParameter, string code);
    }

    internal sealed class LoginActions : ILoginActions
    {
        private readonly ILocalAuthenticateAction _localAuthenticateAction;
        private readonly IOpenIdLocalAuthenticateAction _openIdLocalAuthenticateAction;

        public LoginActions(ILocalAuthenticateAction localAuthenticateAction, IOpenIdLocalAuthenticateAction openIdLocalAuthenticateAction)
        {
            _localAuthenticateAction = localAuthenticateAction;
            _openIdLocalAuthenticateAction = openIdLocalAuthenticateAction;
        }

        public Task<ResourceOwner> LocalAuthenticate(LocalAuthenticateParameter parameter)
        {
            return _localAuthenticateAction.Execute(parameter);
        }

        public Task<LocalOpenIdAuthenticationResult> OpenIdLocalAuthenticate(LocalAuthenticateParameter localAuthenticateParameter, AuthorizationParameter authorizationParameter, string code)
        {
            return _openIdLocalAuthenticateAction.Execute(localAuthenticateParameter, authorizationParameter, code);
        }
    }
}
