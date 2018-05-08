using SimpleIdentityServer.Manager.Common.Responses;

namespace SimpleIdentityServer.Manager.Client.DTOs.Responses
{
    public class SearchClaimResponse : BaseResponse
    {
        public SearchClaimResponse() { }

        public SearchClaimResponse(ErrorResponse error)
        {
            Error = error;
            ContainsError = true;
        }

        public SearchClaimsResponse Content { get; set; }
    }
}
