using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.HierarchicalResource.Common.Responses;
using System.Collections.Generic;

namespace SimpleIdentityServer.HierarchicalResource.Client.Responses
{
    public class GetHierarchicalResourceResponse : BaseResponse
    {
        public IEnumerable<AssetResponse> Content { get; set; }
    }
}