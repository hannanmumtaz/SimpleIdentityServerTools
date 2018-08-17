using SimpleIdentityServer.Client;
using SimpleIdentityServer.Core.Common.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WordAccessManagementAddin.Stores
{
    internal class AccessTokenStore
    {
        private class StoredToken
        {
            public StoredToken()
            {
                Scopes = new List<string>();
            }

            public string Url { get; set; }
            public IEnumerable<string> Scopes { get; set; }
            public GrantedTokenResponse GrantedToken { get; set; }
            public DateTime ExpirationDateTime { get; set; }
        }

        private readonly List<StoredToken> _tokens;
        private readonly IIdentityServerClientFactory _identityServerClientFactory;
        private static AccessTokenStore _instance;

        private AccessTokenStore()
        {
            _tokens = new List<StoredToken>();
            _identityServerClientFactory = new IdentityServerClientFactory();
        }

        public static AccessTokenStore Instance()
        {
            if (_instance == null)
            {
                _instance = new AccessTokenStore();
            }

            return _instance;
        }

        public async Task<GrantedTokenResponse> GetToken(string url, string clientId, string clientSecret, IEnumerable<string> scopes)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            if (string.IsNullOrWhiteSpace(clientSecret))
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            if (scopes == null)
            {
                throw new ArgumentNullException(nameof(scopes));
            }

            var token = _tokens.FirstOrDefault(t => t.Url == url && scopes.Count() == t.Scopes.Count() && scopes.All(s => t.Scopes.Contains(s)));
            if (token != null)
            {
                if (DateTime.UtcNow < token.ExpirationDateTime)
                {
                    return token.GrantedToken;
                }

                _tokens.Remove(token);
            }

            var grantedToken = await _identityServerClientFactory.CreateAuthSelector()
                .UseClientSecretPostAuth(clientId, clientSecret)
                .UseClientCredentials(scopes.ToArray())
                .ResolveAsync(url)
                .ConfigureAwait(false);
            _tokens.Add(new StoredToken
            {
                GrantedToken = grantedToken.Content,
                ExpirationDateTime = DateTime.UtcNow.AddSeconds(grantedToken.Content.ExpiresIn),
                Scopes = scopes,
                Url = url
            });

            return grantedToken.Content;
        }
    }
}
