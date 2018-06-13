using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SimpleIdentityServer.Parameter.Common.DTOs.Requests
{
    [DataContract]
    public class UpdateConnectorRequest
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "lib")]
        public string Library { get; set; }
        [DataMember(Name = "version")]
        public string Version { get; set; }
        [DataMember(Name = "parameters")]
        public IDictionary<string, string> Parameters { get; set; }
    }
}
