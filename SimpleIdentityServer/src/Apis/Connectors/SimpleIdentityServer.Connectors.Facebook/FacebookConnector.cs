using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Connectors.Common;
using SimpleIdentityServer.Connectors.Common.Extensions;
using System;
using System.Collections.Generic;

namespace SimpleIdentityServer.Connectors.Facebook
{
    public class FacebookConnector : IConnector
    {
        private const string CookieName = "CookieName";
        private const string ClientId = "ClientId";
        private const string ClientSecret = "ClientSecret";
        private const string Scopes = "Scopes";

        private IEnumerable<string> ParameterNames = new List<string>
        {
            CookieName,
            ClientId,
            ClientSecret,
            Scopes
        };

        public void Configure(IServiceCollection services, IDictionary<string, string> options)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var cookieName = options.TryGetStr(CookieName);
            if (string.IsNullOrWhiteSpace(cookieName))
            {
                cookieName = Constants.DEFAULT_COOKIE_NAME;
            }

            services.AddAuthentication(cookieName)
                .AddCookie(cookieName)
                .AddFacebook(opts =>
                {
                    opts.ClientId = options.TryGetStr(ClientId);
                    opts.ClientSecret = options.TryGetStr(ClientSecret);
                    opts.SignInScheme = cookieName;
                    var scopes = options.TryGetStrArr(Scopes);
                    if (scopes != null)
                    {
                        foreach(var scope  in scopes)
                        {
                            opts.Scope.Add(scope);
                        }
                    }
                });
        }

        public IEnumerable<string> GetParameters()
        {
            return ParameterNames;
        }
    }
}
