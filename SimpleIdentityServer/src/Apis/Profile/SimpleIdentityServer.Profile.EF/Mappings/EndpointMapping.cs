using Microsoft.EntityFrameworkCore;
using SimpleIdentityServer.Profile.EF.Models;

namespace SimpleIdentityServer.Profile.EF.Mappings
{
    internal static class EndpointMapping
    {
        public static ModelBuilder AddEndpointMapping(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Endpoint>()
                .ToTable("endpoints")
                .HasKey(s => s.Url);
            return modelBuilder;
        }
    }
}
