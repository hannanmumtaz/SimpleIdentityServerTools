using SimpleIdentityServer.DocumentManagement.Core.Aggregates;
using SimpleIdentityServer.DocumentManagement.Core.Exceptions;
using SimpleIdentityServer.DocumentManagement.Core.Parameters;
using SimpleIdentityServer.DocumentManagement.Core.Repositories;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Core.OfficeDocuments.Actions
{
    public interface IGetOfficeDocumentAction
    {
        Task<OfficeDocumentAggregate> Execute(string documentId);
    }

    internal sealed class GetOfficeDocumentAction : IGetOfficeDocumentAction
    {
        private readonly IOfficeDocumentRepository _officeDocumentRepository;

        public GetOfficeDocumentAction(IOfficeDocumentRepository officeDocumentRepository)
        {
            _officeDocumentRepository = officeDocumentRepository;
        }

        public async Task<OfficeDocumentAggregate> Execute(string documentId)
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
            
            return officeDocument;
        }
    }
}
