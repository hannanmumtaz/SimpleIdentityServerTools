using SimpleIdentityServer.ResourceManager.Core.Api.HierarchicalResources.Actions;

namespace SimpleIdentityServer.ResourceManager.Core.Api.HierarchicalResources
{
    public interface IHierarchicalResourcesActions
    {

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
    }
}
