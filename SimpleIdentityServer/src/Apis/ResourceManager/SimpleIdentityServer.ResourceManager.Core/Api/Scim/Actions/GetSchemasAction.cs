using Newtonsoft.Json.Linq;
using SimpleIdentityServer.ResourceManager.Core.Helpers;
using SimpleIdentityServer.Scim.Client;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.ResourceManager.Core.Api.Scim.Actions
{
    public interface IGetSchemasAction
    {
        Task<JArray> Execute(string subject);
    }

    internal sealed class GetSchemasAction : IGetSchemasAction
    {
        private readonly IScimClientFactory _scimClientFactory;
        private readonly IEndpointHelper _endpointHelper;

        public GetSchemasAction(IScimClientFactory scimClientFactory,
            IEndpointHelper endpointHelper)
        {
            _scimClientFactory = scimClientFactory;
            _endpointHelper = endpointHelper;
        }

        public async Task<JArray> Execute(string subject)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }

            var edp = await _endpointHelper.TryGetEndpointFromProfile(subject, Models.EndpointTypes.SCIM);
            // return await _scimClientFactory.GetConfigurationClient().GetSchemas(edp.Url);
            return null;
        }
    }
}
