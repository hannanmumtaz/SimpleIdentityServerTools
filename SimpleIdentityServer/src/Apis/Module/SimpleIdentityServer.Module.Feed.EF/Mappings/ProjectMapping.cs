using Microsoft.EntityFrameworkCore;
using SimpleIdentityServer.Module.Feed.EF.Models;
using System;

namespace SimpleIdentityServer.Module.Feed.EF.Mappings
{
    internal static class ProjectMapping
    {
        public static ModelBuilder AddProjectMapping(this ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<Project>()
                .ToTable("projects")
                .HasKey(p => p.Id);
            modelBuilder.Entity<Project>()
                .HasMany(u => u.Units)
                .WithOne(u => u.Project)
                .HasForeignKey(u => u.ProjectId);
            return modelBuilder;
        }
    }
}
