using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Authenticate.Eid.Core.Login;
using SimpleIdentityServer.Authenticate.Eid.Core.Login.Actions;
using System;

namespace SimpleIdentityServer.Authenticate.Eid.Core
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
