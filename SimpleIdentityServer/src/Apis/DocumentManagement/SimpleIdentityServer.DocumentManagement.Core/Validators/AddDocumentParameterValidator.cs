using SimpleIdentityServer.DocumentManagement.Core.Exceptions;
using SimpleIdentityServer.DocumentManagement.Core.Parameters;
using System;

namespace SimpleIdentityServer.DocumentManagement.Core.Validators
{
    public interface IAddDocumentParameterValidator
    {
        void Check(AddDocumentParameter parameter);
    }

    internal sealed class AddDocumentParameterValidator : IAddDocumentParameterValidator
    {
        public void Check(AddDocumentParameter parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            if (string.IsNullOrWhiteSpace(parameter.Id))
            {
                throw new BaseDocumentManagementApiException(ErrorCodes.InvalidRequest, string.Format(ErrorDescriptions.ParameterIsMissing, "id"));
            }

            if(string.IsNullOrWhiteSpace(parameter.DisplayName))
            {
                throw new BaseDocumentManagementApiException(ErrorCodes.InvalidRequest, string.Format(ErrorDescriptions.ParameterIsMissing, "display_name"));
            }

            if (string.IsNullOrWhiteSpace(parameter.Subject))
            {
                throw new BaseDocumentManagementApiException(ErrorCodes.InvalidRequest, ErrorDescriptions.SubjectIsMissing);
            }
        }
    }
}
