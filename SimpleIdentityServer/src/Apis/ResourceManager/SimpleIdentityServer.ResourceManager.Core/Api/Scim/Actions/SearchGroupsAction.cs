using SimpleIdentityServer.ResourceManager.Core.Helpers;
using SimpleIdentityServer.Scim.Client;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.ResourceManager.Core.Api.Scim.Actions
{
    public interface ISearchGroupsAction
    {
        Task<ScimResponse> Execute(string subject, SearchParameter parameter, string accessToken);
    }
    
    internal sealed class SearchGroupsAction : ISearchGroupsAction
    {
        private readonly IScimClientFactory _scimClientFactory;
        private readonly IEndpointHelper _endpointHelper;

        public SearchGroupsAction(IScimClientFactory scimClientFactory, IEndpointHelper endpointHelper)
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
            return await _scimClientFactory.GetUserClient().SearchUsers($"{edp.Url}/Groups", parameter, accessToken);
        }
    }
}