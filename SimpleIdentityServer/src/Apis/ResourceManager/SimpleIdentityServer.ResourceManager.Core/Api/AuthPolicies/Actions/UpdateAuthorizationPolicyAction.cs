using SimpleIdentityServer.Client;
using SimpleIdentityServer.ResourceManager.Core.Helpers;
using SimpleIdentityServer.ResourceManager.Core.Models;
using SimpleIdentityServer.ResourceManager.Core.Stores;
using SimpleIdentityServer.Uma.Common.DTOs;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.ResourceManager.Core.Api.AuthPolicies.Actions
{
    public interface IUpdateAuthorizationPolicyAction
    {
        Task<bool> Execute(string subject, PutPolicy putPolicy);
    }

    internal sealed class UpdateAuthorizationPolicyAction : IUpdateAuthorizationPolicyAction
    {
        private const string _scopeName = "uma_protection";
        private readonly IEndpointHelper _endpointHelper;
        private readonly IIdentityServerUmaClientFactory _identityServerUmaClientFactory;
        private readonly ITokenStore _tokenStore;

        public UpdateAuthorizationPolicyAction(IEndpointHelper endpointHelper, IIdentityServerUmaClientFactory identityServerUmaClientFactory, ITokenStore tokenStore)
        {
            _endpointHelper = endpointHelper;
            _identityServerUmaClientFactory = identityServerUmaClientFactory;
            _tokenStore = tokenStore;
        }

        public async Task<bool> Execute(string subject, PutPolicy putPolicy)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }

            if (putPolicy == null)
            {
                throw new ArgumentNullException(nameof(putPolicy));
            }

            var endpoint = await _endpointHelper.TryGetEndpointFromProfile(subject, EndpointTypes.AUTH);
            var grantedToken = await _tokenStore.GetToken(endpoint.Url, endpoint.ClientId, endpoint.ClientSecret, new[] { _scopeName });
            return await _identityServerUmaClientFactory.GetPolicyClient().UpdateByResolution(putPolicy, endpoint.Url, grantedToken.AccessToken);
        }
    }
}
