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
    public interface IGetParametersAction
    {
        Task<GetModulesResult> Execute(string subject, string type);
    }

    internal sealed class GetParametersAction : IGetParametersAction
    {
        private Dictionary<string, EndpointTypes> _mappingStrToEnum = new Dictionary<string, EndpointTypes>
        {
            { "openid", EndpointTypes.OPENID },
            { "auth", EndpointTypes.AUTH },
            { "scim", EndpointTypes.SCIM }
        };
        private readonly IEndpointHelper _endpointHelper;
        private readonly IParameterClientFactory _parameterClientFactory;
        private readonly ITokenStore _tokenStore;

        public GetParametersAction(IEndpointHelper endpointHelper, IParameterClientFactory parameterClientFactory, ITokenStore tokenStore)
        {
            _endpointHelper = endpointHelper;
            _parameterClientFactory = parameterClientFactory;
            _tokenStore = tokenStore;
        }

        public async Task<GetModulesResult> Execute(string subject, string type)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }
            
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            if(!_mappingStrToEnum.ContainsKey(type))
            {
                throw new ResourceManagerInternalException("invalid_request", $"the type {type} is not supported");
            }

            var endpoint = await _endpointHelper.TryGetEndpointFromProfile(subject, _mappingStrToEnum[type]);
            var uri = new Uri(endpoint.Url);
            var baseUri = uri.GetLeftPart(System.UriPartial.Authority);
            var grantedToken = await _tokenStore.GetToken(endpoint.AuthUrl, endpoint.ClientId, endpoint.ClientSecret, new[] { "get_parameters" });
            return await _parameterClientFactory.GetParameterClient().Get(baseUri, grantedToken.AccessToken);
        }
    }
}
