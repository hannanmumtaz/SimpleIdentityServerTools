using Microsoft.EntityFrameworkCore;
using SimpleIdentityServer.Module.Feed.EF.Models;
using System;

namespace SimpleIdentityServer.Module.Feed.EF.Mappings
{
    internal static class TwoFactorAuthenticatorMapping
    {
        public static ModelBuilder AddTwoFactorAuthenticatorMapping(this ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }
            
            modelBuilder.Entity<TwoFactorAuthenticator>()
                .ToTable("twoFactorAuthenticator")
                .HasKey(p => p.Id);
            return modelBuilder;
        }
    }
}
