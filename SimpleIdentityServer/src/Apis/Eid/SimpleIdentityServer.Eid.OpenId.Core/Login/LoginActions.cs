using SimpleIdentityServer.Core.Models;
using SimpleIdentityServer.Eid.OpenId.Core.Login.Actions;
using SimpleIdentityServer.Eid.OpenId.Core.Parameters;

namespace SimpleIdentityServer.Eid.OpenId.Core.Login
{
    public interface ILoginActions
    {
        ResourceOwner LocalAuthenticate(LocalAuthenticateParameter parameter);
    }

    internal sealed class LoginActions : ILoginActions
    {
        private readonly ILocalAuthenticateAction _localAuthenticateAction;

        public LoginActions(ILocalAuthenticateAction localAuthenticateAction)
        {
            _localAuthenticateAction = localAuthenticateAction;
        }

        public ResourceOwner LocalAuthenticate(LocalAuthenticateParameter parameter)
        {
            return _localAuthenticateAction.Execute(parameter);
        }
    }
}
