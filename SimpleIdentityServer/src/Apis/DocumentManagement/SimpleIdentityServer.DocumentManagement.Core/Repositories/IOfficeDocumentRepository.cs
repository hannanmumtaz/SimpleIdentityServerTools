using SimpleIdentityServer.DocumentManagement.Core.Aggregates;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Core.Repositories
{
    public interface IOfficeDocumentRepository
    {
        Task<OfficeDocumentAggregate> Get(string id);
        Task<bool> Add(OfficeDocumentAggregate document);
    }
}
