using Microsoft.EntityFrameworkCore;
using SimpleIdentityServer.Module.Feed.EF.Models;
using System;

namespace SimpleIdentityServer.Module.Feed.EF.Mappings
{
    internal static class UnitPackageMapping
    {
        public static ModelBuilder AddUnitPackageMapping(this ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<UnitPackage>()
                .ToTable("unitPackages")
                .HasKey(p => p.Library);
            modelBuilder.Entity<UnitPackage>()
                .HasOne(p => p.Category)
                .WithMany(p => p.Packages)
                .HasForeignKey(p => p.CategoryId);
            modelBuilder.Entity<UnitPackage>()
                .HasOne(p => p.Unit)
                .WithMany(p => p.Packages)
                .HasForeignKey(p => p.UnitName);
            return modelBuilder;
        }
    }
}
