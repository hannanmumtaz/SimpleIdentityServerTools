using Microsoft.EntityFrameworkCore;
using RpEid.Api.Mappings;
using RpEid.Api.Models;

namespace RpEid.Api
{
    public class RpEidContext : DbContext
    {
        public RpEidContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            try
            {
                Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ COMITTED;");
            }
            catch { }
        }

        public virtual DbSet<Account> Accounts { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddAccountMapping();
            base.OnModelCreating(modelBuilder);
        }
    }
}
