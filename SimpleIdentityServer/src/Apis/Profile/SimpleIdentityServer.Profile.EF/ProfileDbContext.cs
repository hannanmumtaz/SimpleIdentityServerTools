using Microsoft.EntityFrameworkCore;
using SimpleIdentityServer.Profile.EF.Mappings;

namespace SimpleIdentityServer.Profile.EF
{
    public class ProfileDbContext : DbContext
    {
        public ProfileDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            try
            {
                Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ COMITTED;");
            }
            catch { }
        }
        
        public virtual DbSet<Models.Endpoint> Endpoints { get; set; }
        public virtual DbSet<Models.Profile> Profiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddEndpointManagerMapping()
                .AddEndpointMapping()
                .AddProfileMapping();
            base.OnModelCreating(modelBuilder);
        }
    }
}
