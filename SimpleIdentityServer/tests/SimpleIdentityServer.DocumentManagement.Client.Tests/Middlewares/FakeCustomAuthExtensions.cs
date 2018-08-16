using Microsoft.AspNetCore.Authentication;
using System;

namespace SimpleIdentityServer.DocumentManagement.Client.Tests.Middlewares
{
    public static class FakeCustomAuthExtensions
    {
        public static AuthenticationBuilder AddFakeUserInfoIntrospection(this AuthenticationBuilder builder, Action<FakeUserInfoIntrospectionOptions> configureOptions)
        {
            return builder.AddScheme<FakeUserInfoIntrospectionOptions, FakeUserInfoIntrospectionHandler>(FakeUserInfoIntrospectionOptions.AuthenticationScheme, configureOptions);
        }
    }
}
