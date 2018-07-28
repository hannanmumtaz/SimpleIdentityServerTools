using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.Manager.Client.Configuration;
using SimpleIdentityServer.Manager.Client.Results;
using SimpleIdentityServer.Manager.Common.Requests;
using SimpleIdentityServer.Manager.Common.Responses;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Manager.Client.Clients
{
    public interface IOpenIdClients
    {
        Task<AddClientResult> ResolveAdd(Uri wellKnownConfigurationUri, AddClientRequest client, string authorizationHeaderValue = null);
        Task<BaseResponse> ResolveUpdate(Uri wellKnownConfigurationUri, UpdateClientRequest client, string authorizationHeaderValue = null);
        Task<GetClientResult> ResolveGet(Uri wellKnownConfigurationUri, string clientId, string authorizationHeaderValue = null);
        Task<BaseResponse> ResolvedDelete(Uri wellKnownConfigurationUri, string clientId, string authorizationHeaderValue = null);
        Task<GetAllClientResult> GetAll(Uri clientsUri, string authorizationHeaderValue = null);
        Task<GetAllClientResult> ResolveGetAll(Uri wellKnownConfigurationUri, string authorizationHeaderValue = null);
        Task<PagedResult<ClientResponse>> ResolveSearch(Uri wellKnownConfigurationUri, SearchClientsRequest searchClientParameter, string authorizationHeaderValue = null);
    }

    internal sealed class OpenIdClients : IOpenIdClients
    {
        private readonly IAddClientOperation _addClientOperation;
        private readonly IUpdateClientOperation _updateClientOperation;
        private readonly IGetAllClientsOperation _getAllClientsOperation;
        private readonly IDeleteClientOperation _deleteClientOperation;
        private readonly IGetClientOperation _getClientOperation;
        private readonly ISearchClientOperation _searchClientOperation;
        private readonly IConfigurationClient _configurationClient;

        public OpenIdClients(IAddClientOperation addClientOperation, IUpdateClientOperation updateClientOperation,
            IGetAllClientsOperation getAllClientsOperation, IDeleteClientOperation deleteClientOperation,
            IGetClientOperation getClientOperation, ISearchClientOperation searchClientOperation, IConfigurationClient configurationClient)
        {
            _addClientOperation = addClientOperation;
            _updateClientOperation = updateClientOperation;
            _getAllClientsOperation = getAllClientsOperation;
            _deleteClientOperation = deleteClientOperation;
            _getClientOperation = getClientOperation;
            _searchClientOperation = searchClientOperation;
            _configurationClient = configurationClient;
        }

        public async Task<AddClientResult> ResolveAdd(Uri wellKnownConfigurationUri, AddClientRequest client, string authorizationHeaderValue = null)
        {
            var configuration = await _configurationClient.GetConfiguration(wellKnownConfigurationUri).ConfigureAwait(false);
            return await _addClientOperation.ExecuteAsync(new Uri(configuration.Content.ClientsEndpoint), client, authorizationHeaderValue).ConfigureAwait(false);
        }

        public async Task<BaseResponse> ResolveUpdate(Uri wellKnownConfigurationUri, UpdateClientRequest client, string authorizationHeaderValue = null)
        {
            var configuration = await _configurationClient.GetConfiguration(wellKnownConfigurationUri).ConfigureAwait(false);
            return await _updateClientOperation.ExecuteAsync(new Uri(configuration.Content.ClientsEndpoint), client, authorizationHeaderValue).ConfigureAwait(false);
        }

        public async Task<GetClientResult> ResolveGet(Uri wellKnownConfigurationUri, string clientId, string authorizationHeaderValue = null)
        {
            var configuration = await _configurationClient.GetConfiguration(wellKnownConfigurationUri).ConfigureAwait(false);
            return await _getClientOperation.ExecuteAsync(new Uri(configuration.Content.ClientsEndpoint + "/" + clientId), authorizationHeaderValue).ConfigureAwait(false);
        }

        public async Task<BaseResponse> ResolvedDelete(Uri wellKnownConfigurationUri, string clientId, string authorizationHeaderValue = null)
        {
            var configuration = await _configurationClient.GetConfiguration(wellKnownConfigurationUri).ConfigureAwait(false);
            return await _deleteClientOperation.ExecuteAsync(new Uri(configuration.Content.ClientsEndpoint + "/" + clientId), authorizationHeaderValue).ConfigureAwait(false);
        }

        public Task<GetAllClientResult> GetAll(Uri clientsUri, string authorizationHeaderValue = null)
        {
            return _getAllClientsOperation.ExecuteAsync(clientsUri, authorizationHeaderValue);
        }

        public async Task<GetAllClientResult> ResolveGetAll(Uri wellKnownConfigurationUri, string authorizationHeaderValue = null)
        {
            var configuration = await _configurationClient.GetConfiguration(wellKnownConfigurationUri).ConfigureAwait(false);
            return await GetAll(new Uri(configuration.Content.ClientsEndpoint), authorizationHeaderValue).ConfigureAwait(false);
        }

        public async Task<PagedResult<ClientResponse>> ResolveSearch(Uri wellKnownConfigurationUri, SearchClientsRequest searchClientParameter, string authorizationHeaderValue = null)
        {
            var configuration = await _configurationClient.GetConfiguration(wellKnownConfigurationUri).ConfigureAwait(false);
            return await _searchClientOperation.ExecuteAsync(new Uri(configuration.Content.ClientsEndpoint + "/.search"), searchClientParameter, authorizationHeaderValue).ConfigureAwait(false);
        }
    }
}
