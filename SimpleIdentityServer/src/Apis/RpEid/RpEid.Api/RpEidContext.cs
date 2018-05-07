using Microsoft.EntityFrameworkCore;
using RpEid.Api.Mappings;
using RpEid.Api.Models;

namespace RpEid.Api
{
    public class RpEidContext : DbContext
    {
        public RpEidContext() { }

        public virtual DbSet<Account> Accounts { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddAccountMapping();
            base.OnModelCreating(modelBuilder);
        }
    }
}
