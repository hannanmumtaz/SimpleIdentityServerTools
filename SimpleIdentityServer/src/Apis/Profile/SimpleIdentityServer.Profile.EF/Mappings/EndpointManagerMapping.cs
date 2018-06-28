using Microsoft.EntityFrameworkCore;
using SimpleIdentityServer.Profile.EF.Models;

namespace SimpleIdentityServer.Profile.EF.Mappings
{
    internal static class EndpointManagerMapping
    {
        public static ModelBuilder AddEndpointManagerMapping(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EndpointManager>()
                .ToTable("endpointManagers")
                .HasKey(s => new { s.SourceUrl, s.AuthUrl });
            modelBuilder.Entity<EndpointManager>()
                .HasOne(s => s.SourceEndpoint)
                .WithOne(s => s.Manager)
                .HasForeignKey<EndpointManager>(s => s.SourceUrl);
            return modelBuilder;
        }
    }
}
