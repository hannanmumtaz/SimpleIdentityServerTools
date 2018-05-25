using SimpleIdentityServer.ResourceManager.Core.Helpers;
using SimpleIdentityServer.Scim.Client;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.ResourceManager.Core.Api.Scim.Actions
{
    public interface ISearchUsersAction
    {
        Task<ScimResponse> Execute(string subject, SearchParameter parameter, string accessToken);
    }
    
    internal sealed class SearchUsersAction : ISearchUsersAction
    {
        private readonly IScimClientFactory _scimClientFactory;
        private readonly IEndpointHelper _endpointHelper;

        public SearchUsersAction(IScimClientFactory scimClientFactory, IEndpointHelper endpointHelper)
        {
            _scimClientFactory = scimClientFactory;
            _endpointHelper = endpointHelper;
        }

        public async Task<ScimResponse> Execute(string subject, SearchParameter parameter, string accessToken)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }

            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }
            
            var edp = await _endpointHelper.TryGetEndpointFromProfile(subject, Models.EndpointTypes.SCIM);
            return await _scimClientFactory.GetUserClient().SearchUsers($"{edp.Url}/Users", parameter, accessToken);
        }
    }
}