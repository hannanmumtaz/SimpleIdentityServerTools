using Microsoft.EntityFrameworkCore;
using SimpleIdentityServer.Module.Feed.EF.Models;
using System;

namespace SimpleIdentityServer.Module.Feed.EF.Mappings
{
    internal static class ConnectorClaimRuleMapping
    {
        public static ModelBuilder AddConnectorClaimRuleMapping(this ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<ConnectorClaimRule>()
                .ToTable("connectorClaimRules")
                .HasKey(p => p.Id);
            return modelBuilder;
        }
    }
}
