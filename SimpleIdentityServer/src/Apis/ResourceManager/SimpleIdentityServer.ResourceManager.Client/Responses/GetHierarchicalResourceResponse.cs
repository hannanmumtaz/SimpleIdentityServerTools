using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.ResourceManager.Common.Responses;
using System.Collections.Generic;

namespace SimpleIdentityServer.ResourceManager.Client.Responses
{
    public class GetHierarchicalResourceResponse : BaseResponse
    {
        public IEnumerable<AssetResponse> Content { get; set; }
    }
}