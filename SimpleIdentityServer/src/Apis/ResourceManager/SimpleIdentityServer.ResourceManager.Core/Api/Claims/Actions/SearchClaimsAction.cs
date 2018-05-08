using SimpleIdentityServer.Manager.Client;
using SimpleIdentityServer.Manager.Client.DTOs.Responses;
using SimpleIdentityServer.Manager.Common.Requests;
using SimpleIdentityServer.ResourceManager.Core.Helpers;
using SimpleIdentityServer.ResourceManager.Core.Stores;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.ResourceManager.Core.Api.Claims.Actions
{
    public interface ISearchClaimsAction
    {
        Task<SearchClaimResponse> Execute(string subject, SearchClaimsRequest searchClaimsRequest);
    }

    internal sealed class SearchClaimsAction : ISearchClaimsAction
    {
        private readonly IOpenIdManagerClientFactory _openIdManagerClientFactory;
        private readonly IEndpointHelper _endpointHelper;
        private readonly ITokenStore _tokenStore;

        public SearchClaimsAction(IOpenIdManagerClientFactory openIdManagerClientFactory,
            IEndpointHelper endpointHelper, ITokenStore tokenStore)
        {
            _openIdManagerClientFactory = openIdManagerClientFactory;
            _endpointHelper = endpointHelper;
            _tokenStore = tokenStore;
        }

        public async Task<SearchClaimResponse> Execute(string subject, SearchClaimsRequest searchClaimsRequest)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }

            if (searchClaimsRequest == null)
            {
                throw new ArgumentNullException(nameof(searchClaimsRequest));
            }

            var endpoint = await _endpointHelper.TryGetEndpointFromProfile(subject, Models.EndpointTypes.OPENID);
            var grantedToken = await _tokenStore.GetToken(endpoint.AuthUrl, endpoint.ClientId, endpoint.ClientSecret, new[] { Constants.MANAGER_SCOPE });
            return await _openIdManagerClientFactory.GetClaimsClient().Search(new Uri(endpoint.ManagerUrl), searchClaimsRequest, grantedToken.AccessToken);
        }
    }
}
