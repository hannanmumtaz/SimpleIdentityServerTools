using SimpleIdentityServer.Core.Common;
using SimpleIdentityServer.Core.Jwt.Signature;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WordAccessManagementAddin.Stores
{
    public class AuthenticationStore
    {
        private static AuthenticationStore _instance;
        private IJwsParser _jwsParser;
        private string _identityToken;

        private AuthenticationStore()
        {
            _jwsParser = new JwsParser(null);
        }

        public static AuthenticationStore Instance()
        {
            if (_instance == null)
            {
                _instance = new AuthenticationStore();
            }

            return _instance;
        }

        public string AccessToken { get; private set; }
        public string IdentityToken {
            get
            {
                return _identityToken;
            }
            set
            {
                if (_identityToken != value)
                {
                    _identityToken = value;
                    JwsPayload = _jwsParser.GetPayload(_identityToken);
                    if (Authenticated != null)
                    {
                        Authenticated(this, EventArgs.Empty);
                    }
                }
            }
        }
        public JwsPayload JwsPayload { get; private set; }
        public event EventHandler Authenticated;

        public void Update(IEnumerable<KeyValuePair<string, string>> dic)
        {
            if (dic == null)
            {
                throw new ArgumentNullException(nameof(dic));
            }
            
            var accessTokenKvp= dic.FirstOrDefault(x => x.Key == "access_token");
            var idTokenTokenKvp = dic.FirstOrDefault(x => x.Key == "id_token");
            if (!default(KeyValuePair<string, string>).Equals(accessTokenKvp))
            {
                AccessToken = accessTokenKvp.Value;
            }

            if (!default(KeyValuePair<string, string>).Equals(idTokenTokenKvp))
            {
                IdentityToken = idTokenTokenKvp.Value;
            }
        }
    }
}
