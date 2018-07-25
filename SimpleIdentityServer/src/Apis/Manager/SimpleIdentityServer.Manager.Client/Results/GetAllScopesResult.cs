using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.Manager.Common.Responses;
using System.Collections.Generic;

namespace SimpleIdentityServer.Manager.Client.Results
{
    public class GetAllScopesResult : BaseResponse
    {
        public IEnumerable<ScopeResponse> Content { get; set; }
    }
}
