using SimpleIdentityServer.DocumentManagement.Core.Exceptions;
using SimpleIdentityServer.DocumentManagement.Core.Parameters;
using System;

namespace SimpleIdentityServer.DocumentManagement.Core.Validators
{
    public interface IGetAllConfirmationLinksValidator
    {
        void Check(GetAllConfirmationLinksParameter parameter);
    }

    internal sealed class GetAllConfirmationLinksValidator : IGetAllConfirmationLinksValidator
    {
        public void Check(GetAllConfirmationLinksParameter parameter)
        {
            if(parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }
            
            if (string.IsNullOrWhiteSpace(parameter.Subject))
            {
                throw new BaseDocumentManagementApiException(ErrorCodes.InvalidRequest, ErrorDescriptions.SubjectIsMissing);
            }
        }
    }
}