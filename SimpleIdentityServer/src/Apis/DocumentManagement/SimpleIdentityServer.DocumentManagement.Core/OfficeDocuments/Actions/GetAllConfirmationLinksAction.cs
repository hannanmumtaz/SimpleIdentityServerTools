using SimpleIdentityServer.DocumentManagement.Core.Exceptions;
using SimpleIdentityServer.DocumentManagement.Core.Parameters;
using SimpleIdentityServer.DocumentManagement.Core.Repositories;
using SimpleIdentityServer.DocumentManagement.Core.Validators;
using SimpleIdentityServer.DocumentManagement.Store;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Core.OfficeDocuments.Actions
{
    public interface IGetAllConfirmationLinksAction
    {
        Task<IEnumerable<OfficeDocumentConfirmationLink>> Execute(GetAllConfirmationLinksParameter parameter);
    }

    internal sealed class GetAllConfirmationLinksAction : IGetAllConfirmationLinksAction
    {
        private readonly IOfficeDocumentRepository _officeDocumentRepository;
        private readonly IOfficeDocumentConfirmationLinkStore _officeDocumentConfirmationLinkStore;
        private readonly IGetAllConfirmationLinksValidator _getAllConfirmationLinksValidator;

        public GetAllConfirmationLinksAction(IOfficeDocumentRepository officeDocumentRepository, IOfficeDocumentConfirmationLinkStore officeDocumentConfirmationLinkStore,
            IGetAllConfirmationLinksValidator getAllConfirmationLinksValidator)
        {
            _officeDocumentRepository = officeDocumentRepository;
            _officeDocumentConfirmationLinkStore = officeDocumentConfirmationLinkStore;
            _getAllConfirmationLinksValidator = getAllConfirmationLinksValidator;
        }

        public async Task<IEnumerable<OfficeDocumentConfirmationLink>> Execute(GetAllConfirmationLinksParameter parameter)
        {
            if(parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            _getAllConfirmationLinksValidator.Check(parameter);
            var officeDocument = await _officeDocumentRepository.Get(parameter.DocumentId);
            if (officeDocument == null)
            {
                throw new DocumentNotFoundException();
            }

            if(officeDocument.Subject != parameter.Subject)
            {
                throw new NotAuthorizedException();
            }

            return await _officeDocumentConfirmationLinkStore.Search(new SearchOfficeDocumentConfirmationLinkParameter
            {
                DocumentIds = new[] { parameter.DocumentId }
            });
        }
    }
}
