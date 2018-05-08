using SimpleIdentityServer.Manager.Common.Responses;

namespace SimpleIdentityServer.Manager.Client.DTOs.Responses
{
    public class GetClaimResponse : BaseResponse
    {
        public GetClaimResponse() { }

        public GetClaimResponse(ErrorResponse error)
        {
            Error = error;
            ContainsError = true;
        }

        public ClaimResponse Content { get; set; }
    }
}
