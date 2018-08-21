using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;
using SimpleIdentityServer.DocumentManagement.Core.Aggregates;
using SimpleIdentityServer.DocumentManagement.Core.OfficeDocuments.Actions;
using SimpleIdentityServer.DocumentManagement.Core.Parameters;
using SimpleIdentityServer.DocumentManagement.Store;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Core.OfficeDocuments
{
    public interface IOfficeDocumentActions
    {
        Task<bool> Add(string openidWellKnownConfiguration, AddDocumentParameter document, AuthenticateParameter authenticateParameter);
        Task<OfficeDocumentAggregate> Get(string documentId);
        Task<DecryptedResponse> Decrypt(DecryptOfficeDocumentParameter decryptOfficeDocumentParameter, string accessToken, AuthenticateParameter authenticateParameter);
        Task<IEnumerable<OfficeDocumentPermissionResponse>> GetPermissions(string documentId, string subject, AuthenticateParameter authenticateParameter);
        Task<string> GenerateConfirmationLink(string documentId, GenerateConfirmationLinkParameter generateConfirmationCodeParameter);
        Task<bool> ValidateConfirmationLink(string wellKnownConfiguration, ValidateConfirmationLinkParameter validateConfirmationLinkParameter, AuthenticateParameter authenticateParameter);
        Task<IEnumerable<OfficeDocumentConfirmationLink>> GetAllConfirmationLinks(GetAllConfirmationLinksParameter parameter);
        Task<bool> DeleteConfirmationCode(DeleteConfirmationCodeParameter parameter);
        Task<OfficeDocumentConfirmationLink> GetConfirmationCode(string code);
    }

    internal sealed class OfficeDocumentActions : IOfficeDocumentActions
    {
        private readonly IAddOfficeDocumentAction _addOfficeDocumentAction;
        private readonly IGetOfficeDocumentAction _getOfficeDocumentAction;
        private readonly IDecryptOfficeDocumentAction _decryptOfficeDocumentAction;
        private readonly IGetOfficeDocumentPermissionsAction _getOfficeDocumentPermissionsAction;
        private readonly IGenerateConfirmationLinkAction _generateConfirmationLinkAction;
        private readonly IValidateConfirmationLinkAction _validateConfirmationLinkAction;
        private readonly IGetAllConfirmationLinksAction _getAllConfirmationLinksAction;
        private readonly IDeleteOfficeDocumentConfirmationCodeAction _deleteOfficeDocumentConfirmationCodeAction;
        private readonly IGetOfficeDocumentInvitationLinkAction _getOfficeDocumentInvitationLinkAction;

        public OfficeDocumentActions(IAddOfficeDocumentAction addOfficeDocumentAction, IGetOfficeDocumentAction getOfficeDocumentAction, 
            IDecryptOfficeDocumentAction decryptOfficeDocumentAction,
            IGetOfficeDocumentPermissionsAction getOfficeDocumentPermissionsAction, IGenerateConfirmationLinkAction generateConfirmationLinkAction,
            IValidateConfirmationLinkAction validateConfirmationLinkAction, IGetAllConfirmationLinksAction getAllConfirmationLinksAction,
            IDeleteOfficeDocumentConfirmationCodeAction deleteOfficeDocumentConfirmationCodeAction, IGetOfficeDocumentInvitationLinkAction getOfficeDocumentInvitationLinkAction)
        {
            _addOfficeDocumentAction = addOfficeDocumentAction;
            _getOfficeDocumentAction = getOfficeDocumentAction;
            _decryptOfficeDocumentAction = decryptOfficeDocumentAction;
            _getOfficeDocumentPermissionsAction = getOfficeDocumentPermissionsAction;
            _generateConfirmationLinkAction = generateConfirmationLinkAction;
            _validateConfirmationLinkAction = validateConfirmationLinkAction;
            _getAllConfirmationLinksAction = getAllConfirmationLinksAction;
            _deleteOfficeDocumentConfirmationCodeAction = deleteOfficeDocumentConfirmationCodeAction;
            _getOfficeDocumentInvitationLinkAction = getOfficeDocumentInvitationLinkAction;
        }

        public Task<bool> Add(string openidWellKnownConfiguration, AddDocumentParameter document, AuthenticateParameter authenticateParameter)
        {
            return _addOfficeDocumentAction.Execute(openidWellKnownConfiguration, document, authenticateParameter);
        }

        public Task<OfficeDocumentAggregate> Get(string documentId)
        {
            return _getOfficeDocumentAction.Execute(documentId);
        }

        public Task<DecryptedResponse> Decrypt(DecryptOfficeDocumentParameter decryptOfficeDocumentParameter, string accessToken, AuthenticateParameter authenticateParameter)
        {
            return _decryptOfficeDocumentAction.Execute(decryptOfficeDocumentParameter, accessToken, authenticateParameter);
        }

        public Task<IEnumerable<OfficeDocumentPermissionResponse>> GetPermissions(string documentId, string subject, AuthenticateParameter authenticateParameter)
        {
            return _getOfficeDocumentPermissionsAction.Execute(documentId, subject, authenticateParameter);
        }

        public Task<string> GenerateConfirmationLink(string documentId, GenerateConfirmationLinkParameter generateConfirmationCodeParameter)
        {
            return _generateConfirmationLinkAction.Execute(documentId, generateConfirmationCodeParameter);
        }

        public Task<bool> ValidateConfirmationLink(string wellKnownConfiguration, ValidateConfirmationLinkParameter validateConfirmationLinkParameter, AuthenticateParameter authenticateParameter)
        {
            return _validateConfirmationLinkAction.Execute(wellKnownConfiguration, validateConfirmationLinkParameter, authenticateParameter);
        }

        public Task<IEnumerable<OfficeDocumentConfirmationLink>> GetAllConfirmationLinks(GetAllConfirmationLinksParameter parameter)
        {
            return _getAllConfirmationLinksAction.Execute(parameter);
        }

        public Task<bool> DeleteConfirmationCode(DeleteConfirmationCodeParameter parameter)
        {
            return _deleteOfficeDocumentConfirmationCodeAction.Execute(parameter);
        }

        public Task<OfficeDocumentConfirmationLink> GetConfirmationCode(string code)
        {
            return _getOfficeDocumentInvitationLinkAction.Execute(code);
        }
    }
}
