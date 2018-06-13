using SimpleIdentityServer.Module.Feed.Common.Responses;
using System.Collections.Generic;

namespace SimpleIdentityServer.Parameter.Core.Responses
{
    public class GetConnectorsResponse
    {
        public GetConnectorsResponse()
        {
            Connectors = new List<ProjectConnectorResponse>();
            TemplateConnectors = new List<ProjectConnectorResponse>();
        }

        public IEnumerable<ProjectConnectorResponse> Connectors { get; set; }
        public IEnumerable<ProjectConnectorResponse> TemplateConnectors { get; set; }
    }
}
