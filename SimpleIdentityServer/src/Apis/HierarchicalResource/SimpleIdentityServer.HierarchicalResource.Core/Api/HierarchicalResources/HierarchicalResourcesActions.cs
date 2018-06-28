using SimpleIdentityServer.HierarchicalResource.Core.Api.HierarchicalResources.Actions;
using SimpleIdentityServer.HierarchicalResource.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleIdentityServer.HierarchicalResource.Core.Api.HierarchicalResources
{
    public interface IHierarchicalResourcesActions
    {
        Task<IEnumerable<AssetAggregate>> Get(string path, bool includeChildren = false);
    }

    internal sealed class HierarchicalResourcesActions : IHierarchicalResourcesActions
    {
        private readonly IAddHierarchicalResourcesAction _addHierarchicalResourcesAction;
        private readonly IDeleteHierarchicalResourcesAction _deleteHierarchicalResourcesAction;
        private readonly IGetHierarchicalResourceAction _getHierarchicalResourceAction;
        private readonly ISearchHierarchicalResourcesAction _searchHierarchicalResourcesAction;

        public HierarchicalResourcesActions(IAddHierarchicalResourcesAction addHierarchicalResourcesAction,
            IDeleteHierarchicalResourcesAction deleteHierarchicalResourcesAction, IGetHierarchicalResourceAction getHierarchicalResourceAction,
            ISearchHierarchicalResourcesAction searchHierarchicalResourcesAction)
        {
            _addHierarchicalResourcesAction = addHierarchicalResourcesAction;
            _deleteHierarchicalResourcesAction = deleteHierarchicalResourcesAction;
            _getHierarchicalResourceAction = getHierarchicalResourceAction;
            _searchHierarchicalResourcesAction = searchHierarchicalResourcesAction;
        }

        public Task<IEnumerable<AssetAggregate>> Get(string path, bool includeChildren = false)
        {
            return _getHierarchicalResourceAction.Execute(path, includeChildren);
        }
    }
}
