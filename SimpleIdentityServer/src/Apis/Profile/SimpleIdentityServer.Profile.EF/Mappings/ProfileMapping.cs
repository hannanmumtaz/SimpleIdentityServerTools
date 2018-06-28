using Microsoft.EntityFrameworkCore;
using System;

namespace SimpleIdentityServer.Profile.EF.Mappings
{
    internal static class ProfileMapping
    {
        public static ModelBuilder AddProfileMapping(this ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<Models.Profile>()
                .ToTable("profiles")
                .HasKey(s => s.Subject);
            return modelBuilder;
        }
    }
}
