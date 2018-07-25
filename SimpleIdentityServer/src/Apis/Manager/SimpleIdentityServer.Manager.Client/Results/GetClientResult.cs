using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.Manager.Common.Responses;

namespace SimpleIdentityServer.Manager.Client.Results
{
    public sealed class GetClientResult : BaseResponse
    {
        public ClientResponse Content { get; set; }
    }
}
