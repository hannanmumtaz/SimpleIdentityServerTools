using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.Manager.Common.Responses;

namespace SimpleIdentityServer.Manager.Client.Results
{
    public class GetScopeResult : BaseResponse
    {
        public ScopeResponse Content { get; set; }
    }
}
