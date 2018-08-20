using SimpleIdentityServer.DocumentManagement.Core.Exceptions;
using SimpleIdentityServer.DocumentManagement.Core.Parameters;
using SimpleIdentityServer.DocumentManagement.Core.Repositories;
using SimpleIdentityServer.DocumentManagement.Core.Validators;
using SimpleIdentityServer.DocumentManagement.Store;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Core.OfficeDocuments.Actions
{
    public interface IDeleteOfficeDocumentConfirmationCodeAction
    {
        Task<bool> Execute(DeleteConfirmationCodeParameter parameter);
    }

    internal sealed class DeleteOfficeDocumentConfirmationCodeAction : IDeleteOfficeDocumentConfirmationCodeAction
    {
        private readonly IOfficeDocumentConfirmationLinkStore _officeDocumentConfirmationLinkStore;
        private readonly IDeleteConfirmationCodeParameterValidator _deleteConfirmationCodeParameterValidator;
        private readonly IOfficeDocumentRepository _officeDocumentRepository;

        public DeleteOfficeDocumentConfirmationCodeAction(IOfficeDocumentConfirmationLinkStore officeDocumentConfirmationLinkStore, IDeleteConfirmationCodeParameterValidator deleteConfirmationCodeParameterValidator,
            IOfficeDocumentRepository officeDocumentRepository)
        {
            _officeDocumentConfirmationLinkStore = officeDocumentConfirmationLinkStore;
            _deleteConfirmationCodeParameterValidator = deleteConfirmationCodeParameterValidator;
            _officeDocumentRepository = officeDocumentRepository;
        }

        public async Task<bool> Execute(DeleteConfirmationCodeParameter parameter)
        {
            if(parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            _deleteConfirmationCodeParameterValidator.Check(parameter);
            var confirmationCode = await _officeDocumentConfirmationLinkStore.Get(parameter.ConfirmationCode);
            if (confirmationCode == null)
            {
                throw new ConfirmationCodeNotFoundException();
            }

            var officeDocument = await _officeDocumentRepository.Get(confirmationCode.DocumentId);
            if (officeDocument == null)
            {
                throw new DocumentNotFoundException();
            }

            if(officeDocument.Subject != parameter.Subject)
            {
                throw new NotAuthorizedException(ErrorCodes.InternalError, ErrorDescriptions.NotAuthorized);
            }

            if(!await _officeDocumentConfirmationLinkStore.Remove(parameter.ConfirmationCode))
            {
                throw new BaseDocumentManagementApiException(ErrorCodes.InternalError, ErrorDescriptions.CannotRemoveConfirmationCode);
            }

            return true;
        }
    }
}
