using Microsoft.EntityFrameworkCore;
using SimpleIdentityServer.Module.Feed.EF.Mappings;
using SimpleIdentityServer.Module.Feed.EF.Models;

namespace SimpleIdentityServer.Module.Feed.EF
{
    public class ModuleFeedDbContext : DbContext
    {
        public ModuleFeedDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            try
            {
                Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ COMITTED;");
            }
            catch { }
        }

        public virtual DbSet<PackageCategory> Categories { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<ProjectUnit> ProjectUnits { get; set; }
        public virtual DbSet<Connector> Connectors { get; set; }
        public virtual DbSet<ConnectorScope> ConnectorScopes { get; set; }
        public virtual DbSet<ConnectorClaimRule> ConnectorClaimRules { get; set; }
        public virtual DbSet<ConnectorClaim> ConnectorClaims { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddProjectMapping()
                .AddPackageCategoryMapping()
                .AddUnitMapping()
                .AddProjectUnitMapping()
                .AddUnitPackageMapping()
                .AddConnectorScopeMapping()
                .AddConnectorClaimRuleMapping()
                .AddConnectorClaimMapping()
                .AddConnectorMapping();
            base.OnModelCreating(modelBuilder);
        }
    }
}
