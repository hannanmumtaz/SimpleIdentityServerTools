using Microsoft.EntityFrameworkCore;
using SimpleIdentityServer.Module.Feed.EF.Models;
using System;

namespace SimpleIdentityServer.Module.Feed.EF.Mappings
{
    internal static class PackageCategoryMapping
    {
        public static ModelBuilder AddPackageCategoryMapping(this ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<PackageCategory>()
                .ToTable("packageCategories")
                .HasKey(p => p.Name);
            return modelBuilder;
        }
    }
}
