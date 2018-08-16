using Microsoft.EntityFrameworkCore;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;
using SimpleIdentityServer.DocumentManagement.Core.Repositories;
using SimpleIdentityServer.DocumentManagement.EF.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.EF.Repositories
{
    internal sealed class JsonWebKeyRepository : IJsonWebKeyRepository
    {
        private readonly DocumentManagementDbContext _context;

        public JsonWebKeyRepository(DocumentManagementDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OfficeDocumentJsonWebKeyResponse>> GetAllAsync()
        {
            return await _context.JsonWebKeys.Select(s => s.ToDomain()).ToListAsync().ConfigureAwait(false);
        }

        public async Task<OfficeDocumentJsonWebKeyResponse> Get(string kid)
        {
            var jsonWebKey = await _context.JsonWebKeys.Select(s => s.ToDomain()).FirstOrDefaultAsync(s => s.Kid == kid).ConfigureAwait(false);
            return jsonWebKey;
        }
    }
}
