using SimpleIdentityServer.Parameter.Client;
using SimpleIdentityServer.Parameter.Common.DTOs.Requests;
using SimpleIdentityServer.Parameter.Common.DTOs.Results;
using SimpleIdentityServer.ResourceManager.Core.Exceptions;
using SimpleIdentityServer.ResourceManager.Core.Helpers;
using SimpleIdentityServer.ResourceManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleIdentityServer.ResourceManager.Core.Api.Parameters.Actions
{
    public interface IUpdateParametersAction
    {
        Task<bool> Execute(string subject, IEnumerable<UpdateParameterRequest> updateParameters, string type);
    }

    internal sealed class UpdateParametersAction : IUpdateParametersAction
    {
        private Dictionary<string, EndpointTypes> _mappingStrToEnum = new Dictionary<string, EndpointTypes>
        {
            { "openid", EndpointTypes.OPENID },
            { "auth", EndpointTypes.AUTH },
            { "scim", EndpointTypes.SCIM }
        };
        private readonly IEndpointHelper _endpointHelper;
        private readonly IParameterClientFactory _parameterClientFactory;

        public UpdateParametersAction(IEndpointHelper endpointHelper, IParameterClientFactory parameterClientFactory)
        {
            _endpointHelper = endpointHelper;
            _parameterClientFactory = parameterClientFactory;
        }

        public async Task<bool> Execute(string subject, IEnumerable<UpdateParameterRequest> updateParameters, string type)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }

            if (updateParameters == null)
            {
                throw new ArgumentNullException(nameof(updateParameters));
            }

            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!_mappingStrToEnum.ContainsKey(type))
            {
                throw new ResourceManagerInternalException("invalid_request", $"the type {type} is not supported");
            }

            var endpoint = await _endpointHelper.TryGetEndpointFromProfile(subject, _mappingStrToEnum[type]);
            var uri = new Uri(endpoint.Url);
            var baseUri = uri.GetLeftPart(UriPartial.Authority);
            await _parameterClientFactory.GetParameterClient().Update(baseUri, updateParameters);
            return true;
        }
    }
}
