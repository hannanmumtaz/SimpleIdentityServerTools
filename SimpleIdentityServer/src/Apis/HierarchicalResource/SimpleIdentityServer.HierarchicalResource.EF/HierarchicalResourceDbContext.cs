using Microsoft.EntityFrameworkCore;
using SimpleIdentityServer.HierarchicalResource.EF.Mappings;
using SimpleIdentityServer.HierarchicalResource.EF.Models;

namespace SimpleIdentityServer.HierarchicalResource.EF
{
    public class HierarchicalResourceDbContext : DbContext
    {
        public HierarchicalResourceDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            try
            {
                Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ COMITTED;");
            }
            catch { }
        }

        public virtual DbSet<Asset> Assets { get; set; }
        public virtual DbSet<AssetAuthPolicy> AssetAuthPolicies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddAssetMapping()
                .AddAssetAuthPolicyMapping();
            base.OnModelCreating(modelBuilder);
        }
    }
}
