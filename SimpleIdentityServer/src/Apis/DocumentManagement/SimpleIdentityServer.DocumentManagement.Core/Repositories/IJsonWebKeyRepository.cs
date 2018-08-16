using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Core.Repositories
{
    public interface IJsonWebKeyRepository
    {
        Task<IEnumerable<OfficeDocumentJsonWebKeyResponse>> GetAllAsync();
        Task<OfficeDocumentJsonWebKeyResponse> Get(string kid);
    }
}
