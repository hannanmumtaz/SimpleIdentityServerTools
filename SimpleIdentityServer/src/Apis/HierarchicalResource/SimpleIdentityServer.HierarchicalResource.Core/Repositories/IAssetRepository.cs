using SimpleIdentityServer.HierarchicalResource.Core.Models;
using SimpleIdentityServer.HierarchicalResource.Core.Parameters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleIdentityServer.HierarchicalResource.Core.Repositories
{
    public interface IAssetRepository
    {
        Task<IEnumerable<AssetAggregate>> Search(SearchAssetsParameter parameter);
        Task<IEnumerable<AssetAggregate>> GetAllParents(string hash);
        Task<IEnumerable<AssetAggregate>> GetAllChildren(string hash);
        Task<AssetAggregate> Get(string hash);
        Task<IEnumerable<AssetAggregate>> Get(IEnumerable<string> pathLst, bool includeChildren);
        Task<bool> Add(IEnumerable<AssetAggregate> asset);
        Task<bool> Remove(IEnumerable<string> hashLst);
        Task<bool> Update(IEnumerable<AssetAggregate> assets);
    }
}
