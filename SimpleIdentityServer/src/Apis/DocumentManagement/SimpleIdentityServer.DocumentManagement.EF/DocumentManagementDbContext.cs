using Microsoft.EntityFrameworkCore;
using SimpleIdentityServer.DocumentManagement.EF.Mappings;
using SimpleIdentityServer.DocumentManagement.EF.Models;

namespace SimpleIdentityServer.DocumentManagement.EF
{
    public class DocumentManagementDbContext : DbContext
    {
        public DocumentManagementDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            try
            {
                Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ COMITTED;");
            }
            catch { }
        }


        public virtual DbSet<OfficeDocument> OfficeDocuments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddOfficeDocumentMapping();
            base.OnModelCreating(modelBuilder);
        }
    }
}
