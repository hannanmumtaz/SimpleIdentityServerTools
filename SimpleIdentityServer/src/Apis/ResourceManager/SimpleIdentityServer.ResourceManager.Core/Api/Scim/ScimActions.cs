using Newtonsoft.Json.Linq;
using SimpleIdentityServer.ResourceManager.Core.Api.Scim.Actions;
using SimpleIdentityServer.Scim.Client;
using System.Threading.Tasks;

namespace SimpleIdentityServer.ResourceManager.Core.Api.Scim
{
    public interface IScimActions
    {
        Task<JArray> GetSchemas(string subject);
        Task<ScimResponse> SearchGroups(string subject, SearchParameter parameter);
        Task<ScimResponse> SearchUsers(string subject, SearchParameter parameter);
        Task<ScimResponse> GetGroup(string subject, string groupId);
        Task<ScimResponse> GetUser(string subject, string userId);
    }

    internal sealed class ScimActions : IScimActions
    {
        private readonly IGetSchemasAction _getSchemasAction;
	    private readonly ISearchUsersAction _searchUsersAction;
	    private readonly ISearchGroupsAction _searchGroupsAction;
        private readonly IGetGroupAction _getGroupAction;
        private readonly IGetUserAction _getUserAction;

        public ScimActions(IGetSchemasAction getSchemasAction, ISearchUsersAction searchUsersAction, 
            ISearchGroupsAction searchGroupsAction, IGetGroupAction getGroupAction, IGetUserAction getUserAction)
        {
            _getSchemasAction = getSchemasAction;
	        _searchUsersAction = searchUsersAction;
	        _searchGroupsAction = searchGroupsAction;
            _getGroupAction = getGroupAction;
            _getUserAction = getUserAction;
        }

        public Task<JArray> GetSchemas(string subject)
        {
            return _getSchemasAction.Execute(subject);
        }

        public Task<ScimResponse> SearchGroups(string subject, SearchParameter parameter)
        {
            return _searchGroupsAction.Execute(subject, parameter);
        }

        public Task<ScimResponse> SearchUsers(string subject, SearchParameter parameter)
        {
            return _searchUsersAction.Execute(subject, parameter);
        }

        public Task<ScimResponse> GetGroup(string subject, string groupId)
        {
            return _getGroupAction.Execute(subject, groupId);
        }

        public Task<ScimResponse> GetUser(string subject, string userId)
        {
            return _getUserAction.Execute(subject, userId);
        }
    }
}
