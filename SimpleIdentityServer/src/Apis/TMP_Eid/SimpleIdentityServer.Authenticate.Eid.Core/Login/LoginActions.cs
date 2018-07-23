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
        Task<ResourceOwner> LocalAuthenticate(LocalAuthenticateParameter parameter, string imagePath, string hostUrl);
        Task<LocalOpenIdAuthenticationResult> OpenIdLocalAuthenticate(LocalAuthenticateParameter localAuthenticateParameter, AuthorizationParameter authorizationParameter, string code,
            string imagePath, string hostUrl);
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

        public Task<ResourceOwner> LocalAuthenticate(LocalAuthenticateParameter parameter, string imagePath, string hostUrl)
        {
            return _localAuthenticateAction.Execute(parameter, imagePath, hostUrl);
        }

        public Task<LocalOpenIdAuthenticationResult> OpenIdLocalAuthenticate(LocalAuthenticateParameter localAuthenticateParameter, AuthorizationParameter authorizationParameter, 
            string code, string imagePath, string hostUrl)
        {
            return _openIdLocalAuthenticateAction.Execute(localAuthenticateParameter, authorizationParameter, code, imagePath, hostUrl);
        }
    }
}
