using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.Module.Feed.Client.Results;
using SimpleIdentityServer.Module.Feed.Common.Requests;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Module.Feed.Client.Connectors
{
    public interface IConnectorClient
    {
        Task<GetConnectorResult> Get(string id, string url);
        Task<GetAllConnectorsResult> GetAll(string url);
        Task<BaseResponse> Add(AddConnectorRequest request, string url);
        Task<BaseResponse> Delete(string id, string url);
    }

    internal sealed class ConnectorClient : IConnectorClient
    {
        private readonly IConfigurationClient _configurationClient;
        private readonly IGetConnectorOperation _getConnectorOperation;
        private readonly IGetAllConnectorsOperation _getAllConnectorsOperation;
        private readonly IAddConnectorOperation _addConnectorOperation;
        private readonly IDeleteConnectorOperation _deleteConnectorOperation;

        public ConnectorClient(IConfigurationClient configurationClient, IGetConnectorOperation getConnectorOperation, IGetAllConnectorsOperation getAllConnectorsOperation,
            IAddConnectorOperation addConnectorOperation, IDeleteConnectorOperation deleteConnectorOperation)
        {
            _configurationClient = configurationClient;
            _getConnectorOperation = getConnectorOperation;
            _getAllConnectorsOperation = getAllConnectorsOperation;
            _addConnectorOperation = addConnectorOperation;
            _deleteConnectorOperation = deleteConnectorOperation;
        }

        public async Task<GetConnectorResult> Get(string id, string url)
        {
            var configuration = await _configurationClient.GetConfiguration(url);
            return await _getConnectorOperation.Execute(id, configuration.ConnectorsEndpoint);
        }

        public async Task<GetAllConnectorsResult> GetAll(string url)
        {
            var configuration = await _configurationClient.GetConfiguration(url);
            return await _getAllConnectorsOperation.Execute(configuration.ConnectorsEndpoint);
        }

        public async Task<BaseResponse> Add(AddConnectorRequest request, string url)
        {
            var configuration = await _configurationClient.GetConfiguration(url);
            return await _addConnectorOperation.Execute(request, configuration.ConnectorsEndpoint);
        }

        public async Task<BaseResponse> Delete(string id, string url)
        {
            var configuration = await _configurationClient.GetConfiguration(url);
            return await _deleteConnectorOperation.Execute(id, configuration.ConnectorsEndpoint);
        }
    }
}
