using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Client.Tests.Middlewares
{
    public class UserStore
    {
        private static UserStore _instance;

        private UserStore()
        {

        }

        public static UserStore Instance()
        {
            if (_instance==null)
            {
                _instance = new UserStore();
            }

            return _instance;
        }

        public string Subject { get; set; }
    }

    public class FakeUserInfoIntrospectionHandler : AuthenticationHandler<FakeUserInfoIntrospectionOptions>
    {
        public FakeUserInfoIntrospectionHandler(IOptionsMonitor<FakeUserInfoIntrospectionOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var instance = UserStore.Instance();
            if (string.IsNullOrWhiteSpace(instance.Subject))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var claims = new List<Claim>();
            claims.Add(new Claim("sub", UserStore.Instance().Subject));
            var claimsIdentity = new ClaimsIdentity(claims, FakeUserInfoIntrospectionOptions.AuthenticationScheme);
            var authenticationTicket = new AuthenticationTicket(
                                             new ClaimsPrincipal(claimsIdentity),
                                             new Microsoft.AspNetCore.Authentication.AuthenticationProperties(),
                                             FakeUserInfoIntrospectionOptions.AuthenticationScheme);
            return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
        }
    }
}
