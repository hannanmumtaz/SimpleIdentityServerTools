using SimpleIdentityServer.Client;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;
using SimpleIdentityServer.DocumentManagement.Core.Exceptions;
using SimpleIdentityServer.DocumentManagement.Core.Parameters;
using SimpleIdentityServer.DocumentManagement.Core.Repositories;
using SimpleIdentityServer.DocumentManagement.Core.Validators;
using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Core.OfficeDocuments.Actions
{
    public interface IDecryptOfficeDocumentAction
    {
        Task<DecryptedResponse> Execute(DecryptOfficeDocumentParameter decryptOfficeDocumentParameter, string accessToken, AuthenticateParameter authenticateParameter);
    }

    internal sealed class DecryptOfficeDocumentAction : IDecryptOfficeDocumentAction
    {
        private readonly IJsonWebKeyRepository _jsonWebKeyRepository;
        private readonly IGetOfficeDocumentAction _getOfficeDocumentAction;
        private readonly IDecryptOfficeDocumentParameterValidator _decryptOfficeDocumentParameterValidator;
        private readonly IIdentityServerClientFactory _identityServerClientFactory;

        public DecryptOfficeDocumentAction(IJsonWebKeyRepository jsonWebKeyRepository, IGetOfficeDocumentAction getOfficeDocumentAction,
            IIdentityServerClientFactory identityServerClientFactory, IDecryptOfficeDocumentParameterValidator decryptOfficeDocumentParameterValidator)
        {
            _jsonWebKeyRepository = jsonWebKeyRepository;
            _getOfficeDocumentAction = getOfficeDocumentAction;
            _identityServerClientFactory = identityServerClientFactory;
            _decryptOfficeDocumentParameterValidator = decryptOfficeDocumentParameterValidator;
        }

        public async Task<DecryptedResponse> Execute(DecryptOfficeDocumentParameter decryptOfficeDocumentParameter, string accessToken, AuthenticateParameter authenticateParameter)
        {
            if (decryptOfficeDocumentParameter == null)
            {
                throw new ArgumentNullException(nameof(decryptOfficeDocumentParameter));
            }

            if (authenticateParameter == null)
            {
                throw new ArgumentNullException(nameof(authenticateParameter));
            }

            _decryptOfficeDocumentParameterValidator.Check(decryptOfficeDocumentParameter);
            await _getOfficeDocumentAction.Execute(decryptOfficeDocumentParameter.DocumentId);
            var jsonWebKey = await _jsonWebKeyRepository.Get(decryptOfficeDocumentParameter.Kid);
            if (jsonWebKey == null)
            {
                throw new BaseDocumentManagementApiException(ErrorCodes.InvalidRequest, string.Format(ErrorDescriptions.TheJsonWebKeyDoesntExist, decryptOfficeDocumentParameter.Kid));
            }

            var payload = Convert.FromBase64String(decryptOfficeDocumentParameter.Credentials);
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
