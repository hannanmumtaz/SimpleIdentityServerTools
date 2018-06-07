using Microsoft.EntityFrameworkCore;
using SimpleIdentityServer.DocumentManagement.EF.Models;
using System;

namespace SimpleIdentityServer.DocumentManagement.EF.Mappings
{
    internal static class OfficeDocumentMapping
    {
        public static ModelBuilder AddOfficeDocumentMapping(this ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<OfficeDocument>()
                .ToTable("officeDocuments")
                .HasKey(d => d.Id);
            return modelBuilder;
        }
    }
}
