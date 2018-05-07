using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Eid.OpenId.Core.Login;
using SimpleIdentityServer.Eid.OpenId.Core.Login.Actions;
using System;

namespace SimpleIdentityServer.Eid.OpenId.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEidOpenidCore(this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            serviceCollection.AddTransient<ILoginActions, LoginActions>();
            serviceCollection.AddTransient<ILocalAuthenticateAction, LocalAuthenticateAction>();
            serviceCollection.AddTransient<IOpenIdLocalAuthenticateAction, OpenIdLocalAuthenticateAction>();
            return serviceCollection;
        }
    }
}
