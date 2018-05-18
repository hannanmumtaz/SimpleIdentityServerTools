using SimpleIdentityServer.ResourceManager.Core.Api.Resources.Actions;
using SimpleIdentityServer.Uma.Common.DTOs;
using System.Threading.Tasks;

namespace SimpleIdentityServer.ResourceManager.Core.Api.Resources
{
    public interface IResourcesetActions
    {
        Task<ResourceSetResponse> Get(string subject, string resourceId);
        Task<SearchResourceSetResponse> Search(string subject, SearchResourceSet searchResourceSet);
        Task<SearchAuthPoliciesResponse> GetAuthPolicies(string subject, string resourceId);
        Task<UpdateResourceSetResponse> Update(string subject, PutResourceSet putResourceSet);
        Task<bool> Delete(string subject, string resourceId);
        Task<AddResourceSetResponse> Add(string subject, PostResourceSet postResourceSet);
    }

    internal sealed class ResourcesetActions : IResourcesetActions
    {
        private readonly ISearchResourcesetAction _searchResourcesetAction;
        private readonly IGetAuthPoliciesByResourceAction _getAuthPoliciesByResourceAction;
        private readonly IGetResourceAction _getResourceAction;
        private readonly IUpdateResourceAction _updateResourceAction;
        private readonly IDeleteResourceAction _deleteResourceAction;
        private readonly IAddResourceAction _addResourceAction;

        public ResourcesetActions(ISearchResourcesetAction searchResourcesetAction, IGetAuthPoliciesByResourceAction getAuthPoliciesByResourceAction,
            IGetResourceAction getResourceAction, IUpdateResourceAction updateResourceAction, IDeleteResourceAction deleteResourceAction,
            IAddResourceAction addResourceAction)
        {
            _searchResourcesetAction = searchResourcesetAction;
            _getAuthPoliciesByResourceAction = getAuthPoliciesByResourceAction;
            _getResourceAction = getResourceAction;
            _updateResourceAction = updateResourceAction;
            _deleteResourceAction = deleteResourceAction;
            _addResourceAction = addResourceAction;
        }

        public Task<SearchResourceSetResponse> Search(string subject, SearchResourceSet searchResourceSet)
        {
            return _searchResourcesetAction.Execute(subject, searchResourceSet);
        }

        public Task<SearchAuthPoliciesResponse> GetAuthPolicies(string subject, string resourceId)
        {
            return _getAuthPoliciesByResourceAction.Execute(subject, resourceId);
        }

        public Task<ResourceSetResponse> Get(string subject, string resourceId)
        {
            return _getResourceAction.Execute(subject, resourceId);
        }

        public Task<UpdateResourceSetResponse> Update(string subject, PutResourceSet putResourceSet)
        {
            return _updateResourceAction.Execute(subject, putResourceSet);
        }

        public Task<AddResourceSetResponse> Add(string subject, PostResourceSet postResourceSet)
        {
            return _addResourceAction.Execute(subject, postResourceSet);
        }

        public Task<bool> Delete(string subject, string resourceId)
        {
            return _deleteResourceAction.Execute(subject, resourceId);
        }
    }
}
