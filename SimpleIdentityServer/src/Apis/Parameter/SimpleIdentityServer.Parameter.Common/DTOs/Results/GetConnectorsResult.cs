using SimpleIdentityServer.Module.Feed.Common.Responses;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SimpleIdentityServer.Parameter.Common.DTOs.Results
{
    [DataContract]
    public class GetConnectorsResult
    {
        [DataMember(Name = "connectors")]
        public IEnumerable<ProjectConnectorResponse> Connectors { get; set; }
        [DataMember(Name = "template_connectors")]
        public IEnumerable<ProjectConnectorResponse> TemplateConnectors { get; set; }
    }
}
