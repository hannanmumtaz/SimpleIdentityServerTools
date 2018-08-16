using Microsoft.EntityFrameworkCore;
using SimpleIdentityServer.DocumentManagement.EF.Models;

namespace SimpleIdentityServer.DocumentManagement.EF.Mappings
{
    internal static class OfficeDocumentJsonWebKeyMapping
    {
        public static void AddJsonWebKeyMapping(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OfficeDocumentJsonWebKey>()
                .ToTable("officeDocumentJsonWebKeys")
                .HasKey(j => j.Kid);
        }
    }
}
