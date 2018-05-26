using SimpleIdentityServer.ResourceManager.Core.Helpers;
using SimpleIdentityServer.ResourceManager.Core.Stores;
using SimpleIdentityServer.Scim.Client;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.ResourceManager.Core.Api.Scim.Actions
{
    public interface ISearchGroupsAction
    {
        Task<ScimResponse> Execute(string subject, SearchParameter parameter);
    }
    
    internal sealed class SearchGroupsAction : ISearchGroupsAction
    {
        private readonly IScimClientFactory _scimClientFactory;
        private readonly IEndpointHelper _endpointHelper;
        private readonly ITokenStore _tokenStore;

        public SearchGroupsAction(IScimClientFactory scimClientFactory, IEndpointHelper endpointHelper, ITokenStore tokenStore)
        {
            _scimClientFactory = scimClientFactory;
            _endpointHelper = endpointHelper;
            _tokenStore = tokenStore;
        }

        public async Task<ScimResponse> Execute(string subject, SearchParameter parameter)
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
            var grantedToken = await _tokenStore.GetToken(edp.AuthUrl, edp.ClientId, edp.ClientSecret, new[] { Constants.READ_SCIM_SCOPE });
            return await _scimClientFactory.GetUserClient().SearchUsers(edp.Url, parameter, grantedToken.AccessToken);
        }
    }
}