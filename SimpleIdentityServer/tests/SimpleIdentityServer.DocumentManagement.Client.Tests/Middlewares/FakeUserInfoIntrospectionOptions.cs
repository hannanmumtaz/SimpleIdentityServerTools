using Microsoft.AspNetCore.Authentication;

namespace SimpleIdentityServer.DocumentManagement.Client.Tests.Middlewares
{
    public class FakeUserInfoIntrospectionOptions : AuthenticationSchemeOptions
    {
        public const string AuthenticationScheme = "UserInfoIntrospection";
    }
}
