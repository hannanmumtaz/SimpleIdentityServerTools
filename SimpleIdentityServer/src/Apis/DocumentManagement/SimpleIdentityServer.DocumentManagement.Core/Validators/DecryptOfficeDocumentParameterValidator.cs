using SimpleIdentityServer.DocumentManagement.Core.Exceptions;
using SimpleIdentityServer.DocumentManagement.Core.Parameters;
using System;

namespace SimpleIdentityServer.DocumentManagement.Core.Validators
{
    public interface IDecryptOfficeDocumentParameterValidator
    {
        void Check(DecryptOfficeDocumentParameter decryptOfficeDocumentParameter);
    }

    internal sealed class DecryptOfficeDocumentParameterValidator : IDecryptOfficeDocumentParameterValidator
    {
        public void Check(DecryptOfficeDocumentParameter decryptOfficeDocumentParameter)
        {
            if (decryptOfficeDocumentParameter == null)
            {
                throw new ArgumentNullException(nameof(decryptOfficeDocumentParameter));
            }

            if (string.IsNullOrWhiteSpace(decryptOfficeDocumentParameter.DocumentId))
            {
                throw new BaseDocumentManagementApiException(ErrorCodes.InvalidRequest, string.Format(ErrorDescriptions.ParameterIsMissing, "document_id"));
            }

            if (string.IsNullOrWhiteSpace(decryptOfficeDocumentParameter.Kid))
            {
                throw new BaseDocumentManagementApiException(ErrorCodes.InvalidRequest, string.Format(ErrorDescriptions.ParameterIsMissing, "kid"));
            }

            if (string.IsNullOrWhiteSpace(decryptOfficeDocumentParameter.Credentials))
            {
                throw new BaseDocumentManagementApiException(ErrorCodes.InvalidRequest, string.Format(ErrorDescriptions.ParameterIsMissing, "credentials"));
            }
        }
    }
}
