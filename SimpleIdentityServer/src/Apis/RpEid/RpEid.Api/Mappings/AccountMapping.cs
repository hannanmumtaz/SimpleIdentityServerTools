using Microsoft.EntityFrameworkCore;
using RpEid.Api.Models;
using System;

namespace RpEid.Api.Mappings
{
    internal static class AccountMapping
    {
        public static ModelBuilder AddAccountMapping(this ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<Account>()
                .ToTable("accounts")
                .HasKey(a => a.Subject);
            return modelBuilder;
        }
    }
}
