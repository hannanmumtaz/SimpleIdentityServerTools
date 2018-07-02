using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.HierarchicalResource.Core.Models;
using SimpleIdentityServer.HierarchicalResource.Core.Parameters;
using SimpleIdentityServer.HierarchicalResource.Core.Repositories;
using SimpleIdentityServer.HierarchicalResource.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleIdentityServer.HierarchicalResource.EF.Repositories
{
    internal class AssetRepository : IAssetRepository
    {
        private readonly IServiceProvider _serviceProvider;

        public AssetRepository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<IEnumerable<AssetAggregate>> Search(SearchAssetsParameter parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<HierarchicalResourceDbContext>())
                {
                    IQueryable<Asset> assets = context.Assets
                        .Include(a => a.Children)
                        .Include(a => a.Parent);
                    if (parameter.HashLst != null && parameter.HashLst.Any())
                    {
                        assets = assets.Where(a => parameter.HashLst.Contains(a.Hash));
                    }

                    if (parameter.ExcludedHashLst != null && parameter.ExcludedHashLst.Any())
                    {
                        assets = assets.Where(a => !parameter.ExcludedHashLst.Contains(a.Hash));
                    }

                    switch (parameter.AssetLevelType)
                    {
                        case AssetLevelTypes.ROOT:
                            assets = assets.Where(a => a.Parent == null);
                            break;
                    }

                    if (parameter.IsDefaultWorkingDirectory != null)
                    {
                        assets = assets.Where(a => a.IsDefaultWorkingDirectory == parameter.IsDefaultWorkingDirectory.Value);
                    }

                    if (parameter.Names != null && parameter.Names.Any())
                    {
                        assets = assets.Where(a => parameter.Names.Any(n => a.Name.Contains(n)));
                    }

                    var result = await assets.ToListAsync().ConfigureAwait(false);
                    return result.Select(a => GetAsset(a));
                }
            }
        }

        public async Task<AssetAggregate> Get(string hash)
        {
            if (string.IsNullOrWhiteSpace(hash))
            {
                throw new ArgumentNullException(nameof(hash));
            }

            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<HierarchicalResourceDbContext>())
                {
                    var asset = await context.Assets
                        .Include(a => a.Parent)
                        .Include(a => a.Children).ThenInclude(a => a.Children)
                        .FirstOrDefaultAsync(a => a.Hash == hash).ConfigureAwait(false);
                    if (asset == null)
                    {
                        return null;
                    }

                    return GetAsset(asset);
                }
            }
        }

        public async Task<IEnumerable<AssetAggregate>> Get(IEnumerable<string> pathLst, bool includeChildren)
        {
            if (pathLst == null)
            {
                throw new ArgumentNullException(nameof(pathLst));
            }

            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<HierarchicalResourceDbContext>())
                {
                    IQueryable<Asset> assets = context.Assets;
                    if (includeChildren)
                    {
                        assets = assets.Where(a => pathLst.Any(p => a.Path.StartsWith(p)));
                    }
                    else
                    {
                        assets = assets.Where(a => pathLst.Contains(a.Path));
                    }

                    var res = await assets.ToListAsync().ConfigureAwait(false);
                    if (res == null)
                    {
                        return null;
                    }

                    var result = new List<AssetAggregate>();
                    foreach(var asset in res)
                    {
                        result.Add(GetAsset(asset));
                    }

                    return result;
                }
            }
        }

        public async Task<IEnumerable<AssetAggregate>> GetAllParents(string hash)
        {
            if (string.IsNullOrWhiteSpace(hash))
            {
                throw new ArgumentNullException(nameof(hash));
            }

            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<HierarchicalResourceDbContext>())
                {
                    var asset = await context.Assets.Include(a => a.Parent).ThenInclude(a => a.Children).ThenInclude(a => a.Children).FirstOrDefaultAsync(a => a.Hash == hash).ConfigureAwait(false);
                    if (asset == null)
                    {
                        return new List<AssetAggregate>();
                    }

                    var result = new List<AssetAggregate>();
                    if (asset.Parent != null)
                    {
                        result.Add(GetAsset(asset.Parent));
                        result.AddRange(await GetAllParents(asset.Parent.Hash));
                    }

                    return result;
                }
            }
        }
        
        public async Task<IEnumerable<AssetAggregate>> GetAllChildren(string hash)
        {
            if (string.IsNullOrWhiteSpace(hash))
            {
                throw new ArgumentNullException(nameof(hash));
            }

            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<HierarchicalResourceDbContext>())
                {
                    var asset = await context.Assets.Include(a => a.Children).FirstOrDefaultAsync(a => a.Hash == hash).ConfigureAwait(false);
                    if (asset == null)
                    {
                        return new List<AssetAggregate>();
                    }

                    var result = new List<AssetAggregate>();
                    var tasks = new List<Task<IEnumerable<AssetAggregate>>>();
                    if (asset.Children != null)
                    {
                        foreach (var child in asset.Children)
                        {
                            result.Add(GetAsset(child));
                            tasks.Add(GetAllChildren(child.Hash));
                        }
                    }

                    var lstAssets = await Task.WhenAll(tasks);
                    foreach (var rec in lstAssets)
                    {
                        result.AddRange(rec);
                    }

                    return result;
                }
            }
        }

        public async Task<bool> Add(IEnumerable<AssetAggregate> assets)
        {
            if (assets == null)
            {
                throw new ArgumentNullException(nameof(assets));
            }

            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<HierarchicalResourceDbContext>())
                {
                    using (var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false))
                    {
                        try
                        {
                            foreach (var asset in assets)
                            {
                                var record = new Asset
                                {
                                    CanRead = asset.CanRead,
                                    CanWrite = asset.CanWrite,
                                    CreateDateTime = asset.CreatedAt,
                                    Hash = asset.Hash,
                                    IsDefaultWorkingDirectory = false,
                                    IsLocked = asset.IsLocked,
                                    Name = asset.Name,
                                    ResourceParentHash = asset.ResourceParentHash,
                                    Path = asset.Path,
                                    MimeType = asset.MimeType,
                                    ResourceId = asset.ResourceId,
                                    PolicyIds = asset.AuthPolicyIds == null ? string.Empty : string.Join(",", asset.AuthPolicyIds)
                                };

                                context.Assets.Add(record);
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

        public async Task<bool> Remove(IEnumerable<string> hashLst)
        {
            if (hashLst == null)
            {
                throw new ArgumentNullException(nameof(hashLst));
            }

            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<HierarchicalResourceDbContext>())
                {
                    using (var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false))
                    {
                        try
                        {
                            var assets = context.Assets.Where(a => hashLst.Contains(a.Hash));
                            context.Assets.RemoveRange(assets);
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

        public async Task<bool> Update(IEnumerable<AssetAggregate> assets)
        {
            if (assets == null)
            {
                throw new ArgumentNullException(nameof(assets));
            }

            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<HierarchicalResourceDbContext>())
                {
                    using (var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false))
                    {
                        try
                        {
                            foreach(var asset in assets)
                            {
                                var record = context.Assets.FirstOrDefault(a => a.Hash == asset.Hash);
                                if (record == null)
                                {
                                    return false;
                                }

                                record.Name = asset.Name;
                                record.ResourceId = asset.ResourceId;
                                record.CanRead = asset.CanRead;
                                record.CanWrite = asset.CanWrite;
                                record.IsLocked = asset.IsLocked;
                                record.ResourceId = asset.ResourceId;
                                record.PolicyIds = asset.AuthPolicyIds == null ? string.Empty : string.Join(",", asset.AuthPolicyIds);
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

        private static AssetAggregate GetAsset(Asset asset)
        {
            return new AssetAggregate
            {
                Hash = asset.Hash,
                ResourceId = asset.ResourceId,
                ResourceParentHash = asset.ResourceParentHash,
                AuthPolicyIds = string.IsNullOrWhiteSpace(asset.PolicyIds) ? new string[0] : asset.PolicyIds.Split(','),
                CreatedAt = asset.CreateDateTime,
                Path = asset.Path,
                Name = asset.Name,
                CanRead = asset.CanRead,
                CanWrite = asset.CanWrite,
                IsLocked = asset.IsLocked,
                MimeType=  asset.MimeType,
                IsDefaultWorkingDirectory = asset.IsDefaultWorkingDirectory,
                Children = asset.Children == null ? new List<AssetAggregate>() : asset.Children.Select(a => GetAsset(a)).ToList()
            };
        }
    }
}
