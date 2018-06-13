using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.Module.Feed.Common.Responses;

namespace SimpleIdentityServer.Module.Feed.Client.Results
{
    public class GetConnectorResult : BaseResponse
    {
        public ConnectorResponse Content { get; set; }
    }
}
