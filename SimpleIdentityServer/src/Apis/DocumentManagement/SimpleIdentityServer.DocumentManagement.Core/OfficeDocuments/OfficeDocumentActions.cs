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
        Task<OfficeDocumentAggregate> Get(string documentId, string accessToken, AuthenticateParameter authenticateParameter);
        Task<bool> Update(string wellKnownConfiguration, string documentId, UpdateOfficeDocumentParameter parameter, AuthenticateParameter authenticateParameter);
        Task<DecryptedResponse> Decrypt(DecryptOfficeDocumentParameter decryptOfficeDocumentParameter, string accessToken, AuthenticateParameter authenticateParameter);
        Task<IEnumerable<OfficeDocumentPermissionResponse>> GetPermissions(string documentId, string accessToken, AuthenticateParameter authenticateParameter);
        Task<string> GenerateConfirmationLink(string documentId, GenerateConfirmationLinkParameter generateConfirmationCodeParameter);
        Task<bool> ValidateConfirmationLink(string wellKnownConfiguration, ValidateConfirmationLinkParameter validateConfirmationLinkParameter, AuthenticateParameter authenticateParameter);
        Task<IEnumerable<OfficeDocumentConfirmationLink>> GetAllConfirmationLinks(GetAllConfirmationLinksParameter parameter);
    }

    internal sealed class OfficeDocumentActions : IOfficeDocumentActions
    {
        private readonly IAddOfficeDocumentAction _addOfficeDocumentAction;
        private readonly IGetOfficeDocumentAction _getOfficeDocumentAction;
        private readonly IUpdateOfficeDocumentAction _updateOfficeDocumentAction;
        private readonly IDecryptOfficeDocumentAction _decryptOfficeDocumentAction;
        private readonly IGetOfficeDocumentPermissionsAction _getOfficeDocumentPermissionsAction;
        private readonly IGenerateConfirmationLinkAction _generateConfirmationLinkAction;
        private readonly IValidateConfirmationLinkAction _validateConfirmationLinkAction;
        private readonly IGetAllConfirmationLinksAction _getAllConfirmationLinksAction;

        public OfficeDocumentActions(IAddOfficeDocumentAction addOfficeDocumentAction, IGetOfficeDocumentAction getOfficeDocumentAction, 
            IUpdateOfficeDocumentAction updateOfficeDocumentAction, IDecryptOfficeDocumentAction decryptOfficeDocumentAction,
            IGetOfficeDocumentPermissionsAction getOfficeDocumentPermissionsAction, IGenerateConfirmationLinkAction generateConfirmationLinkAction,
            IValidateConfirmationLinkAction validateConfirmationLinkAction, IGetAllConfirmationLinksAction getAllConfirmationLinksAction)
        {
            _addOfficeDocumentAction = addOfficeDocumentAction;
            _getOfficeDocumentAction = getOfficeDocumentAction;
            _updateOfficeDocumentAction = updateOfficeDocumentAction;
            _decryptOfficeDocumentAction = decryptOfficeDocumentAction;
            _getOfficeDocumentPermissionsAction = getOfficeDocumentPermissionsAction;
            _generateConfirmationLinkAction = generateConfirmationLinkAction;
            _validateConfirmationLinkAction = validateConfirmationLinkAction;
            _getAllConfirmationLinksAction = getAllConfirmationLinksAction;
        }

        public Task<bool> Add(string openidWellKnownConfiguration, AddDocumentParameter document, AuthenticateParameter authenticateParameter)
        {
            return _addOfficeDocumentAction.Execute(openidWellKnownConfiguration, document, authenticateParameter);
        }

        public Task<OfficeDocumentAggregate> Get(string documentId, string accessToken, AuthenticateParameter authenticateParameter)
        {
            return _getOfficeDocumentAction.Execute(documentId, accessToken, authenticateParameter);
        }

        public Task<bool> Update(string wellKnownConfiguration, string documentId, UpdateOfficeDocumentParameter parameter, AuthenticateParameter authenticateParameter)
        {
            return _updateOfficeDocumentAction.Execute(wellKnownConfiguration, documentId, parameter, authenticateParameter);
        }

        public Task<DecryptedResponse> Decrypt(DecryptOfficeDocumentParameter decryptOfficeDocumentParameter, string accessToken, AuthenticateParameter authenticateParameter)
        {
            return _decryptOfficeDocumentAction.Execute(decryptOfficeDocumentParameter, accessToken, authenticateParameter);
        }

        public Task<IEnumerable<OfficeDocumentPermissionResponse>> GetPermissions(string documentId, string accessToken, AuthenticateParameter authenticateParameter)
        {
            return _getOfficeDocumentPermissionsAction.Execute(documentId, accessToken, authenticateParameter);
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
    }
}
