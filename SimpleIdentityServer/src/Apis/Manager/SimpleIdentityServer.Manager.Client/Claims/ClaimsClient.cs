using SimpleIdentityServer.Manager.Client.Configuration;
using SimpleIdentityServer.Manager.Client.DTOs.Responses;
using SimpleIdentityServer.Manager.Common.Requests;
using SimpleIdentityServer.Manager.Common.Responses;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Manager.Client.Claims
{
    public interface IClaimsClient
    {
        Task<BaseResponse> Add(Uri wellKnownConfigurationUri, ClaimResponse claim, string authorizationHeaderValue = null);
        Task<GetClaimResponse> Get(Uri wellKnownConfigurationUri, string claimId, string authorizationHeaderValue = null);
        Task<BaseResponse> Delete(Uri wellKnownConfigurationUri, string claimId, string authorizationHeaderValue = null);
        Task<SearchClaimResponse> Search(Uri wellKnownConfigurationUri, SearchClaimsRequest searchClaimsRequest, string authorizationHeaderValue = null);
    }

    internal sealed class ClaimsClient : IClaimsClient
    {
        private readonly IAddClaimOperation _addClaimOperation;
        private readonly IDeleteClaimOperation _deleteClaimOperation;
        private readonly IGetClaimOperation _getClaimOperation;
        private readonly ISearchClaimsOperation _searchClaimsOperation;
        private readonly IConfigurationClient _configurationClient;

        public ClaimsClient(IAddClaimOperation addClaimOperation, IDeleteClaimOperation deleteClaimOperation, IGetClaimOperation getClaimOperation,
            ISearchClaimsOperation searchClaimsOperation, IConfigurationClient configurationClient)
        {
            _addClaimOperation = addClaimOperation;
            _deleteClaimOperation = deleteClaimOperation;
            _getClaimOperation = getClaimOperation;
            _searchClaimsOperation = searchClaimsOperation;
            _configurationClient = configurationClient;
        }

        public async Task<BaseResponse> Add(Uri wellKnownConfigurationUri, ClaimResponse claim, string authorizationHeaderValue = null)
        {
            var configuration = await _configurationClient.GetConfiguration(wellKnownConfigurationUri);
            return await _addClaimOperation.ExecuteAsync(new Uri(configuration.ClaimsEndpoint), claim, authorizationHeaderValue);
        }
        
        public async Task<GetClaimResponse> Get(Uri wellKnownConfigurationUri, string claimId, string authorizationHeaderValue = null)
        {
            var configuration = await _configurationClient.GetConfiguration(wellKnownConfigurationUri);
            return await _getClaimOperation.ExecuteAsync(new Uri(configuration.ClaimsEndpoint + "/" + claimId), authorizationHeaderValue);
        }

        public async Task<BaseResponse> Delete(Uri wellKnownConfigurationUri, string claimId, string authorizationHeaderValue = null)
        {
            var configuration = await _configurationClient.GetConfiguration(wellKnownConfigurationUri);
            return await _deleteClaimOperation.ExecuteAsync(new Uri(configuration.ClaimsEndpoint + "/" + claimId), authorizationHeaderValue);
        }

        public async Task<SearchClaimResponse> Search(Uri wellKnownConfigurationUri, SearchClaimsRequest searchClaimsRequest, string authorizationHeaderValue = null)
        {
            var configuration = await _configurationClient.GetConfiguration(wellKnownConfigurationUri);
            return await _searchClaimsOperation.ExecuteAsync(new Uri(configuration.ClaimsEndpoint + "/.search"), searchClaimsRequest, authorizationHeaderValue);
        }
    }
}
