using Newtonsoft.Json;
using SimpleIdentityServer.Core.Common;
using SimpleIdentityServer.Core.Jwt.Signature;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WordAccessManagementAddin.Stores
{
    public class AuthenticationStore
    {
        private class StoredAuthentication
        {
            public string AccessToken { get; set; }
            public string IdentityToken { get; set; }
        }

        private static string _fileName = string.Format(Constants.FilePatternName, "authenticationStore");
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

        /// <summary>
        /// Restore the file.
        /// </summary>
        public void Restore()
        {
            var storedAuthentication = GetRestoredFile();
            if (storedAuthentication == null)
            {
                return;
            }

            // 1. RESTORE THE FILE
            // 2. CHECK THE ACCESS_TOKEN
            Authenticate(storedAuthentication.AccessToken, storedAuthentication.IdentityToken);
        }

        /// <summary>
        /// Authenticate the user.
        /// </summary>
        /// <param name="dic"></param>
        public void Authenticate(IEnumerable<KeyValuePair<string, string>> dic)
        {
            if (dic == null)
            {
                throw new ArgumentNullException(nameof(dic));
            }

            string accessToken = null,
                identityToken = null;
            var accessTokenKvp= dic.FirstOrDefault(x => x.Key == "access_token");
            var idTokenTokenKvp = dic.FirstOrDefault(x => x.Key == "id_token");
            if (!default(KeyValuePair<string, string>).Equals(accessTokenKvp))
            {
                accessToken = accessTokenKvp.Value;
            }

            if (!default(KeyValuePair<string, string>).Equals(idTokenTokenKvp))
            {
                identityToken = idTokenTokenKvp.Value;
            }

            Authenticate(accessToken, identityToken);
        }

        /// <summary>
        /// Authenticate the user.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="identityToken"></param>
        public void Authenticate(string accessToken, string identityToken)
        {
            AccessToken = accessToken;
            IdentityToken = identityToken;
            Persist(new StoredAuthentication
            {
                AccessToken = accessToken,
                IdentityToken = identityToken
            });
            if (!string.IsNullOrWhiteSpace(identityToken))
            {
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

            RemoveRestoredFile();
        }

        private static StoredAuthentication GetRestoredFile()
        {
            var fullPath = GetFullPath();
            if (!File.Exists(fullPath))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<StoredAuthentication>(File.ReadAllText(fullPath));
        }

        private static void RemoveRestoredFile()
        {
            var fullPath = GetFullPath();
            if (!File.Exists(fullPath))
            {
                return;
            }

            File.Delete(fullPath);
        }

        private static void Persist(StoredAuthentication storedAuthentication)
        {
            var json = JsonConvert.SerializeObject(storedAuthentication);
            var fullPath = GetFullPath();
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            File.WriteAllText(fullPath, json);
        }

        private static string GetFullPath()
        {
            return Path.Combine(Path.GetTempPath(), _fileName);
        }
    }
}