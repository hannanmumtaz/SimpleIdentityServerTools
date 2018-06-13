using SimpleIdentityServer.Parameter.Client;
using SimpleIdentityServer.Parameter.Common.DTOs.Results;
using SimpleIdentityServer.ResourceManager.Core.Exceptions;
using SimpleIdentityServer.ResourceManager.Core.Helpers;
using SimpleIdentityServer.ResourceManager.Core.Models;
using SimpleIdentityServer.ResourceManager.Core.Stores;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleIdentityServer.ResourceManager.Core.Api.Parameters.Actions
{
    public interface IGetConnectorsAction
    {
        Task<GetConnectorsResult> Execute(string subject);
    }

    internal sealed class GetConnectorsAction : IGetConnectorsAction
    {
        private readonly IEndpointHelper _endpointHelper;
        private readonly IParameterClientFactory _parameterClientFactory;
        private readonly ITokenStore _tokenStore;

        public GetConnectorsAction(IEndpointHelper endpointHelper, IParameterClientFactory parameterClientFactory, ITokenStore tokenStore)
        {
            _endpointHelper = endpointHelper;
            _parameterClientFactory = parameterClientFactory;
            _tokenStore = tokenStore;
        }

        public async Task<GetConnectorsResult> Execute(string subject)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }

            var endpoint = await _endpointHelper.TryGetEndpointFromProfile(subject, EndpointTypes.OPENID);
            var uri = new Uri(endpoint.Url);
            var baseUri = uri.GetLeftPart(System.UriPartial.Authority);
            var grantedToken = await _tokenStore.GetToken(endpoint.AuthUrl, endpoint.ClientId, endpoint.ClientSecret, new[] { "get_parameters" });
            return await _parameterClientFactory.GetParameterClient().GetConnectors(baseUri, grantedToken.AccessToken);
        }
    }
}
