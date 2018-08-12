using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.Manager.Common.Responses;
using System.Collections.Generic;

namespace SimpleIdentityServer.Manager.Client.Results
{
    public class GetAllClaimsResult : BaseResponse
    {
        public IEnumerable<ClaimResponse> Content { get; set; }
    }
}
