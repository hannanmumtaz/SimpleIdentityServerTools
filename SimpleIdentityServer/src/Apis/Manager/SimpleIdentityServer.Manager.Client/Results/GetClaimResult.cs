using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.Manager.Common.Responses;

namespace SimpleIdentityServer.Manager.Client.Results
{
    public class GetClaimResult : BaseResponse
    {
        public ClaimResponse Content { get; set; }
    }
}
