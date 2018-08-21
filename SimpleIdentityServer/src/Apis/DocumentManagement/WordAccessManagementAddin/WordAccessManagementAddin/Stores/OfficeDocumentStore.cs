using Newtonsoft.Json;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.Core.Common.DTOs.Responses;
using SimpleIdentityServer.DocumentManagement.Client;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;
using SimpleIdentityServer.Uma.Common.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WordAccessManagementAddin.Stores
{
    internal class OfficeDocumentStore
    {
        private class StoredUmaAccessToken
        {
            public GrantedTokenResponse GrantedToken { get; set; }
            public DateTime ExpirationDateTime { get; set; }
            public string UmaResourceId { get; set; }
        }

        private class StoredOfficeDocument
        {
            public string DocumentId { get; set; }
            public string UmaResourceId { get; set; }
        }

        private static string _fileName = string.Format(Constants.FilePatternName, "{0}_officeDocumentStore");
        private static OfficeDocumentStore _instance;
        private readonly IdentityServerUmaClientFactory _identityServerUmaClientFactory;
        private readonly IdentityServerClientFactory _identityServerClientFactory;
        private readonly DocumentManagementFactory _documentManagementFactory;
        private readonly AccessTokenStore _accessTokenStore;
        private readonly AuthenticationStore _authenticationStore;
        private readonly List<StoredUmaAccessToken> _tokens;
        private readonly List<StoredOfficeDocument> _documents;

        private OfficeDocumentStore()
        {
            _identityServerUmaClientFactory = new IdentityServerUmaClientFactory();
            _identityServerClientFactory = new IdentityServerClientFactory();
            _documentManagementFactory = new DocumentManagementFactory();
            _accessTokenStore = AccessTokenStore.Instance();
            _authenticationStore = AuthenticationStore.Instance();
            _tokens = new List<StoredUmaAccessToken>();
            _documents = new List<StoredOfficeDocument>();
        }

        public event EventHandler Decrypted;

        public static OfficeDocumentStore Instance()
        {
            if (_instance == null)
            {
                _instance = new OfficeDocumentStore();
            }

            return _instance;
        }

        public void StoreDecryption(string documentId, DecryptedResponse decryptedResponse)
        {
            if(decryptedResponse == null)
            {
                throw new ArgumentNullException(nameof(decryptedResponse));
            }

            var fullPath = GetFullPath(documentId);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            File.WriteAllText(fullPath, JsonConvert.SerializeObject(decryptedResponse));
        }

        public DecryptedResponse RestoreDecryption(string documentId)
        {
            var fullPath = GetFullPath(documentId);
            if (!File.Exists(fullPath))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<DecryptedResponse>(File.ReadAllText(fullPath));
        }

        public void ResetDecryption(string documentId)
        {
            var fullPath = GetFullPath(documentId);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        public void LaunchDecryption()
        {
            if(Decrypted != null)
            {
                Decrypted(this, EventArgs.Empty);
            }
        }

        public async Task<string> GetUmaResourceId(string documentId)
        {            
            var document = _documents.FirstOrDefault(d => d.DocumentId == documentId);
            if (document != null)
            {
                return document.UmaResourceId;
            }

            var getDocumentResponse = await _documentManagementFactory.GetOfficeDocumentClient().GetResolve(documentId, Constants.DocumentApiConfiguration, AuthenticationStore.Instance().AccessToken).ConfigureAwait(false);           
            _documents.Add(new StoredOfficeDocument
            {
                DocumentId = documentId,
                UmaResourceId = getDocumentResponse.OfficeDocument.UmaResourceId
            });

            return getDocumentResponse.OfficeDocument.UmaResourceId;
        }

        public async Task<GrantedTokenResponse> GetOfficeDocumentAccessTokenViaUmaGrantType(string umaResourceId)
        {
            var token = _tokens.FirstOrDefault(t => t.UmaResourceId == umaResourceId);
            if (token != null)
            {
                if (DateTime.UtcNow < token.ExpirationDateTime)
                {
                    return token.GrantedToken;
                }

                _tokens.Remove(token);
            }

            var tokenResponse = await _accessTokenStore.GetToken(Constants.UmaWellKnownConfiguration, Constants.ClientId, Constants.ClientSecret, new[] { "uma_protection" }).ConfigureAwait(false);
            if (tokenResponse == null)
            {
                return null;
            }

            var permissionResponse = await _identityServerUmaClientFactory.GetPermissionClient().AddByResolution(new PostPermission
            {
                ResourceSetId = umaResourceId,
                Scopes = new[] { "read" }
            }, Constants.UmaWellKnownConfiguration, tokenResponse.AccessToken).ConfigureAwait(false);
            if (permissionResponse.ContainsError)
            {
                return null;
            }
            
            var grantedToken = await _identityServerClientFactory.CreateAuthSelector().UseClientSecretPostAuth(Constants.ClientId, Constants.ClientSecret).UseTicketId(permissionResponse.Content.TicketId, _authenticationStore.IdentityToken)
                .ResolveAsync(Constants.UmaWellKnownConfiguration)
                .ConfigureAwait(false);
            if (grantedToken.ContainsError)
            {
                return null;
            }

            var result = new StoredUmaAccessToken
            {
                ExpirationDateTime = DateTime.UtcNow.AddSeconds(grantedToken.Content.ExpiresIn),
                GrantedToken = grantedToken.Content,
                UmaResourceId = umaResourceId
            };
            _tokens.Add(result);
            return grantedToken.Content;
        }

        private static string GetFullPath(string documentId)
        {
            return Path.Combine(Path.GetTempPath(), string.Format(_fileName, documentId));
        }
    }
}