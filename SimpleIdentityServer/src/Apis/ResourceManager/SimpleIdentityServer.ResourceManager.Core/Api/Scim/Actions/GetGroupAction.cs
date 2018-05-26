using SimpleIdentityServer.ResourceManager.Core.Helpers;
using SimpleIdentityServer.ResourceManager.Core.Stores;
using SimpleIdentityServer.Scim.Client;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.ResourceManager.Core.Api.Scim.Actions
{
    public interface IGetGroupAction
    {
        Task<ScimResponse> Execute(string subject, string userId);
    }

    internal sealed class GetGroupAction : IGetGroupAction
    {
        private readonly IScimClientFactory _scimClientFactory;
        private readonly IEndpointHelper _endpointHelper;
        private readonly ITokenStore _tokenStore;

        public GetGroupAction(IScimClientFactory scimClientFactory, IEndpointHelper endpointHelper, ITokenStore tokenStore)
        {
            _scimClientFactory = scimClientFactory;
            _endpointHelper = endpointHelper;
            _tokenStore = tokenStore;
        }

        public async Task<ScimResponse> Execute(string subject, string groupId)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }

            if (string.IsNullOrWhiteSpace(groupId))
            {
                throw new ArgumentNullException(nameof(groupId));
            }

            var edp = await _endpointHelper.TryGetEndpointFromProfile(subject, Models.EndpointTypes.SCIM);
            var grantedToken = await _tokenStore.GetToken(edp.AuthUrl, edp.ClientId, edp.ClientSecret, new[] { Constants.READ_SCIM_SCOPE });
            return await _scimClientFactory.GetGroupClient().GetGroup(edp.Url, groupId, grantedToken.AccessToken);
        }
    }
}
