using SimpleIdentityServer.ResourceManager.Core.Repositories;

namespace SimpleIdentityServer.ResourceManager.Core.Api.HierarchicalResources.Actions
{
    public interface ISearchHierarchicalResourcesAction
    {

    }

    internal sealed class SearchHierarchicalResourcesAction : ISearchHierarchicalResourcesAction
    {
        private readonly IAssetRepository _assetRepository;

        public SearchHierarchicalResourcesAction(IAssetRepository assetRepository)
        {

        }


    }
}
