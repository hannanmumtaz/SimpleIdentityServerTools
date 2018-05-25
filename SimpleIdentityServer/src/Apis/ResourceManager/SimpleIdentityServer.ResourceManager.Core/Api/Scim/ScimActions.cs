using Newtonsoft.Json.Linq;
using SimpleIdentityServer.ResourceManager.Core.Api.Scim.Actions;
using SimpleIdentityServer.Scim.Client;
using System.Threading.Tasks;

namespace SimpleIdentityServer.ResourceManager.Core.Api.Scim
{
    public interface IScimActions
    {
        Task<JArray> GetSchemas(string subject);
        Task<ScimResponse> SearchGroups(string subject, SearchParameter parameter, string accessToken);
        Task<ScimResponse> SearchUsers(string subject, SearchParameter parameter, string accessToken);
    }

    internal sealed class ScimActions : IScimActions
    {
        private readonly IGetSchemasAction _getSchemasAction;
	    private readonly ISearchUsersAction _searchUsersAction;
	    private readonly ISearchGroupsAction _searchGroupsAction;

        public ScimActions(IGetSchemasAction getSchemasAction, ISearchUsersAction searchUsersAction, ISearchGroupsAction searchGroupsAction)
        {
            _getSchemasAction = getSchemasAction;
	        _searchUsersAction = searchUsersAction;
	        _searchGroupsAction = searchGroupsAction;
        }

        public Task<JArray> GetSchemas(string subject)
        {
            return _getSchemasAction.Execute(subject);
        }

        public Task<ScimResponse> SearchGroups(string subject, SearchParameter parameter, string accessToken)
        {
            return _searchGroupsAction.Execute(subject, parameter, accessToken);
        }

        public Task<ScimResponse> SearchUsers(string subject, SearchParameter parameter, string accessToken)
        {
            return _searchUsersAction.Execute(subject, parameter, accessToken);
        }
    }
}
