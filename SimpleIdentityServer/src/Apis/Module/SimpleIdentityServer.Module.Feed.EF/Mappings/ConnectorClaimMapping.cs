using Microsoft.EntityFrameworkCore;
using SimpleIdentityServer.Module.Feed.EF.Models;
using System;

namespace SimpleIdentityServer.Module.Feed.EF.Mappings
{
    internal static class ConnectorClaimMapping
    {
        public static ModelBuilder AddConnectorClaimMapping(this ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<ConnectorClaim>()
                .ToTable("connectorClaims")
                .HasKey(p => p.Id);
            modelBuilder.Entity<ConnectorClaim>()
                .HasMany(c => c.Children)
                .WithOne(c => c.ParentConnector)
                .HasForeignKey(c => c.ParentId);
            return modelBuilder;
        }
    }
}
