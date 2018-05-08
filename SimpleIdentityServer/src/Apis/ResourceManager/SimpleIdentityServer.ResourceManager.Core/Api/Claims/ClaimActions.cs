using SimpleIdentityServer.Manager.Client.DTOs.Responses;
using SimpleIdentityServer.Manager.Common.Requests;
using SimpleIdentityServer.Manager.Common.Responses;
using SimpleIdentityServer.ResourceManager.Core.Api.Claims.Actions;
using System.Threading.Tasks;

namespace SimpleIdentityServer.ResourceManager.Core.Api.Claims
{
    public interface IClaimActions
    {
        Task<BaseResponse> Add(string subject, ClaimResponse request);
        Task<BaseResponse> Delete(string subject, string claim);
        Task<GetClaimResponse> Get(string subject, string claim);
        Task<SearchClaimResponse> Search(string subject, SearchClaimsRequest searchClaimsRequest);
    }

    internal sealed class ClaimActions : IClaimActions
    {
        private readonly IAddClaimAction _addClaimAction;
        private readonly IDeleteClaimAction _deleteClaimAction;
        private readonly IGetClaimAction _getClaimAction;
        private readonly ISearchClaimsAction _searchClaimsAction;

        public ClaimActions(IAddClaimAction addClaimAction, IDeleteClaimAction deleteClaimAction, IGetClaimAction getClaimAction, ISearchClaimsAction searchClaimsAction)
        {
            _addClaimAction = addClaimAction;
            _deleteClaimAction = deleteClaimAction;
            _getClaimAction = getClaimAction;
            _searchClaimsAction = searchClaimsAction;
        }

        public Task<BaseResponse> Add(string subject, ClaimResponse request)
        {
            return _addClaimAction.Execute(subject, request);
        }

        public Task<BaseResponse> Delete(string subject, string claim)
        {
            return _deleteClaimAction.Execute(subject, claim);
        }

        public Task<GetClaimResponse> Get(string subject, string claim)
        {
            return _getClaimAction.Execute(subject, claim);
        }

        public Task<SearchClaimResponse> Search(string subject, SearchClaimsRequest searchClaimsRequest)
        {
            return _searchClaimsAction.Execute(subject, searchClaimsRequest);
        }
    }
}
