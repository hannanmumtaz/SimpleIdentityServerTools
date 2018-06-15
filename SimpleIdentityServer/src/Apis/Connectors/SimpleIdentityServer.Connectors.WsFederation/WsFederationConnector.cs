using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Connectors.Common;
using SimpleIdentityServer.Connectors.Common.Extensions;
using System;
using System.Collections.Generic;

namespace SimpleIdentityServer.Connectors.MicrosoftAccount
{
    public class WsFederationConnector : IConnector
    {
        private const string CallbackPath = "CallbackPath";
        private const string Wtrealm = "Wtrealm";
        private const string MetadataAddress = "MetadataAddress";
        private const string CookieName = "CookieName";

        private IEnumerable<string> ParameterNames = new List<string>
        {
            CallbackPath,
            Wtrealm,
            MetadataAddress,
            CookieName
        };

        public void Configure(AuthenticationBuilder authBuilder, IDictionary<string, string> options)
        {
            if (authBuilder == null)
            {
                throw new ArgumentNullException(nameof(authBuilder));
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

            authBuilder.AddWsFederation(opts =>
            {
                opts.CallbackPath = options.TryGetStr(CallbackPath);
                opts.Wtrealm = options.TryGetStr(Wtrealm);
                opts.MetadataAddress = options.TryGetStr(MetadataAddress);
                opts.SignInScheme = cookieName;
            });
        }

        public IEnumerable<string> GetParameters()
        {
            return ParameterNames;
        }
    }
}
