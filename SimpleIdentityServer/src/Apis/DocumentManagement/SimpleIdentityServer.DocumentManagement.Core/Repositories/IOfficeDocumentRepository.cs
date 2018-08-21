using SimpleIdentityServer.DocumentManagement.Core.Aggregates;
using SimpleIdentityServer.DocumentManagement.Core.Parameters;
using SimpleIdentityServer.DocumentManagement.Core.Results;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Core.Repositories
{
    public interface IOfficeDocumentRepository
    {
        Task<OfficeDocumentAggregate> Get(string id);
        Task<bool> Add(OfficeDocumentAggregate document);
        Task<SearchOfficeDocumentsResult> Search(SearchDocumentsParameter parameter);
    }
}
