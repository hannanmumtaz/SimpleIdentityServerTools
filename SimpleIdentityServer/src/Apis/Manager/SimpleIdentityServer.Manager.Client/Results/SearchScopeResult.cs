using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.Manager.Common.Responses;

namespace SimpleIdentityServer.Manager.Client.DTOs.Responses
{
    public class SearchScopeResult : BaseResponse
    {
        public SearchScopesResponse Content { get; set; }
    }
}
