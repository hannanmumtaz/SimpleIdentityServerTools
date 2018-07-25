using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.Manager.Common.Responses;
using System.Collections.Generic;

namespace SimpleIdentityServer.Manager.Client.Results
{
    public class GetAllResourceOwnersResult : BaseResponse
    {
        public IEnumerable<ResourceOwnerResponse> Content { get; set; }
    }
}
