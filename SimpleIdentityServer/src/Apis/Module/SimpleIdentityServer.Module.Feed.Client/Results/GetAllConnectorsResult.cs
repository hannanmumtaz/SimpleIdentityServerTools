using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.Module.Feed.Common.Responses;
using System.Collections.Generic;

namespace SimpleIdentityServer.Module.Feed.Client.Results
{
    public class GetAllConnectorsResult : BaseResponse
    {
        public IEnumerable<ConnectorResponse> Content { get; set; }
    }
}
