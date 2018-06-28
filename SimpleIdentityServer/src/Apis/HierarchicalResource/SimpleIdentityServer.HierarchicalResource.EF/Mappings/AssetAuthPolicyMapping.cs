using Microsoft.EntityFrameworkCore;
using SimpleIdentityServer.HierarchicalResource.EF.Models;

namespace SimpleIdentityServer.HierarchicalResource.EF.Mappings
{
    internal static class AssetAuthPolicyMapping
    {
        public static ModelBuilder AddAssetAuthPolicyMapping(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AssetAuthPolicy>()
                .ToTable("assetAuthPolicies")
                .HasKey(s => new
                {
                    s.AssetHash,
                    s.AuthPolicyId
                });
            return modelBuilder;
        }
    }
}
