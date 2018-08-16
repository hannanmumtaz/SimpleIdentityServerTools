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
        public string IdentityToken { get; private set; }
        public JwsPayload JwsPayload { get; private set; }

        public event EventHandler Authenticated;
        public event EventHandler Disconnected;

        public void Authenticate(IEnumerable<KeyValuePair<string, string>> dic)
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
                JwsPayload = _jwsParser.GetPayload(IdentityToken);
                if (Authenticated != null)
                {
                    Authenticated(this, EventArgs.Empty);
                }
            }
        }

        public void Disconnect()
        {
            AccessToken = null;
            IdentityToken = null;
            JwsPayload = null;
            if (Disconnected != null)
            {
                Disconnected(this, EventArgs.Empty);
            }
        }
    }
}
