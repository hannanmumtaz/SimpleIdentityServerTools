using SimpleIdentityServer.DocumentManagement.Core.Exceptions;
using SimpleIdentityServer.DocumentManagement.Core.Parameters;
using System;

namespace SimpleIdentityServer.DocumentManagement.Core.Validators
{
    public interface IUpdateOfficeDocumentParameterValidator
    {
        void Check(UpdateOfficeDocumentParameter updateOfficeDocumentParameter);
    }

    internal sealed class UpdateOfficeDocumentParameterValidator : IUpdateOfficeDocumentParameterValidator
    {
        public void Check(UpdateOfficeDocumentParameter updateOfficeDocumentParameter)
        {
            if(updateOfficeDocumentParameter == null)
            {
                throw new ArgumentNullException(nameof(updateOfficeDocumentParameter));
            }
            
            if (string.IsNullOrWhiteSpace(updateOfficeDocumentParameter.Subject))
            {
                throw new BaseDocumentManagementApiException(ErrorCodes.InvalidRequest, ErrorDescriptions.SubjectIsMissing);
            }

            if (updateOfficeDocumentParameter.Permissions == null)
            {
                throw new BaseDocumentManagementApiException(ErrorCodes.InvalidRequest, string.Format(ErrorDescriptions.ParameterIsMissing, "permissions"));
            }
        }
    }
}
