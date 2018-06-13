using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Module.Feed.Core.Models;
using SimpleIdentityServer.Module.Feed.Core.Repositories;
using SimpleIdentityServer.Module.Feed.EF.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Module.Feed.EF.Repositories
{
    internal sealed class ConnectorRepository : IConnectorRepository
    {
        private readonly IServiceProvider _serviceProvider;

        public ConnectorRepository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<ConnectorAggregate> Get(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            ConnectorAggregate result;
            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ModuleFeedDbContext>())
                {
                    var connector = await context.Connectors.FirstOrDefaultAsync(p => p.Name == name).ConfigureAwait(false);
                    if (connector == null)
                    {
                        result =  null;
                    }
                    else
                    {
                        result = connector.ToDomain();
                    }
                }
            }

            return result;
        }

        public async Task<bool> Add(IEnumerable<ConnectorAggregate> connectors)
        {
            if (connectors == null)
            {
                throw new ArgumentNullException(nameof(connectors));
            }


            var result = true;
            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ModuleFeedDbContext>())
                {
                    using (var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false))
                    {
                        try
                        {
                            foreach (var connector in connectors)
                            {
                                var record = connector.ToModel();
                                record.CreateDateTime = DateTime.UtcNow;
                                record.UpdateDateTime = DateTime.UtcNow;
                                context.Connectors.Add(record);
                            }

                            await context.SaveChangesAsync().ConfigureAwait(false);
                            transaction.Commit();
                        }
                        catch
                        {

                            result = false;
                            transaction.Rollback();
                        }
                    }
                }
            }

            return result;
        }

        public async Task<bool> Delete(IEnumerable<string> names)
        {
            if (names == null)
            {
                throw new ArgumentNullException(nameof(names));
            }

            var result = true;
            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ModuleFeedDbContext>())
                {
                    using (var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false))
                    {
                        try
                        {
                            foreach(var name in names)
                            {
                                var connector = await context.Connectors.FirstOrDefaultAsync(p => p.Name == name).ConfigureAwait(false);
                                if (connector != null)
                                {
                                    context.Connectors.Remove(connector);
                                }
                                else
                                {
                                    result = false;
                                    break;
                                }
                            }

                            await context.SaveChangesAsync().ConfigureAwait(false);
                            transaction.Commit();
                        }
                        catch
                        {
                            result = false;
                            transaction.Rollback();
                        }
                    }
                }
            }

            return result;
        }

        public async Task<IEnumerable<ConnectorAggregate>> GetAll()
        {
            IEnumerable<ConnectorAggregate> result = null;
            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ModuleFeedDbContext>())
                {
                    var connectors = await context.Connectors.ToListAsync().ConfigureAwait(false);
                    result = connectors.Select(c => c.ToDomain());
                }
            }

            return result;
        }
    }
}
