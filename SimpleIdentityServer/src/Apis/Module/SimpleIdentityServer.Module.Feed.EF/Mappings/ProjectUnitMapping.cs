using Microsoft.EntityFrameworkCore;
using SimpleIdentityServer.Module.Feed.EF.Models;
using System;

namespace SimpleIdentityServer.Module.Feed.EF.Mappings
{
    internal static class ProjectUnitMapping
    {
        public static ModelBuilder AddProjectUnitMapping(this ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<ProjectUnit>()
                .ToTable("projectUnits")
                .HasKey(p => p.Id);
            return modelBuilder;
        }
    }
}
