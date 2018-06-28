using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Profile.Core.Models;
using SimpleIdentityServer.Profile.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Profile.EF.Repositories
{
    internal sealed class ProfileRepository : IProfileRepository
    {
        private readonly IServiceProvider _serviceProvider;

        public ProfileRepository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<ProfileAggregate> Get(string subject)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }

            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ProfileDbContext>())
                {
                    var asset = await context.Profiles.FirstOrDefaultAsync(a => a.Subject == subject).ConfigureAwait(false);
                    if (asset == null)
                    {
                        return null;
                    }

                    return GetProfile(asset);
                }
            }
        }

        public async Task<bool> Add(IEnumerable<ProfileAggregate> profiles)
        {
            if (profiles == null)
            {
                throw new ArgumentNullException(nameof(profiles));
            }

            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ProfileDbContext>())
                {
                    using (var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false))
                    {
                        try
                        {
                            foreach (var profile in profiles)
                            {
                                var record = new Models.Profile
                                {
                                    AuthUrl = profile.AuthUrl,
                                    OpenIdUrl = profile.OpenidUrl,
                                    ScimUrl = profile.ScimUrl,
                                    Subject = profile.Subject
                                };

                                context.Profiles.Add(record);
                            }

                            await context.SaveChangesAsync().ConfigureAwait(false);
                            transaction.Commit();
                            return true;
                        }
                        catch
                        {
                            transaction.Rollback();
                        }
                    }

                    return false;
                }
            }
        }

        public async Task<bool> Delete(IEnumerable<string> subjects)
        {
            if (subjects == null)
            {
                throw new ArgumentNullException(nameof(subjects));
            }

            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ProfileDbContext>())
                {
                    using (var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false))
                    {
                        try
                        {
                            var profiles = context.Profiles.Where(a => subjects.Contains(a.Subject));
                            context.Profiles.RemoveRange(profiles);
                            await context.SaveChangesAsync().ConfigureAwait(false);
                            transaction.Commit();
                            return true;
                        }
                        catch
                        {
                            transaction.Rollback();
                        }
                    }

                    return false;
                }
            }
        }

        public async Task<bool> Update(IEnumerable<ProfileAggregate> profiles)
        {
            if (profiles == null)
            {
                throw new ArgumentNullException(nameof(profiles));
            }

            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ProfileDbContext>())
                {
                    using (var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false))
                    {
                        try
                        {
                            foreach (var profile in profiles)
                            {
                                var record = context.Profiles.FirstOrDefault(a => a.Subject == profile.Subject);
                                if (record == null)
                                {
                                    return false;
                                }

                                record.OpenIdUrl = profile.OpenidUrl;
                                record.AuthUrl = profile.AuthUrl;
                                record.ScimUrl = profile.ScimUrl;
                            }

                            await context.SaveChangesAsync().ConfigureAwait(false);
                            transaction.Commit();
                            return true;
                        }
                        catch
                        {
                            transaction.Rollback();
                        }
                    }

                    return false;
                }
            }
        }

        private static ProfileAggregate GetProfile(Models.Profile profile)
        {
            return new ProfileAggregate
            {
                AuthUrl = profile.AuthUrl,
                OpenidUrl = profile.OpenIdUrl,
                ScimUrl = profile.ScimUrl,
                Subject = profile.Subject
            };
        }
    }
}
