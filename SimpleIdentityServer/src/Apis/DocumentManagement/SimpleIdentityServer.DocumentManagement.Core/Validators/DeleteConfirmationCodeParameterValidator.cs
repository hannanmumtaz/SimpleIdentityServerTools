using SimpleIdentityServer.DocumentManagement.Core.Exceptions;
using SimpleIdentityServer.DocumentManagement.Core.Parameters;
using System;

namespace SimpleIdentityServer.DocumentManagement.Core.Validators
{
    public interface IDeleteConfirmationCodeParameterValidator
    {
        void Check(DeleteConfirmationCodeParameter deleteConfirmationCodeParameter);
    }

    internal sealed class DeleteConfirmationCodeParameterValidator : IDeleteConfirmationCodeParameterValidator
    {
        public void Check(DeleteConfirmationCodeParameter deleteConfirmationCodeParameter)
        {
            if(deleteConfirmationCodeParameter == null)
            {
                throw new ArgumentNullException(nameof(deleteConfirmationCodeParameter));
            }

            if (string.IsNullOrWhiteSpace(deleteConfirmationCodeParameter.Subject))
            {
                throw new BaseDocumentManagementApiException(ErrorCodes.InvalidRequest, ErrorDescriptions.SubjectIsMissing);
            }
        }
    }
}
