using SimpleIdentityServer.DocumentManagement.Core.Exceptions;
using SimpleIdentityServer.DocumentManagement.Core.Parameters;
using System;

namespace SimpleIdentityServer.DocumentManagement.Core.Validators
{
    public interface IValidateConfirmationLinkParameterValidator
    {
        void Check(ValidateConfirmationLinkParameter parameter);
    }

    internal sealed class ValidateConfirmationLinkParameterValidator : IValidateConfirmationLinkParameterValidator
    {
        public void Check(ValidateConfirmationLinkParameter parameter)
        {
            if(parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            if (string.IsNullOrWhiteSpace(parameter.ConfirmationCode))
            {
                throw new BaseDocumentManagementApiException(ErrorCodes.InvalidRequest, string.Format(ErrorDescriptions.ParameterIsMissing, "confirmation_code"));
            }

            if (string.IsNullOrWhiteSpace(parameter.Subject))
            {
                throw new BaseDocumentManagementApiException(ErrorCodes.InvalidRequest, ErrorDescriptions.SubjectIsMissing);
            }
        }
    }
}
