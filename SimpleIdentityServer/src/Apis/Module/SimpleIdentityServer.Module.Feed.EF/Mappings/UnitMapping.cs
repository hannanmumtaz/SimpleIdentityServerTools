using Microsoft.EntityFrameworkCore;
using SimpleIdentityServer.Module.Feed.EF.Models;
using System;

namespace SimpleIdentityServer.Module.Feed.EF.Mappings
{
    internal static class UnitMapping
    {
        public static ModelBuilder AddUnitMapping(this ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<Unit>()
                .ToTable("units")
                .HasKey(p => p.UnitName);
            modelBuilder.Entity<Unit>()
                .HasMany(u => u.Projects)
                .WithOne(u => u.Unit)
                .HasForeignKey(u => u.UnitId);
            return modelBuilder;
        }
    }
}
