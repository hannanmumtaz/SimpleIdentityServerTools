using Microsoft.EntityFrameworkCore;
using SimpleIdentityServer.Module.Feed.EF.Models;
using System;

namespace SimpleIdentityServer.Module.Feed.EF.Mappings
{
    internal static class ConnectorScopeMapping
    {
        public static ModelBuilder AddConnectorScopeMapping(this ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<ConnectorScope>()
                .ToTable("connectorScopes")
                .HasKey(p => p.Id);
            return modelBuilder;
        }
    }
}
