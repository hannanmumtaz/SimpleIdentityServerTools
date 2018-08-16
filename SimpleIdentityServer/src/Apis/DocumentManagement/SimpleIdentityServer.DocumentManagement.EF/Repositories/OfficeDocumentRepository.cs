using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.DocumentManagement.Core.Aggregates;
using SimpleIdentityServer.DocumentManagement.Core.Repositories;
using SimpleIdentityServer.DocumentManagement.EF.Extensions;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.EF.Repositories
{
    internal sealed class OfficeDocumentRepository : IOfficeDocumentRepository
    {
        private readonly IServiceProvider _serviceProvider;

        public OfficeDocumentRepository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<bool> Add(OfficeDocumentAggregate document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<DocumentManagementDbContext>())
                {
                    var rec = await context.OfficeDocuments.FirstOrDefaultAsync(p => p.Id == document.Id).ConfigureAwait(false);
                    if (rec != null)
                    {
                        return false;
                    }

                    var newRecord = document.ToModel();
                    newRecord.CreateDateTime = DateTime.UtcNow;
                    newRecord.UpdateDateTime = DateTime.UtcNow;
                    context.OfficeDocuments.Add(newRecord);
                    await context.SaveChangesAsync().ConfigureAwait(false);
                    return true;
                }
            }
        }

        public async Task<OfficeDocumentAggregate> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<DocumentManagementDbContext>())
                {
                    var document = await context.OfficeDocuments.FirstOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);
                    if (document == null)
                    {
                        return null;
                    }

                    return document.ToDomain();
                }
            }
        }
    }
}
