using SimpleIdentityServer.HierarchicalResource.Core.Repositories;

namespace SimpleIdentityServer.HierarchicalResource.Core.Api.HierarchicalResources.Actions
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
