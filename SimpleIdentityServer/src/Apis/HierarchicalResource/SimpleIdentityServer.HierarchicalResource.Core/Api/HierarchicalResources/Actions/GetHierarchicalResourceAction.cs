using SimpleIdentityServer.HierarchicalResource.Core.Models;
using SimpleIdentityServer.HierarchicalResource.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleIdentityServer.HierarchicalResource.Core.Api.HierarchicalResources.Actions
{
    public interface IGetHierarchicalResourceAction
    {
        Task<IEnumerable<AssetAggregate>> Execute(string hash, bool includeChildren = false);
    }

    internal sealed class GetHierarchicalResourceAction : IGetHierarchicalResourceAction
    {
        private readonly IAssetRepository _assetRepository;

        public GetHierarchicalResourceAction(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }

        public async Task<IEnumerable<AssetAggregate>> Execute(string hash, bool includeChildren = false)
        {
            if (string.IsNullOrWhiteSpace(hash))
            {
                throw new ArgumentNullException(nameof(hash));
            }

            return await _assetRepository.Get(new[] { hash }, includeChildren);
        }
    }
}