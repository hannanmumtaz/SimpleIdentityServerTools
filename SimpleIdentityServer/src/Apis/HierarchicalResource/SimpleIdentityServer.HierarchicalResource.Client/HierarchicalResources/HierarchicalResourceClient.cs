using SimpleIdentityServer.HierarchicalResource.Client.Configuration;
using SimpleIdentityServer.HierarchicalResource.Client.Responses;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.HierarchicalResource.Client.HierarchicalResources
{
    public interface IHierarchicalResourceClient
    {
        Task<GetHierarchicalResourceResponse> Get(Uri configurationUri, string name, bool includeChildren, string authorizationHeaderValue = null);
    }

    internal sealed class HierarchicalResourceClient : IHierarchicalResourceClient
    {
        private readonly IGetHierarchicalResourceOperation _getHierarchicalResourceOperation;
        private readonly IConfigurationClient _configurationClient;

        public HierarchicalResourceClient(IGetHierarchicalResourceOperation getHierarchicalResourceOperation, IConfigurationClient configurationClient)
        {
            _getHierarchicalResourceOperation = getHierarchicalResourceOperation;
            _configurationClient = configurationClient;
        }

        public async Task<GetHierarchicalResourceResponse> Get(Uri configurationUri, string name, bool includeChildren, string authorizationHeaderValue = null)
        {
            var configuration = await _configurationClient.GetConfiguration(configurationUri);
            return await _getHierarchicalResourceOperation.Execute(configuration.HierarchicalresourcesEdp, name, includeChildren, authorizationHeaderValue);
        }
    }
}
