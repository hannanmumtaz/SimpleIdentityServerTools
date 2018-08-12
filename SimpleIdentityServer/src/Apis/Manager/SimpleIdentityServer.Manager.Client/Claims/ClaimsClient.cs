using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.Manager.Client.Configuration;
using SimpleIdentityServer.Manager.Client.Results;
using SimpleIdentityServer.Manager.Common.Requests;
using SimpleIdentityServer.Manager.Common.Responses;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Manager.Client.Claims
{
    public interface IClaimsClient
    {
        Task<BaseResponse> Add(Uri wellKnownConfigurationUri, ClaimResponse claim, string authorizationHeaderValue = null);
        Task<GetClaimResult> Get(Uri wellKnownConfigurationUri, string claimId, string authorizationHeaderValue = null);
        Task<BaseResponse> Delete(Uri wellKnownConfigurationUri, string claimId, string authorizationHeaderValue = null);
        Task<PagedResult<ClaimResponse>> Search(Uri wellKnownConfigurationUri, SearchClaimsRequest searchClaimsRequest, string authorizationHeaderValue = null);
        Task<GetAllClaimsResult> GetAll(Uri wellKnownConfigurationUri, string authorizationHeaderValue = null);
    }

    internal sealed class ClaimsClient : IClaimsClient
    {
        private readonly IAddClaimOperation _addClaimOperation;
        private readonly IDeleteClaimOperation _deleteClaimOperation;
        private readonly IGetClaimOperation _getClaimOperation;
        private readonly ISearchClaimsOperation _searchClaimsOperation;
        private readonly IConfigurationClient _configurationClient;
        private readonly IGetAllClaimsOperation _getAllClaimsOperation;

        public ClaimsClient(IAddClaimOperation addClaimOperation, IDeleteClaimOperation deleteClaimOperation, IGetClaimOperation getClaimOperation,
            ISearchClaimsOperation searchClaimsOperation, IConfigurationClient configurationClient,
            IGetAllClaimsOperation getAllClaimsOperation)
        {
            _addClaimOperation = addClaimOperation;
            _deleteClaimOperation = deleteClaimOperation;
            _getClaimOperation = getClaimOperation;
            _searchClaimsOperation = searchClaimsOperation;
            _configurationClient = configurationClient;
            _getAllClaimsOperation = getAllClaimsOperation;
        }

        public async Task<BaseResponse> Add(Uri wellKnownConfigurationUri, ClaimResponse claim, string authorizationHeaderValue = null)
        {
            var configuration = await _configurationClient.GetConfiguration(wellKnownConfigurationUri).ConfigureAwait(false);
            return await _addClaimOperation.ExecuteAsync(new Uri(configuration.Content.ClaimsEndpoint), claim, authorizationHeaderValue).ConfigureAwait(false);
        }
        
        public async Task<GetClaimResult> Get(Uri wellKnownConfigurationUri, string claimId, string authorizationHeaderValue = null)
        {
            var configuration = await _configurationClient.GetConfiguration(wellKnownConfigurationUri).ConfigureAwait(false);
            return await _getClaimOperation.ExecuteAsync(new Uri(configuration.Content.ClaimsEndpoint + "/" + claimId), authorizationHeaderValue).ConfigureAwait(false);
        }

        public async Task<BaseResponse> Delete(Uri wellKnownConfigurationUri, string claimId, string authorizationHeaderValue = null)
        {
            var configuration = await _configurationClient.GetConfiguration(wellKnownConfigurationUri).ConfigureAwait(false);
            return await _deleteClaimOperation.ExecuteAsync(new Uri(configuration.Content.ClaimsEndpoint + "/" + claimId), authorizationHeaderValue).ConfigureAwait(false);
        }

        public async Task<PagedResult<ClaimResponse>> Search(Uri wellKnownConfigurationUri, SearchClaimsRequest searchClaimsRequest, string authorizationHeaderValue = null)
        {
            var configuration = await _configurationClient.GetConfiguration(wellKnownConfigurationUri).ConfigureAwait(false);
            return await _searchClaimsOperation.ExecuteAsync(new Uri(configuration.Content.ClaimsEndpoint + "/.search"), searchClaimsRequest, authorizationHeaderValue).ConfigureAwait(false);
        }

        public async Task<GetAllClaimsResult> GetAll(Uri wellKnownConfigurationUri, string authorizationHeaderValue = null)
        {
            var configuration = await _configurationClient.GetConfiguration(wellKnownConfigurationUri).ConfigureAwait(false);
            return await _getAllClaimsOperation.ExecuteAsync(new Uri(configuration.Content.ClaimsEndpoint), authorizationHeaderValue);
        }
    }
}
