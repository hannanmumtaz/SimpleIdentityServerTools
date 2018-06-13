using SimpleIdentityServer.Parameter.Client;
using SimpleIdentityServer.Parameter.Common.DTOs.Requests;
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
    public interface IUpdateConnectorsAction
    {
        Task<bool> Execute(string subject, IEnumerable<UpdateConnectorRequest> updateParameters);
    }

    internal sealed class UpdateConnectorsAction : IUpdateConnectorsAction
    {
        private readonly IEndpointHelper _endpointHelper;
        private readonly IParameterClientFactory _parameterClientFactory;
        private readonly ITokenStore _tokenStore;

        public UpdateConnectorsAction(IEndpointHelper endpointHelper, IParameterClientFactory parameterClientFactory, ITokenStore tokenStore)
        {
            _endpointHelper = endpointHelper;
            _parameterClientFactory = parameterClientFactory;
            _tokenStore = tokenStore;
        }

        public async Task<bool> Execute(string subject, IEnumerable<UpdateConnectorRequest> updateParameters)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }

            if (updateParameters == null)
            {
                throw new ArgumentNullException(nameof(updateParameters));
            }

            var endpoint = await _endpointHelper.TryGetEndpointFromProfile(subject, EndpointTypes.OPENID);
            var uri = new Uri(endpoint.Url);
            var baseUri = uri.GetLeftPart(UriPartial.Authority);
            var grantedToken = await _tokenStore.GetToken(endpoint.AuthUrl, endpoint.ClientId, endpoint.ClientSecret, new[] { "add_parameters" });
            await _parameterClientFactory.GetParameterClient().UpdateConnectors(baseUri, updateParameters, grantedToken.AccessToken);
            return true;
        }
    }
}
