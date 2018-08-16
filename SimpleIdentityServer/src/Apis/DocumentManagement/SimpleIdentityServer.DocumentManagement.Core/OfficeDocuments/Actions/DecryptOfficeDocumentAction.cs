using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;
using SimpleIdentityServer.DocumentManagement.Core.Exceptions;
using SimpleIdentityServer.DocumentManagement.Core.Repositories;
using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Core.OfficeDocuments.Actions
{
    public interface IDecryptOfficeDocumentAction
    {
        Task<DecryptedResponse> Execute(string kid, string credentials);
    }

    internal sealed class DecryptOfficeDocumentAction : IDecryptOfficeDocumentAction
    {
        private readonly IJsonWebKeyRepository _jsonWebKeyRepository;

        public DecryptOfficeDocumentAction(IJsonWebKeyRepository jsonWebKeyRepository)
        {
            _jsonWebKeyRepository = jsonWebKeyRepository;
        }

        public async Task<DecryptedResponse> Execute(string kid, string credentials)
        {
            if (string.IsNullOrWhiteSpace(kid))
            {
                throw new ArgumentNullException(nameof(kid));
            }

            if (string.IsNullOrWhiteSpace(credentials))
            {
                throw new ArgumentNullException(nameof(credentials));
            }

            var jsonWebKey = await _jsonWebKeyRepository.Get(kid);
            if (jsonWebKey == null)
            {
                throw new BaseDocumentManagementApiException("invalid_request", $"the json web key {kid} doesn't exist");
            }

            var payload = Convert.FromBase64String(credentials);
            byte[] decryptedPayload = null;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                using (var provider = new RSACryptoServiceProvider())
                {
                    provider.FromXmlString(jsonWebKey.SerializedKey);
                    decryptedPayload = provider.Decrypt(payload, true);
                }
            }
            else
            {
                using (var rsa = new RSAOpenSsl())
                {
                    rsa.FromXmlString(jsonWebKey.SerializedKey);
                    decryptedPayload = rsa.Decrypt(payload, RSAEncryptionPadding.OaepSHA1);
                }
            }

            var decryptedContent = Encoding.UTF8.GetString(decryptedPayload);
            var splitted = decryptedContent.Split('.');
            return new DecryptedResponse
            {
                Password = splitted[0],
                Salt = splitted[1]
            };
        }
    }
}
