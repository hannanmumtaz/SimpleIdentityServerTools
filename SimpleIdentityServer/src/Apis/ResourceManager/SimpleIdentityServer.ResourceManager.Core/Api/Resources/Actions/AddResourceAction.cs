using SimpleIdentityServer.Client;
using SimpleIdentityServer.ResourceManager.Core.Helpers;
using SimpleIdentityServer.ResourceManager.Core.Models;
using SimpleIdentityServer.ResourceManager.Core.Stores;
using SimpleIdentityServer.Uma.Common.DTOs;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.ResourceManager.Core.Api.Resources.Actions
{
    public interface IAddResourceAction
    {
        Task<AddResourceSetResponse> Execute(string subject, PostResourceSet postResourceSet);
    }

    internal sealed class AddResourceAction : IAddResourceAction
    {
        private const string _scopeName = "uma_protection";
        private readonly IEndpointHelper _endpointHelper;
        private readonly IIdentityServerUmaClientFactory _identityServerUmaClientFactory;
        private readonly ITokenStore _tokenStore;

        public AddResourceAction(IEndpointHelper endpointHelper, IIdentityServerUmaClientFactory identityServerUmaClientFactory, ITokenStore tokenStore)
        {
            _endpointHelper = endpointHelper;
            _identityServerUmaClientFactory = identityServerUmaClientFactory;
            _tokenStore = tokenStore;
        }

        public async Task<AddResourceSetResponse> Execute(string subject, PostResourceSet postResourceSet)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }

            if (postResourceSet == null)
            {
                throw new ArgumentNullException(nameof(postResourceSet));
            }

            var endpoint = await _endpointHelper.TryGetEndpointFromProfile(subject, EndpointTypes.AUTH);
            var grantedToken = await _tokenStore.GetToken(endpoint.Url, endpoint.ClientId, endpoint.ClientSecret, new[] { _scopeName });
            return await _identityServerUmaClientFactory.GetResourceSetClient().AddByResolution(postResourceSet, endpoint.Url, grantedToken.AccessToken);
        }
    }
}
