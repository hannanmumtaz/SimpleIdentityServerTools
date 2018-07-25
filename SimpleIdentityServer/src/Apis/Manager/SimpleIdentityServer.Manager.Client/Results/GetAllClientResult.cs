using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.Manager.Common.Responses;
using System.Collections.Generic;

namespace SimpleIdentityServer.Manager.Client.Results
{
    public class GetAllClientResult : BaseResponse
    {
        public IEnumerable<ClientResponse> Content { get; set; }
    }
}
