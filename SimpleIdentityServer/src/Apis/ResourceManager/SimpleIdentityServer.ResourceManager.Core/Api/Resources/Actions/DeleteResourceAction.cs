using SimpleIdentityServer.Client;
using SimpleIdentityServer.ResourceManager.Core.Helpers;
using SimpleIdentityServer.ResourceManager.Core.Models;
using SimpleIdentityServer.ResourceManager.Core.Stores;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.ResourceManager.Core.Api.Resources.Actions
{
    public interface IDeleteResourceAction
    {
        Task<bool> Execute(string subject, string resourceId);
    }

    internal sealed class DeleteResourceAction : IDeleteResourceAction
    {
        private const string _scopeName = "uma_protection";
        private readonly IEndpointHelper _endpointHelper;
        private readonly IIdentityServerUmaClientFactory _identityServerUmaClientFactory;
        private readonly ITokenStore _tokenStore;

        public DeleteResourceAction(IEndpointHelper endpointHelper, IIdentityServerUmaClientFactory identityServerUmaClientFactory, ITokenStore tokenStore)
        {
            _endpointHelper = endpointHelper;
            _identityServerUmaClientFactory = identityServerUmaClientFactory;
            _tokenStore = tokenStore;
        }

        public async Task<bool> Execute(string subject, string resourceId)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }

            if (string.IsNullOrWhiteSpace(resourceId))
            {
                throw new ArgumentNullException(nameof(resourceId));
            }

            var endpoint = await _endpointHelper.TryGetEndpointFromProfile(subject, EndpointTypes.AUTH);
            var grantedToken = await _tokenStore.GetToken(endpoint.Url, endpoint.ClientId, endpoint.ClientSecret, new[] { _scopeName });
            return await _identityServerUmaClientFactory.GetResourceSetClient().DeleteByResolution(resourceId, endpoint.Url, grantedToken.AccessToken);
        }
    }
}
