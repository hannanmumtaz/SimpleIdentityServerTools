using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.Manager.Client.Configuration;
using SimpleIdentityServer.Manager.Client.Results;
using SimpleIdentityServer.Manager.Common.Requests;
using SimpleIdentityServer.Manager.Common.Responses;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Manager.Client.Scopes
{
    public interface IScopeClient
    {
        Task<BaseResponse> ResolveAdd(Uri wellKnownConfigurationUri, ScopeResponse scope, string authorizationHeaderValue = null);
        Task<BaseResponse> ResolveUpdate(Uri wellKnownConfigurationUri, ScopeResponse client, string authorizationHeaderValue = null);
        Task<GetScopeResult> ResolveGet(Uri wellKnownConfigurationUri, string scopeId, string authorizationHeaderValue = null);
        Task<BaseResponse> ResolvedDelete(Uri wellKnownConfigurationUri, string scopeId, string authorizationHeaderValue = null);
        Task<GetAllScopesResult> GetAll(Uri scopesUri, string authorizationHeaderValue = null);
        Task<GetAllScopesResult> ResolveGetAll(Uri wellKnownConfigurationUri, string authorizationHeaderValue = null);
        Task<PagedResult<ScopeResponse>> ResolveSearch(Uri wellKnownConfigurationUri, SearchScopesRequest searchScopesParameter, string authorizationHeaderValue = null);
    }

    internal sealed class ScopeClient : IScopeClient
    {
        private readonly IAddScopeOperation _addScopeOperation;
        private readonly IDeleteScopeOperation _deleteScopeOperation;
        private readonly IGetAllScopesOperation _getAllScopesOperation;
        private readonly IGetScopeOperation _getScopeOperation;
        private readonly IUpdateScopeOperation _updateScopeOperation;
        private readonly IConfigurationClient _configurationClient;
        private readonly ISearchScopesOperation _searchScopesOperation;

        public ScopeClient(IAddScopeOperation addScopeOperation, IDeleteScopeOperation deleteScopeOperation, IGetAllScopesOperation getAllScopesOperation, IGetScopeOperation getScopeOperation, 
            IUpdateScopeOperation updateScopeOperation, IConfigurationClient configurationClient, ISearchScopesOperation searchScopesOperation)
        {
            _addScopeOperation = addScopeOperation;
            _deleteScopeOperation = deleteScopeOperation;
            _getAllScopesOperation = getAllScopesOperation;
            _getScopeOperation = getScopeOperation;
            _updateScopeOperation = updateScopeOperation;
            _configurationClient = configurationClient;
            _searchScopesOperation = searchScopesOperation;
        }

        public async Task<BaseResponse> ResolveAdd(Uri wellKnownConfigurationUri, ScopeResponse scope, string authorizationHeaderValue = null)
        {
            var configuration = await _configurationClient.GetConfiguration(wellKnownConfigurationUri).ConfigureAwait(false);
            return await _addScopeOperation.ExecuteAsync(new Uri(configuration.Content.ScopesEndpoint), scope, authorizationHeaderValue).ConfigureAwait(false);
        }

        public async Task<BaseResponse> ResolveUpdate(Uri wellKnownConfigurationUri, ScopeResponse client, string authorizationHeaderValue = null)
        {
            var configuration = await _configurationClient.GetConfiguration(wellKnownConfigurationUri).ConfigureAwait(false);
            return await _updateScopeOperation.ExecuteAsync(new Uri(configuration.Content.ScopesEndpoint), client, authorizationHeaderValue).ConfigureAwait(false);
        }

        public async Task<GetScopeResult> ResolveGet(Uri wellKnownConfigurationUri, string scopeId, string authorizationHeaderValue = null)
        {
            var configuration = await _configurationClient.GetConfiguration(wellKnownConfigurationUri).ConfigureAwait(false);
            return await _getScopeOperation.ExecuteAsync(new Uri(configuration.Content.ScopesEndpoint + "/" + scopeId), authorizationHeaderValue).ConfigureAwait(false);
        }

        public async Task<BaseResponse> ResolvedDelete(Uri wellKnownConfigurationUri, string scopeId, string authorizationHeaderValue = null)
        {
            var configuration = await _configurationClient.GetConfiguration(wellKnownConfigurationUri).ConfigureAwait(false);
            return await _deleteScopeOperation.ExecuteAsync(new Uri(configuration.Content.ScopesEndpoint + "/" + scopeId), authorizationHeaderValue).ConfigureAwait(false);
        }

        public Task<GetAllScopesResult> GetAll(Uri scopesUri, string authorizationHeaderValue = null)
        {
            return _getAllScopesOperation.ExecuteAsync(scopesUri, authorizationHeaderValue);
        }

        public async Task<GetAllScopesResult> ResolveGetAll(Uri wellKnownConfigurationUri, string authorizationHeaderValue = null)
        {
            var configuration = await _configurationClient.GetConfiguration(wellKnownConfigurationUri).ConfigureAwait(false);
            return await GetAll(new Uri(configuration.Content.ScopesEndpoint), authorizationHeaderValue).ConfigureAwait(false);
        }

        public async Task<PagedResult<ScopeResponse>> ResolveSearch(Uri wellKnownConfigurationUri, SearchScopesRequest searchScopesParameter, string authorizationHeaderValue = null)
        {
            var configuration = await _configurationClient.GetConfiguration(wellKnownConfigurationUri).ConfigureAwait(false);
            return await _searchScopesOperation.ExecuteAsync(new Uri(configuration.Content.ScopesEndpoint + "/.search"), searchScopesParameter, authorizationHeaderValue).ConfigureAwait(false);
        }
    }
}
