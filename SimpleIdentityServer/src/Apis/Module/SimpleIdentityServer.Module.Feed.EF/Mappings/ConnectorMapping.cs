﻿using Microsoft.EntityFrameworkCore;
using SimpleIdentityServer.Module.Feed.EF.Models;
using System;

namespace SimpleIdentityServer.Module.Feed.EF.Mappings
{
    internal static class ConnectorMapping
    {
        public static ModelBuilder AddConnectorMapping(this ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<Connector>()
                .ToTable("connectors")
                .HasKey(p => p.Id);
            modelBuilder.Entity<Connector>()
                .HasMany(p => p.Scopes)
                .WithOne(p => p.Connector)
                .HasForeignKey(p => p.ConnectorId);
            modelBuilder.Entity<Connector>()
                .HasMany(p => p.ConnectorClaimRules)
                .WithOne(p => p.Connector)
                .HasForeignKey(p => p.ConnectorId);
            modelBuilder.Entity<Connector>()
                .HasMany(p => p.ConnectorClaims)
                .WithOne(p => p.Connector)
                .HasForeignKey(p => p.ConnectorId);
            return modelBuilder;
        }
    }
}
