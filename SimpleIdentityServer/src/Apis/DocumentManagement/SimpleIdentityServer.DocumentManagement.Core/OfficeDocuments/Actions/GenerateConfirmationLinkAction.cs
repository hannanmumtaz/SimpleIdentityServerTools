using SimpleIdentityServer.DocumentManagement.Core.Exceptions;
using SimpleIdentityServer.DocumentManagement.Core.Parameters;
using SimpleIdentityServer.DocumentManagement.Core.Repositories;
using SimpleIdentityServer.DocumentManagement.Store;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Core.OfficeDocuments.Actions
{
    public interface IGenerateConfirmationLinkAction
    {
        Task<string> Execute(string documentId, GenerateConfirmationLinkParameter generateConfirmationCodeParameter);
    }

    internal sealed class GenerateConfirmationLinkAction : IGenerateConfirmationLinkAction
    {
        private readonly IOfficeDocumentRepository _officeDocumentRepository;
        private readonly IOfficeDocumentConfirmationLinkStore _officeDocumentConfirmationLinkStore;

        public GenerateConfirmationLinkAction(IOfficeDocumentRepository officeDocumentRepository, IOfficeDocumentConfirmationLinkStore officeDocumentConfirmationLinkStore)
        {
            _officeDocumentRepository = officeDocumentRepository;
            _officeDocumentConfirmationLinkStore = officeDocumentConfirmationLinkStore;
        }

        public async Task<string> Execute(string documentId, GenerateConfirmationLinkParameter generateConfirmationCodeParameter)
        {
            if (string.IsNullOrWhiteSpace(documentId))
            {
                throw new ArgumentNullException(nameof(documentId));
            }
            
            var officeDocument = await _officeDocumentRepository.Get(documentId);
            if (officeDocument == null)
            {
                throw new DocumentNotFoundException();
            }
            
            if (string.IsNullOrWhiteSpace(officeDocument.UmaResourceId))
            {
                throw new BaseDocumentManagementApiException(ErrorCodes.InternalError, ErrorDescriptions.NoUmaResource);
            }

            if (string.IsNullOrWhiteSpace(officeDocument.UmaPolicyId))
            {
                throw new BaseDocumentManagementApiException(ErrorCodes.InternalError, ErrorDescriptions.NoUmaPolicy);
            }
            
            if (officeDocument.Subject != generateConfirmationCodeParameter.Subject)
            {
                throw new NotAuthorizedException();
            }

            var confirmationLink = new OfficeDocumentConfirmationLink
            {
                ConfirmationCode = Guid.NewGuid().ToString(),
                DocumentId = documentId,
                ExpiresIn = generateConfirmationCodeParameter.ExpiresIn,
                NumberOfConfirmations = generateConfirmationCodeParameter.NumberOfConfirmations
            };
            await _officeDocumentConfirmationLinkStore.Add(confirmationLink);
            return confirmationLink.ConfirmationCode;
        }
    }
}
