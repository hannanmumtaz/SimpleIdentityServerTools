using SimpleIdentityServer.DocumentManagement.Core.Exceptions;
using SimpleIdentityServer.DocumentManagement.Core.Parameters;
using System;

namespace SimpleIdentityServer.DocumentManagement.Core.Validators
{
    public interface IGenerateConfirmationLinkParameterValidator
    {
        void Check(GenerateConfirmationLinkParameter gnerateConfirmationLinkParameter);
    }

    internal sealed class GenerateConfirmationLinkParameterValidator : IGenerateConfirmationLinkParameterValidator
    {
        public void Check(GenerateConfirmationLinkParameter generateConfirmationLinkParameter)
        {
            if (generateConfirmationLinkParameter == null)
            {
                throw new ArgumentNullException(nameof(generateConfirmationLinkParameter));
            }

            if(string.IsNullOrWhiteSpace(generateConfirmationLinkParameter.Subject))
            {
                throw new BaseDocumentManagementApiException(ErrorCodes.InvalidRequest, ErrorDescriptions.SubjectIsMissing);
            }
        }
    }
}
