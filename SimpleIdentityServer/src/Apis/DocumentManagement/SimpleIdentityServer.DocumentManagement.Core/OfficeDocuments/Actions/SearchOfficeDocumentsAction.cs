using SimpleIdentityServer.DocumentManagement.Core.Parameters;
using SimpleIdentityServer.DocumentManagement.Core.Repositories;
using SimpleIdentityServer.DocumentManagement.Core.Results;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Core.OfficeDocuments.Actions
{
    public interface ISearchOfficeDocumentsAction
    {
        Task<SearchOfficeDocumentsResult> Execute(SearchDocumentsParameter parameter);
    }

    internal sealed class SearchOfficeDocumentsAction : ISearchOfficeDocumentsAction
    {
        private readonly IOfficeDocumentRepository _officeDocumentRepository;

        public SearchOfficeDocumentsAction(IOfficeDocumentRepository officeDocumentRepository)
        {
            _officeDocumentRepository = officeDocumentRepository;
        }


        public Task<SearchOfficeDocumentsResult> Execute(SearchDocumentsParameter parameter)
        {
            if(parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            return _officeDocumentRepository.Search(parameter);
        }
    }
}