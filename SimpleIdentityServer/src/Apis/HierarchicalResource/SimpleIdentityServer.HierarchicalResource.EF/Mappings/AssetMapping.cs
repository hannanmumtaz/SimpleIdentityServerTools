using Microsoft.EntityFrameworkCore;
using SimpleIdentityServer.HierarchicalResource.EF.Models;

namespace SimpleIdentityServer.HierarchicalResource.EF.Mappings
{
    internal static class AssetMapping
    {
        public static ModelBuilder AddAssetMapping(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Asset>()
                .ToTable("assets")
                .HasKey(s => s.Hash);
            modelBuilder.Entity<Asset>()
                .HasMany(s => s.Children)
                .WithOne(s => s.Parent)
                .HasForeignKey(s => s.ResourceParentHash)
                .OnDelete(DeleteBehavior.Cascade);
            return modelBuilder;
        }
    }
}
